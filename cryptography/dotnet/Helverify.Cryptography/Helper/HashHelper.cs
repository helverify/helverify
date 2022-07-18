using System.Security.Cryptography;
using System.Text;
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
        /// Inspired by https://stackoverflow.com/questions/35984198/c-sharp-sha256-computehash-result-different-with-cryptojs-sha256-function
        /// to achieve same results as crypto-js sha256
        /// </summary>
        /// <param name="q">Modulus</param>
        /// <param name="numbers">Numbers to be hashed</param>
        /// <returns></returns>
        internal static BigInteger GetHash(BigInteger q, params BigInteger[] numbers)
        {
            SHA256 sha256 = SHA256.Create();

            StringBuilder sb = new StringBuilder();

            foreach (BigInteger number in numbers)
            {
                string hexString = number.ToString(16);

                byte[] bytes = Encoding.UTF8.GetBytes(hexString);

                byte[] numberHash = sha256.ComputeHash(bytes);

                string hashString = string.Empty;
                
                foreach (byte b in numberHash)
                {
                    hashString += $"{b:x2}";
                }

                sb.Append(hashString);
            }

            string combinedHash = sb.ToString();

            byte[] combinedBytes = Encoding.UTF8.GetBytes(combinedHash);

            byte[] hash = sha256.ComputeHash(combinedBytes);

            return new BigInteger(1, hash).Mod(q);
        }
    }
}
