﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
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