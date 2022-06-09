using System.Security.Cryptography;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Helper
{
    internal static class HashHelper
    {
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
