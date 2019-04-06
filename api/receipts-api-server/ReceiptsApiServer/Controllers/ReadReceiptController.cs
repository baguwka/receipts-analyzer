using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReceiptsApiServer.Model;

namespace ReceiptsApiServer.Controllers
{
    [ApiController]
    public class ReadReceiptController : ControllerBase
    {
        [HttpPost]
        [Route("api/test")]
        public async Task<IActionResult> Test([FromForm] ReceiptRequest request)
        {
            return Ok();
        }
    }
}