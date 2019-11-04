namespace Receipts.Logic.Contract.Hash
{
    public interface IHashCalculator
    {
        string Calculate(IHashable hashable);
    }
}