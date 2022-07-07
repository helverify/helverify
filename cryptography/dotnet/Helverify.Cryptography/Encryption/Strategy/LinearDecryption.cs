using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption.Strategy
{
    /// <summary>
    /// Iterating linearly through the exponents of g to find the correct decryption of the ciphertext.
    /// </summary>
    public class LinearDecryption: IDecryptionStrategy
    {
        /// <inheritdoc cref="IDecryptionStrategy.Decrypt"/>
        public int Decrypt(BigInteger cipher, BigInteger p, BigInteger g)
        {
            int exp = -1;

            bool found = false;

            while (!found)
            {
                exp++;
                
                BigInteger exponent = new BigInteger(exp.ToString(), 10);
                BigInteger candidate = g.ModPow(exponent, p).Mod(p);
                found = candidate.Equals(cipher);

                if (exp >= 10000000)
                {
                    throw new ArgumentOutOfRangeException(nameof(exp));
                }
            }

            return exp;
        }
    }
}
