using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Receipts.Core.Contract;
using Receipts.Core.Contract.EF.Model;
using Receipts.Logic.Contract;
using Receipts.Logic.Contract.Exceptions;
using Receipts.Logic.Contract.Hash;
using Receipts.Logic.Contract.Receipts;

namespace Receipts.Logic.Receipts
{
    public class DefaultReceiptsProvider : IReceiptsProvider
    {
        private readonly IReceiptsRepository _ReceiptsRepository;
        private readonly IItemsRepository _ItemsRepository;
        private readonly IFnsService _FnsService;
        private readonly IHashCalculator _HashCalculator;
        private readonly ILogger _Logger;

        public DefaultReceiptsProvider(
            [NotNull] IReceiptsRepository receiptsRepository,
            [NotNull] IItemsRepository itemsRepository,
            [NotNull] IFnsService fnsService,
            [NotNull] IHashCalculator hashCalculator,
            [NotNull] ILogger logger)
        {
            _ReceiptsRepository = receiptsRepository ?? throw new ArgumentNullException(nameof(receiptsRepository));
            _ItemsRepository = itemsRepository ?? throw new ArgumentNullException(nameof(itemsRepository));
            _FnsService = fnsService ?? throw new ArgumentNullException(nameof(fnsService));
            _HashCalculator = hashCalculator ?? throw new ArgumentNullException(nameof(hashCalculator));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AddReceiptsResult> AddManyReceipts([NotNull] IEnumerable<ReceiptRequestDto> requests)
        {
            if (requests == null) throw new ArgumentNullException(nameof(requests));

            var added = new List<AddReceiptResult>();
            var skipped = new List<AddReceiptResult>();
            
            foreach (var request in requests)
            {
                try
                {
                    var isAlreadyAdded = await IsReceiptAlreadyAddedAsync(request);
                    if (isAlreadyAdded)
                    {
                        _Logger.LogInformation("Receipt \'{receiptRawData}\' already added to db", request.RawQRData);
                        //todo объединить с IsReceiptAlreadyAddedAsync, убрать вообще это и провероять в query есть ли такой
                        var hash = _HashCalculator.Calculate(request);
                        skipped.Add(new AddReceiptResult
                        {
                            Hash = hash
                        });
                        continue;
                    }
                    
                    var result = await AddReceiptAsync(request);
                    added.Add(result);
                }
                catch (Exception exception)
                {
                    _Logger.LogError(exception, "Failed to add receipt of many for \'{receiptRawData}\'", request.RawQRData);
                }
            }
            
            return new AddReceiptsResult
            {
                Added = added.ToArray()
            };
        }

        public async Task<bool> IsReceiptAlreadyAddedAsync([NotNull] ReceiptRequestDto requestDto)
        {
            if (requestDto == null) throw new ArgumentNullException(nameof(requestDto));
            var hash = _HashCalculator.Calculate(requestDto);

            var isExists = await _ReceiptsRepository.IsReceiptExistsByHashAsync(hash);
            return isExists;
        }

        public async Task<AddReceiptResult> AddReceiptAsync([NotNull] ReceiptRequestDto requestDto)
        {
            if (requestDto == null) throw new ArgumentNullException(nameof(requestDto));
            var hash = _HashCalculator.Calculate(requestDto);

            var isExistsInFnsDb = await _FnsService.IsReceiptExists(requestDto.FiscalNumber, requestDto.FiscalDocument, requestDto.FiscalSign, requestDto.Time,
                requestDto.Sum);
            if (!isExistsInFnsDb.ReceiptExists)
                throw new ReceiptNotExistsInFns("Receipt is not exists in FNS db");

            CheckReceiptSDK.Results.Document doc = null;

            int tries = 6;
            while (tries > 0)
            {
                tries--;
                
                var result = await _FnsService.ReceiveAsync(requestDto.FiscalNumber, requestDto.FiscalDocument, requestDto.FiscalSign);
                doc = result.Document;
                if (result.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    throw new OverLimitException("Free FNS requests limit exceeded. Login as another user or pay to FNS.");
                }

                if (result.StatusCode != HttpStatusCode.Accepted &&
                    result.StatusCode != HttpStatusCode.OK &&
                    result.StatusCode != HttpStatusCode.Created)
                {
                    throw new Exception($"Failed to receive fns data. Status Code: {result.StatusCode}; Details: \'{result.Message}\'");
                }
                if (doc == null && result.StatusCode == HttpStatusCode.Accepted)
                {
                    await Task.Delay(750);
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
                Date = requestDto.Time,
                RawQrData = requestDto.RawQRData,
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
                _Logger.Log(LogLevel.Error, "Failed to add extended receipt for {receiptId}", extended.ReceiptId);
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
                Id = addedReceipt.ReceiptId,
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