using System;

namespace Receipts.Logic.Contract.Exceptions
{
    public class FnsUserNotFoundException : Exception
    {
        public FnsUserNotFoundException()
        {
        }

        public FnsUserNotFoundException(string message) : base(message)
        {
        }
    }
}