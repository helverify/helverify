using Helverify.Cryptography.Helper;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Helverify.Cryptography.ZeroKnowledge
{
    /// <summary>
    /// Schnorr1991
    /// Bernhard2016
    /// KillerProvotum2020
    ///
    /// Schnorr Proof
    /// </summary>
    public class ProofOfPrivateKeyOwnership
    {
        private static readonly SecureRandom SecureRandom = new(new DigestRandomGenerator(new Sha256Digest()));

        public BigInteger C { get; }
        public BigInteger D { get; }

        /// <summary>
        /// Constructor is private, use <see cref="Create"/> instead.
        /// </summary>
        /// <param name="c">Proof parameter c</param>
        /// <param name="d">Proof parameter d</param>
        private ProofOfPrivateKeyOwnership(BigInteger c, BigInteger d)
        {
            C = c;
            D = d;
        }

        /// <summary>
        /// Creates a proof that someone owns a private key to a specific public key.
        /// </summary>
        /// <param name="h">Public key of an ElGamal cryptosystem</param>
        /// <param name="x">Private key of an ElGamal cryptosystem</param>
        /// <param name="p">Public prime p of an ElGamal cryptosystem</param>
        /// <param name="g">Generator g of an ElGamal cryptosystem</param>
        /// <returns>Proof of private key ownership</returns>
        public static ProofOfPrivateKeyOwnership Create(BigInteger h, BigInteger x,
            BigInteger p, BigInteger g)
        {
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);

            BigInteger a = new BigInteger(x.BitLength, SecureRandom).Mod(q);

            BigInteger b = g.ModPow(a, p);

            BigInteger c = HashHelper.GetHash(q, h, b);

            BigInteger d = a.Add(c.Multiply(x).Mod(q)).Mod(q);

            return new ProofOfPrivateKeyOwnership(c, d);
        }

        /// <summary>
        /// Allows to verify that someone owns the private key to the specified public key.
        /// </summary>
        /// <param name="h">Public key of an ElGamal cryptosystem</param>
        /// <param name="p">Public prime p of an ElGamal cryptosystem</param>
        /// <param name="g">Generator g of an ElGamal cryptosystem</param>
        /// <returns>True if proof is valid, false if is invalid</returns>
        public bool Verify(BigInteger h, BigInteger p, BigInteger g)
        {
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);

            BigInteger b = g.ModPow(D, p).Multiply(h.ModInverse(p).ModPow(C, p)).Mod(p);

            BigInteger cPrime = HashHelper.GetHash(q, h, b);

            bool condition1 = cPrime.Equals(C);

            bool condition2 = g.ModPow(D, p).Equals(b.Multiply(h.ModPow(C, p)).Mod(p));

            return condition1 && condition2;
        }
    }
}
