using System;
using System.Threading.Tasks;
using CheckReceiptSDK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceiptsCore;
using ReceiptsServer.Model;

namespace ReceiptsServer.Controllers
{
    [ApiController]
    public class ReadReceiptController : Controller
    {
        private readonly IItemsProvider _ItemsProvider;

        public ReadReceiptController(IItemsProvider itemsProvider)
        {
            _ItemsProvider = itemsProvider ?? throw new ArgumentNullException(nameof(itemsProvider));
        }

        [HttpGet]
        [Route("api/items")]
        public async Task<IActionResult> Test([FromForm] ReceiptRequest request)
        {
            var items = await _ItemsProvider.GetItemsAsync();
            return Json(items);
        }
    }
}