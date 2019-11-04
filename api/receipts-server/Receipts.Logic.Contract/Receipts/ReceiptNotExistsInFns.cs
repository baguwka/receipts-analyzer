using System;

namespace Receipts.Logic.Contract.Receipts
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