namespace ReceiptsCore.Hash
{
    public interface IHashCalculator
    {
        string Calculate(IHashable hashable);
    }
}