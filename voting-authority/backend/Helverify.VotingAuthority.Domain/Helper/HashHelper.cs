using System.Security.Cryptography;
using System.Text;
using Helverify.Cryptography.Encryption;

namespace Helverify.VotingAuthority.Domain.Helper
{
    /// <summary>
    /// Encapsulates hash generation functionality
    /// </summary>
    internal class HashHelper
    {
        private SHA256 _sha256 = SHA256.Create();

        /// <summary>
        /// Generates a hash of all specified ciphertexts
        /// </summary>
        /// <param name="ciphers">ElGamal ciphertexts</param>
        /// <returns></returns>
        internal string Hash(params ElGamalCipher[] ciphers)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ElGamalCipher cipher in ciphers)
            {
                string hashC = Hash(cipher.C.ToString(16));
                string hashD = Hash(cipher.D.ToString(16));

                sb.Append(hashC).Append(hashD);
            }

            return Hash(sb.ToString());
        }

        /// <summary>
        /// Generates a hash of all the specified strings.
        /// </summary>
        /// <param name="strs">Strings to be hashed</param>
        /// <returns></returns>
        internal string Hash(params string[] strs)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string str in strs)
            {
                sb.Append(Hash(str));
            }

            return Hash(sb.ToString());
        }

        /// <summary>
        /// Generates the hash of the specified string in hex format.
        /// </summary>
        /// <param name="str">String to be hashed</param>
        /// <returns></returns>
        private string Hash(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            byte[] hash = _sha256.ComputeHash(bytes);

            return ConvertHashToHexString(hash);
        }

        /// <summary>
        /// Converts a hash from byte[] to a hex string representation.
        /// </summary>
        /// <param name="hash">Hash value</param>
        /// <returns>Hex string representation</returns>
        private string ConvertHashToHexString(byte[] hash)
        {
            // Conversion according to: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=net-6.0

            StringBuilder sb = new StringBuilder();
            
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
