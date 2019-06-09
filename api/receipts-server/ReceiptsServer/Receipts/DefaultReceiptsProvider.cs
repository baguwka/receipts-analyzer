using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CheckReceiptSDK.Results;
using JetBrains.Annotations;
using ReceiptsCore;
using ReceiptsCore.EF.Model;
using ReceiptsCore.Hash;
using ReceiptsServer.Model;
using ReceiptsServer.Receipts.Exceptions;
using Item = ReceiptsCore.EF.Model.Item;
using Receipt = ReceiptsCore.EF.Model.Receipt;

namespace ReceiptsServer.Receipts
{
    public class DefaultReceiptsProvider : IReceiptsProvider
    {
        private readonly IReceiptsRepository _ReceiptsRepository;
        private readonly IItemsRepository _ItemsRepository;
        private readonly IFnsService _FnsService;
        private readonly IHashCalculator _HashCalculator;

        public DefaultReceiptsProvider(
            [NotNull] IReceiptsRepository receiptsRepository,
            [NotNull] IItemsRepository itemsRepository,
            [NotNull] IFnsService fnsService,
            [NotNull] IHashCalculator hashCalculator)
        {
            _ReceiptsRepository = receiptsRepository ?? throw new ArgumentNullException(nameof(receiptsRepository));
            _ItemsRepository = itemsRepository ?? throw new ArgumentNullException(nameof(itemsRepository));
            _FnsService = fnsService ?? throw new ArgumentNullException(nameof(fnsService));
            _HashCalculator = hashCalculator ?? throw new ArgumentNullException(nameof(hashCalculator));
        }

        public async Task<bool> IsReceiptAlreadyAddedAsync([NotNull] ReceiptRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var hash = _HashCalculator.Calculate(request);

            var isExists = await _ReceiptsRepository.IsReceiptExistsByHashAsync(hash);
            return isExists;
        }

        public async Task<AddReceiptResult> AddReceiptAsync([NotNull] ReceiptRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var hash = _HashCalculator.Calculate(request);

            var isExistsInFnsDb = await _FnsService.IsReceiptExists(request.FiscalNumber, request.FiscalDocument, request.FiscalSign, request.Time,
                request.Sum);
            if (!isExistsInFnsDb.ReceiptExists)
                throw new ReceiptNotExistsInFns("Receipt is not exists in FNS db");

            Document doc = null;

            int tries = 3;
            while (tries > 0)
            {
                tries--;
                
                var result = await _FnsService.ReceiveAsync(request.FiscalNumber, request.FiscalDocument, request.FiscalSign);
                doc = result.Document;
                if (result.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    throw new OverLimitException("Free FNS requests limit exceeded. Login as another user or pay to FNS.");
                }
                if (doc == null && result.StatusCode == HttpStatusCode.Accepted)
                {
                    await Task.Delay(500);
                    continue;
                }

                break;
            }

            //todo вместо этого, реализовать очередь на добавление, что бы сервис сам добавил рецепт, когда фнс станет доступен.
            if (doc == null)
                throw new FnsProbablyUnavailableException("FNS Service currently can be unavailable. Please try later.");

            var receipt = new Receipt
            {
                Hash = hash,
                Date = request.Time,
                RawQrData = request.RawQRData,
                //todo после реализации юзеров, получать id реального авторизованного юзера веб-сервиса.
                UserId = 1
            };

            var addedReceipt = await _ReceiptsRepository.AddReceiptAsync(receipt);
            if (addedReceipt == null)
                throw new FailedToAddReceiptException("Failed to add receipt to db");

            var extended = new ReceiptExtended
            {
                ReceiptId = addedReceipt.ReceiptId,
                Cashier = doc.Receipt.Cashier,
                StoreName = doc.Receipt.StoreName,
                RetailAddress = doc.Receipt.RetailPlaceAddress,
                KktRegId = doc.Receipt.KktRegId,
                ShiftNumber = doc.Receipt.ShiftNumber.ToString(),
                RetailInn = doc.Receipt.RetailInn
            };

            var addedExtended = await _ReceiptsRepository.AddExtendedInfoToReceipt(extended);
            if (addedExtended == null)
            {
                //todo what to do? we don't care actually much, but need logs or something
            }

            var addedItems = new List<Item>();

            foreach (var receiptItem in doc.Receipt.Items)
            {
                var item = new Item()
                {
                    Sum = receiptItem.Sum,
                    Name = receiptItem.Name,
                    Price = receiptItem.Price,
                    Quantity = (int)receiptItem.Quantity,
                    ReceiptId = addedReceipt.ReceiptId
                };
                var addedItem = await _ItemsRepository.AddItem(item);
                addedItems.Add(addedItem);
            }

            return new AddReceiptResult
            {
                Hash = addedReceipt.Hash,
                AddedItemsCount = addedItems.Count
            };
        }

        public async Task<GetReceiptsResult> GetReceiptsAsync()
        {
            var rawReceipts = await _ReceiptsRepository.GetReceiptsAsync();
            return new GetReceiptsResult
            {
                Receipts = rawReceipts
                    .Select(ReceiptModel.FromDbReceipt)
                    .ToImmutableList()
            };
        }
    }
}