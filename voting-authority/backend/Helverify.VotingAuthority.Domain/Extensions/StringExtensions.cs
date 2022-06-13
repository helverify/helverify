using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Extensions
{
    /// <summary>
    /// String helper
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Converts a hexadecimal string to a BigInteger
        /// </summary>
        /// <param name="str">String to be converted</param>
        /// <returns>BigInteger representation of the specified string.</returns>
        public static BigInteger ConvertToBigInteger(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return BigInteger.Zero;
            }

            return new BigInteger(str, 16);
        }
    }
}
