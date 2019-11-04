using System;

namespace Receipts.Logic.Contract.Exceptions
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