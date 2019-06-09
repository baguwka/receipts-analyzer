using System;

namespace ReceiptsServer.Receipts.Exceptions
{
    public class FnsProbablyUnavailableException : Exception
    {
        public FnsProbablyUnavailableException()
        {
        }

        public FnsProbablyUnavailableException(string message) : base(message)
        {
        }
    }
}