using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckReceiptSDK;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ReceiptsServer.Model;

namespace ReceiptsServer.Controllers
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