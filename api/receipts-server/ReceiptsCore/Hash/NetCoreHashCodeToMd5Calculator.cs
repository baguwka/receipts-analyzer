using System;
using ReceiptsCore.Utils;

namespace ReceiptsCore.Hash
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