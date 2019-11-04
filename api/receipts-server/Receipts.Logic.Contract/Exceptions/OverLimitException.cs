using System;

namespace Receipts.Logic.Contract.Exceptions
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