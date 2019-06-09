using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CheckReceiptSDK;
using CheckReceiptSDK.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReceiptsCore;
using ReceiptsServer.Model;
using ReceiptsServer.Receipts;
using Item = ReceiptsCore.EF.Model.Item;
using Receipt = ReceiptsCore.EF.Model.Receipt;

namespace ReceiptsServer.Controllers
{
    [ApiController]
    public class ReceiptsController : Controller
    {
        private readonly IFnsUsersRepository _FnsUsersRepository;
        private readonly IItemsRepository _ItemsRepository;
        private readonly IReceiptsProvider _ReceiptsProvider;

        public ReceiptsController(
            IFnsUsersRepository fnsUsersRepository,
            IItemsRepository itemsRepository,
            IReceiptsProvider receiptsProvider)
        {
            _FnsUsersRepository = fnsUsersRepository ?? throw new ArgumentNullException(nameof(fnsUsersRepository));
            _ItemsRepository = itemsRepository ?? throw new ArgumentNullException(nameof(itemsRepository));
            _ReceiptsProvider = receiptsProvider ?? throw new ArgumentNullException(nameof(receiptsProvider));
        }

        [HttpGet]
        [Route("api/items")]
        public async Task<IActionResult> GetReceipt()
        {
            var items = await _ItemsRepository.GetItemsAsync();
            return Json(items);
        }
        //t=20190411T204900&s=1544.46&fn=9289000100255640&i=32050&fp=3926647768&n=1

        [HttpGet]
        [Route("api/receipts")]
        public async Task<IActionResult> GetReceipts()
        {
            var receipts = await _ReceiptsProvider.GetReceiptsAsync();

            return Json(receipts, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }

        [HttpPost]
        [Route("api/receipts")]
        public async Task<IActionResult> AddReceipt([FromForm] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Parameter \'query must not be null or empty\'");

            var request = ReceiptRequest.FromQuery(query);
            if (request == null || request.IsValid() == false)
                return BadRequest("Check your query string, it must be incorrect.");

            var isAlreadyAdded = await _ReceiptsProvider.IsReceiptAlreadyAddedAsync(request);

            if (isAlreadyAdded)
                return BadRequest("This receipt already added");

            var addResult = await _ReceiptsProvider.AddReceiptAsync(request);

            return Ok($"Ok! Receipt with hash {addResult.Hash} added to db with {addResult.AddedItemsCount} items.");
        }
    }
}