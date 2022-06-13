using System.Security.Cryptography;
using System.Text;
using Helverify.Cryptography.Encryption;

namespace Helverify.VotingAuthority.Domain.Helper
{
    internal static class HashHelper
    {
        internal static string Hash(params ElGamalCipher[] ciphers)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] hashes = Array.Empty<byte>();

            foreach (ElGamalCipher cipher in ciphers)
            {
                byte[] hashC = sha256.ComputeHash(cipher.C.ToByteArray());
                byte[] hashD = sha256.ComputeHash(cipher.D.ToByteArray());
                hashes = hashes.Concat(hashC.Concat(hashD).ToArray()).ToArray();
            }

            // Conversion according to: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=net-6.0

            StringBuilder sb = new StringBuilder();

            byte[] ciphersHash = sha256.ComputeHash(hashes);

            for (int i = 0; i < ciphers.Length; i++)
            {
                sb.Append(ciphersHash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
