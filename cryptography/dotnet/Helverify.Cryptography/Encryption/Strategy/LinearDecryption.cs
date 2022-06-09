using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption.Strategy
{
    public class LinearDecryption: IDecryptionStrategy
    {
        public int Decrypt(BigInteger generator, BigInteger p, BigInteger m)
        {
            int exp = -1;

            bool found = false;

            while (!found)
            {
                exp++;

                if (exp % 10000 == 0)
                {
                    Console.WriteLine($"Thread: {Task.CurrentId} Exponent: {exp}");
                }

                BigInteger exponent = new BigInteger(exp.ToString(), 10);
                BigInteger candidate = generator.ModPow(exponent, p).Mod(p);
                found = candidate.Equals(m);

                if (exp >= 10000000)
                {
                    throw new ArgumentOutOfRangeException(nameof(exp));
                }
            }

            return exp;
        }
    }
}
