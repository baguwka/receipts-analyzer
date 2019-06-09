using System;

namespace ReceiptsServer.Receipts
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