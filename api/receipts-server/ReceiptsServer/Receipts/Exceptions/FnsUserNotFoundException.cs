using System;

namespace ReceiptsServer.Receipts.Exceptions
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