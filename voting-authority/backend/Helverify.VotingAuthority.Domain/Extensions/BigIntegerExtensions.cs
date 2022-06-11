using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Extensions
{
    /// <summary>
    /// BigInteger helper
    /// </summary>
    public static class BigIntegerExtensions
    {
        /// <summary>
        /// Converts a BigInteger to a hexadecimal string.
        /// </summary>
        /// <param name="bigInt">Number to be converted</param>
        /// <returns>Hexadecimal string representation of the specified number.</returns>
        public static string ConvertToHexString(this BigInteger? bigInt)
        {
            if (bigInt == null)
            {
                return string.Empty;
            }

            return bigInt.ToString(16);
        }
    }
}
