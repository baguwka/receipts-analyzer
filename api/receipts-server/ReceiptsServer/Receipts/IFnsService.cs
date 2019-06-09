using System;
using System.Threading.Tasks;
using CheckReceiptSDK.Results;

namespace ReceiptsServer.Receipts
{
    public interface IFnsService
    {
        Task<CheckResult> IsReceiptExists(string fiscalNumber,
            string fiscalDocument,
            string fiscalSign,
            DateTime date,
            Decimal sum);

        Task<ReceiptResult> ReceiveAsync(
            string fiscalNumber,
            string fiscalDocument,
            string fiscalSign);
    }
}