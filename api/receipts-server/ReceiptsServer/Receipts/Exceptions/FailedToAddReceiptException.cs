using System;

namespace ReceiptsServer.Receipts.Exceptions
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