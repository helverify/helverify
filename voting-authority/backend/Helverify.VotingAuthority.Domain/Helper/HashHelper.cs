using System.Security.Cryptography;
using System.Text;
using Helverify.Cryptography.Encryption;

namespace Helverify.VotingAuthority.Domain.Helper
{
    internal static class HashHelper
    {
        private static SHA256 _sha256 = SHA256.Create();

        internal static string Hash(params ElGamalCipher[] ciphers)
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

        internal static string Hash(params string[] strs)
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

        private static string ConvertHashToHexString(byte[] hash)
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
