using System.Threading.Tasks;
using ReceiptsServer.Model;

namespace ReceiptsServer.Receipts
{
    public interface IReceiptsProvider
    {
        Task<bool> IsReceiptAlreadyAddedAsync(ReceiptRequest request);
        Task<AddReceiptResult> AddReceiptAsync(ReceiptRequest request);
        Task<GetReceiptsResult> GetReceiptsAsync();
    }
}