using System;

namespace ReceiptsServer.Receipts
{
    public class FailedToAddReceiptException : Exception
    {
        public FailedToAddReceiptException()
        {
        }

        public FailedToAddReceiptException(string message) : base(message)
        {
        }
    }
}