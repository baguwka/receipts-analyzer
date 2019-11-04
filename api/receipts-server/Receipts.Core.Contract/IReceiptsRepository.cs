using System.Collections.Generic;
using System.Threading.Tasks;
using Receipts.Core.Contract.EF.Model;

namespace Receipts.Core.Contract
{
    public interface IReceiptsRepository
    {
        Task<IReadOnlyCollection<Receipt>> GetReceiptsAsync();
        Task<Receipt> GetReceiptByHashAsync(string hash);
        Task<Receipt> AddReceiptAsync(Receipt receipt);
        Task<bool> IsReceiptExistsByHashAsync(string hash);
        Task<ReceiptExtended> AddExtendedInfoToReceipt(ReceiptExtended extended);
    }
}