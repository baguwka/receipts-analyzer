using System;

namespace ReceiptsServer.Receipts
{
    public class ReceiptNotExistsInFns : Exception
    {
        public ReceiptNotExistsInFns()
        {
        }

        public ReceiptNotExistsInFns(string message) : base(message)
        {
        }
    }
}