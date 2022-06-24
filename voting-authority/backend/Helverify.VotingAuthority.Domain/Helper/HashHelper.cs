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
            byte[] hashes = Array.Empty<byte>();

            foreach (ElGamalCipher cipher in ciphers)
            {
                byte[] hashC = _sha256.ComputeHash(cipher.C.ToByteArray());
                byte[] hashD = _sha256.ComputeHash(cipher.D.ToByteArray());
                hashes = hashes.Concat(hashC.Concat(hashD).ToArray()).ToArray();
            }

            byte[] ciphersHash = _sha256.ComputeHash(hashes);

            return ConvertHashToHexString(ciphersHash);
        }

        /// <summary>
        /// Generates a hash of all the specified strings.
        /// </summary>
        /// <param name="strs">Strings to be hashed</param>
        /// <returns></returns>
        internal string Hash(params string[] strs)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] hashes = Array.Empty<byte>();

            foreach (string str in strs)
            {
                byte[] h = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                
                hashes = hashes.Concat(h).ToArray();
            }

            return ConvertHashToHexString(hashes);
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
