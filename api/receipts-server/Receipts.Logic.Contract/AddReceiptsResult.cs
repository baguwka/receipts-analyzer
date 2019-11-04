namespace Receipts.Logic.Contract
{
    public class AddReceiptsResult
    {
        public AddReceiptResult[] Added { get; set; } = new AddReceiptResult[0];
        public AddReceiptResult[] Skipped { get; set; } = new AddReceiptResult[0];
    }
}