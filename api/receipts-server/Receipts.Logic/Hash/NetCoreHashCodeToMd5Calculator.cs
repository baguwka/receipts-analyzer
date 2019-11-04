using System;
using Receipts.Logic.Contract.Hash;
using ReceiptsCore.Utils;

namespace Receipts.Logic.Hash
{
    [Obsolete]
    public class NetCoreHashCodeToMd5Calculator : IHashCalculator
    {
        public string Calculate(IHashable hashable)
        {
            var hashCode = hashable.GetHashCode();
            var md5 = hashCode.ToString().ToMd5();
            return md5;
        }
    }
}