﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public interface IReceiptsProvider
    {
        Task<IReadOnlyCollection<Receipt>> GetReceipts();
        Task<Receipt> GetReceiptByHash(string hash);
        Task<Receipt> AddReceipt(Receipt receipt);
    }
}