using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckReceiptSDK;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ReceiptsServer.Model;

namespace ReceiptsServer.Controllers
{
    [ApiController]
    public class ReadReceiptController : Controller
    {
        [HttpGet]
        [Route("api/items")]
        public async Task<IActionResult> Test([FromForm] ReceiptRequest request)
        {
            using (var db = new ApplicationContext())
            {
                var items = await db.Items.ToListAsync();
                return Json(new
                {
                    items = items
                });
            }
        }
    }
}