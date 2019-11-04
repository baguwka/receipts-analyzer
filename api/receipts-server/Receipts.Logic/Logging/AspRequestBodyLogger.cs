using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Receipts.Logic.Contract.Logging;

namespace Receipts.Logic.Logging
{
    public class AspRequestBodyLogger : IRequestBodyLogger
    {
        private readonly ILogger _Logger;

        public AspRequestBodyLogger(
            ILogger logger)
        {
            _Logger = logger;
        }
        
        public async Task Log(HttpRequest request)
        {
            if (request?.Body?.CanSeek ?? false)
            {
                request.Body.Position = 0;
                using (var reader = new StreamReader(request.Body))
                {
                    var content = await reader.ReadToEndAsync();
                    
                    _Logger.Log(LogLevel.Information, content);
                }
            }
        }
    }
}