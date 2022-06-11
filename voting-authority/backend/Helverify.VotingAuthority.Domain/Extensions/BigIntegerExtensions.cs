using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Extensions
{
    public static class BigIntegerExtensions
    {
        public static string ExportToHexString(this BigInteger? bigInt)
        {
            if (bigInt == null)
            {
                return string.Empty;
            }

            return bigInt.ToString(16);
        }
    }
}
