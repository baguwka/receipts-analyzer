using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Receipts.Core.Contract;

namespace Receipts.Logic.Api.Controllers
{
    [Route("api/items")]
    public class ItemsController : Controller
    {
        private readonly IItemsRepository _ItemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            _ItemsRepository = itemsRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _ItemsRepository.GetItemsAsync();
            return Json(items);
        }
    }
}