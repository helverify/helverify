using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Extensions
{
    public static class StringExtensions
    {
        public static BigInteger ExportToBigInteger(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return BigInteger.Zero;
            }

            return new BigInteger(str, 16);
        }
    }
}
