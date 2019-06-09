using System;

namespace ReceiptsServer.Receipts.Exceptions
{
    public class OverLimitException : Exception
    {
        public OverLimitException()
        {
        }

        public OverLimitException(string message) : base(message)
        {
        }
    }
}