using System.Security.Cryptography;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Helper
{
    /// <summary>
    /// Helper to calculate hashes consistently across the library.
    /// </summary>
    internal static class HashHelper
    {
        /// <summary>
        /// Returns the SHA-256 hash for the specified numbers within the modulus of q.
        /// </summary>
        /// <param name="q">Modulus</param>
        /// <param name="numbers">Numbers to be hashed</param>
        /// <returns></returns>
        internal static BigInteger GetHash(BigInteger q, params BigInteger[] numbers)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] combined = Array.Empty<byte>();

            foreach (BigInteger number in numbers)
            {
                byte[] numberHash = sha256.ComputeHash(number.ToByteArray());
                combined = combined.Concat(numberHash).ToArray();
            }

            byte[] hash = sha256.ComputeHash(combined);

            return new BigInteger(1, hash).Mod(q);
        }
    }
}
