using System;

namespace Receipts.Logic.Contract.Exceptions
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