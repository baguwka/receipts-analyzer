using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Receipts.Logic.Contract.Logging
{
    public interface IRequestBodyLogger
    {
        Task Log(HttpRequest request);
    }
}