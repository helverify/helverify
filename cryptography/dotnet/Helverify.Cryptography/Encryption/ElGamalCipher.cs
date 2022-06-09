using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption
{
    /// <summary>
    /// Represents an ElGamal ciphertext.
    /// </summary>
    public class ElGamalCipher
    {
        /// <summary>
        /// First component of ciphertext.
        /// </summary>
        public BigInteger C { get; }
        
        /// <summary>
        /// Second component of ciphertext.
        /// </summary>
        public BigInteger D { get; }

        /// <summary>
        /// Randomness used for the encryption.
        /// </summary>
        public BigInteger R { get; } // considered a secret value, but needed for proof of containing 1 and proof of containing 0 or 1

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c">First component</param>
        /// <param name="d">Second component</param>
        /// <param name="r">Randomness</param>
        public ElGamalCipher(BigInteger c, BigInteger d, BigInteger r)
        {
            C = c;
            D = d;
            R = r;
        }

        /// <summary>
        /// Homomorphically combine two ciphertexts.
        /// </summary>
        /// <param name="otherCipher">The ciphertext to be added.</param>
        /// <param name="p">Prime</param>
        /// <returns>Combined ciphertext.</returns>
        public ElGamalCipher Add(ElGamalCipher otherCipher, BigInteger p)
        {
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);

            BigInteger c = C.Multiply(otherCipher.C).Mod(p);
            BigInteger d = D.Multiply(otherCipher.D).Mod(p);
            BigInteger r = R.Add(otherCipher.R).Mod(q);

            return new ElGamalCipher(c, d, r);
        }
    }
}
