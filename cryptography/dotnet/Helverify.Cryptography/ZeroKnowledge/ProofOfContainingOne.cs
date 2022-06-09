using Helverify.Cryptography.Helper;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Helverify.Cryptography.ZeroKnowledge
{
    /// <summary>
    /// Bernhard2016
    /// KillerProvotum2020
    /// Chaum1993
    ///
    /// Variation of DCP proof adapted towards only proving that the ciphertext contains 1.
    /// => useful for row and column product == 1 proof
    /// </summary>
    public class ProofOfContainingOne
    {
        private static readonly SecureRandom SecureRandom = new(new DigestRandomGenerator(new Sha256Digest()));

        public BigInteger U { get; }
        public BigInteger V { get; }
        public BigInteger S { get; }

        /// <summary>
        /// Private constructor, use <see cref="Create"/> instead.
        /// </summary>
        /// <param name="u">Proof parameter u</param>
        /// <param name="v">Proof parameter v</param>
        /// <param name="s">Proof parameter s</param>
        private ProofOfContainingOne(BigInteger u, BigInteger v, BigInteger s)
        {
            U = u;
            V = v;
            S = s;
        }

        /// <summary>
        /// Creates a proof that a ciphertext contains the value one.
        /// </summary>
        /// <param name="a">First component of an ElGamal ciphertext</param>
        /// <param name="b">Second component of an ElGamal ciphertext</param>
        /// <param name="h">Public key of an ElGamal cryptosystem</param>
        /// <param name="x">Private key of an ElGamal cryptosystem</param>
        /// <param name="p">Public prime p of an ElGamal cryptosystem</param>
        /// <param name="g">Generator g of an ElGamal cryptosystem</param>
        /// <returns>Proof that a ciphertext contains the value one.</returns>
        public static ProofOfContainingOne Create(BigInteger a, BigInteger b, BigInteger h, BigInteger x, BigInteger p, BigInteger g)
        {
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);

            BigInteger r = new BigInteger(x.BitLength, SecureRandom).Mod(q);

            BigInteger u = g.ModPow(r, p).Mod(p);
            BigInteger v = h.ModPow(r, p).Mod(p);

            BigInteger c = HashHelper.GetHash(q, h, a, b, u, v).Mod(q);

            BigInteger s = r.Add(c.Multiply(x).Mod(q)).Mod(q);

            return new ProofOfContainingOne(u, v, s);
        }

        /// <summary>
        /// Allows to verify that a ciphertext contains the value one.
        /// </summary>
        /// <param name="a">First component of an ElGamal ciphertext</param>
        /// <param name="b">Second component of an ElGamal ciphertext</param>
        /// <param name="h">Public key of an ElGamal cryptosystem</param>
        /// <param name="p">Public prime p of an ElGamal cryptosystem</param>
        /// <param name="g">Generator g of an ElGamal cryptosystem</param>
        /// <returns>True if the ciphertext represents one, false otherwise.</returns>
        public bool Verify(BigInteger a, BigInteger b, BigInteger h, BigInteger p, BigInteger g)
        {
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);

            BigInteger c = HashHelper.GetHash(q, h, a, b, U, V).Mod(q);

            bool firstCondition = g.ModPow(S, p).Mod(p).Equals(U.Multiply(a.ModPow(c, p)).Mod(p));

            BigInteger gInverse = g.ModInverse(p).Mod(p);

            bool secondCondition = h.ModPow(S, p).Mod(p).Equals(V.Multiply(b.Multiply(gInverse).Mod(p).ModPow(c, p).Mod(p)).Mod(p));

            return firstCondition && secondCondition;
        }
    }
}
