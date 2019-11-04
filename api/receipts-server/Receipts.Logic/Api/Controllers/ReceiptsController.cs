using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Receipts.Logic.Contract;
using Receipts.Logic.Contract.Exceptions;
using Receipts.Logic.Contract.Logging;
using Receipts.Logic.Contract.Receipts;

namespace Receipts.Logic.Api.Controllers
{
    [Route("api/receipts")]
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsProvider _ReceiptsProvider;
        private readonly IRequestBodyLogger _RequestBodyLogger;

        public ReceiptsController(
            IReceiptsProvider receiptsProvider,
            IRequestBodyLogger requestBodyLogger)
        {
            _ReceiptsProvider = receiptsProvider ?? throw new ArgumentNullException(nameof(receiptsProvider));
            _RequestBodyLogger = requestBodyLogger;
        }

        //t=20190411T204900&s=1544.46&fn=9289000100255640&i=32050&fp=3926647768&n=1

        [HttpGet]
        public async Task<GetReceiptsResult> GetReceipts() => await _ReceiptsProvider.GetReceiptsAsync();

        [HttpPut]
        public async Task<AddReceiptsResult> AddManyReceipts([FromBody] AddManyReceiptsDto addManyReceiptsDto)
        {
            await _RequestBodyLogger.Log(Request);

            //todo move validation to service and add invalid ones to Skipped array
            var validRequests = addManyReceiptsDto.Receipts?
                .Select(FromQuery)
                .Where(r => r.IsValid())
                .ToArray();
            
            if (validRequests == null || validRequests.Any() == false)
                throw new BadRequestException("None of given query strings are correct. Please ensure that queries to be correct.");

            return await _ReceiptsProvider.AddManyReceipts(validRequests);
        }

        [Obsolete("Use PUT overload")]
        [HttpPost]
        public async Task<IActionResult> AddReceipt([FromForm] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Parameter \'query must not be null or empty\'");

            var request = FromQuery(query);
            if (request == null || request.IsValid() == false)
                return BadRequest("Check your query string, it must be incorrect.");

            var isAlreadyAdded = await _ReceiptsProvider.IsReceiptAlreadyAddedAsync(request);

            if (isAlreadyAdded)
                return BadRequest("This receipt already added");

            var addResult = await _ReceiptsProvider.AddReceiptAsync(request);

            return Ok($"Ok! Receipt with hash {addResult.Hash} added to db with {addResult.AddedItemsCount} items.");
        }

        private static ReceiptRequestDto FromQuery(string query)
        {
            var dict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(query);
            return FromDictionary(query, dict);
        }
        
        private static ReceiptRequestDto FromDictionary(string rawQuery, Dictionary<string, StringValues> dict)
        {
            try
            {
                var singleValuesDict = dict
                    .ToDictionary(pair => pair.Key, pair => pair.Value.FirstOrDefault());

                var json = JsonConvert.SerializeObject(singleValuesDict);
                var obj = JsonConvert.DeserializeObject<ReceiptRequestDto>(json);
                obj.RawQRData = rawQuery;
                if (DateTime.TryParseExact(obj.TimeRaw, new[]
                {
                    "yyyyMMddTHHmmss",
                    "yyyyMMddTHHmm"
                }, null, DateTimeStyles.None, out var result))
                {
                    obj.Time = result;
                }

                return obj;
            }
            catch (Exception)
            {
                //todo logs
                return null;
            }
        }
    }
}