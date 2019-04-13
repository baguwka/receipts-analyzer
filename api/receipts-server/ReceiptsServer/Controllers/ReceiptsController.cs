using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CheckReceiptSDK;
using CheckReceiptSDK.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReceiptsCore;
using ReceiptsServer.Model;
using Item = ReceiptsCore.EF.Model.Item;
using Receipt = ReceiptsCore.EF.Model.Receipt;

namespace ReceiptsServer.Controllers
{
    [ApiController]
    public class ReceiptsController : Controller
    {
        private readonly IUsersProvider _UsersProvider;
        private readonly IItemsProvider _ItemsProvider;
        private readonly IReceiptsProvider _ReceiptsProvider;

        public ReceiptsController(
            IUsersProvider usersProvider,
            IItemsProvider itemsProvider,
            IReceiptsProvider receiptsProvider)
        {
            _UsersProvider = usersProvider ?? throw new ArgumentNullException(nameof(usersProvider));
            _ItemsProvider = itemsProvider ?? throw new ArgumentNullException(nameof(itemsProvider));
            _ReceiptsProvider = receiptsProvider;
        }

        [HttpGet]
        [Route("api/items")]
        public async Task<IActionResult> GetReceipt()
        {
            var items = await _ItemsProvider.GetItemsAsync();
            return Json(items);
        }
        //t=20190411T204900&s=1544.46&fn=9289000100255640&i=32050&fp=3926647768&n=1

        [HttpPost]
        [Route("api/addReceipt")]
        public async Task<IActionResult> AddReceipt()
        {
            var user = await _UsersProvider.GetMainUserAsync();
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Can't find user to authorize to FNS. Internal server problem. Check db.");

            var request = ReceiptRequest.FromQuery(Request.Query);
            if (request == null || request.IsValid() == false)
                return BadRequest("Check your query string, it must be incorrect.");

            var hash = request.ToMd5();
            var existedReceipt = await _ReceiptsProvider.GetReceiptByHash(hash);
            if (existedReceipt != null)
                return BadRequest("This receipt already added");

            var checkResult = await FNS.CheckAsync(request.FiscalNumber, request.FiscalDocument, request.FiscalSign, request.Time, request.Sum);
            if (!checkResult.ReceiptExists)
                return NotFound("Receipt is not exists");

            Document doc = null;

            int tries = 3;
            while (tries > 0)
            {
                tries--;
                var result = await FNS.ReceiveAsync(request.FiscalNumber, request.FiscalDocument, request.FiscalSign, user.Username, user.Password);
                doc = result.Document;
                if (doc == null)
                {
                    await Task.Delay(100);
                    continue;
                }

                break;
            }

            if (doc == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Please try later");

            var receipt = new Receipt
            {
                Hash = hash,
                Date = request.Time,
                RawQrData = Request.QueryString.Value,
            };

            var addedReceipt = await _ReceiptsProvider.AddReceipt(receipt);
            if (addedReceipt == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add receipt to db");

            var addedItems = new List<Item>();

            foreach (var receiptItem in doc.Receipt.Items)
            {
                var item = new Item
                {
                   Sum = receiptItem.Sum,
                   Name = receiptItem.Name,
                   Price = receiptItem.Price,
                   Quantity = (int)receiptItem.Quantity,
                   ReceiptId = addedReceipt.ReceiptId
                };
                var addedItem = await _ItemsProvider.AddItem(item);
                addedItems.Add(addedItem);
            }

            return Ok($"Ok! Receipt with hash {hash} added to db with {addedItems.Count} items.");
        }
    }
}