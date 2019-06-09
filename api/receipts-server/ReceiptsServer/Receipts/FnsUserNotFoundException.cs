using System;

namespace ReceiptsServer.Receipts
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