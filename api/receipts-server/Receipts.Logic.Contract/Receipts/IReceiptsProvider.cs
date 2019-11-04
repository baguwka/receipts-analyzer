using System.Threading.Tasks;

namespace Receipts.Logic.Contract.Receipts
{
    public interface IReceiptsProvider
    {
        Task<bool> IsReceiptAlreadyAddedAsync(ReceiptRequestDto requestDto);
        Task<AddReceiptResult> AddReceiptAsync(ReceiptRequestDto requestDto);
        Task<GetReceiptsResult> GetReceiptsAsync();
    }
}