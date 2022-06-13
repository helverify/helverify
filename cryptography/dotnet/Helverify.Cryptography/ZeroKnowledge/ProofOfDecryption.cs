using Helverify.Cryptography.Helper;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Helverify.Cryptography.ZeroKnowledge
{
    /// <summary>
    /// Chaum1993
    /// Bernhard2016
    /// KillerProvotum2020
    /// 
    /// Chaum-Pedersen Proof
    /// </summary>
    public class ProofOfDecryption
    {
        private static readonly SecureRandom SecureRandom = new(new DigestRandomGenerator(new Sha256Digest()));

        public BigInteger D { get; }
        public BigInteger U { get; }
        public BigInteger V { get; }
        public BigInteger S { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="d">Proof parameter d</param>
        /// <param name="u">Proof parameter u</param>
        /// <param name="v">Proof parameter v</param>
        /// <param name="s">Proof parameter s</param>
        public ProofOfDecryption(BigInteger d, BigInteger u, BigInteger v, BigInteger s)
        {
            D = d;
            U = u;
            V = v;
            S = s;
        }

        /// <summary>
        /// Creates a proof that the specified ciphertext has been decrypted correctly.
        /// </summary>
        /// <param name="a">First component of an ElGamal ciphertext</param>
        /// <param name="b">Second component of an ElGamal ciphertext</param>
        /// <param name="publicKeyShare">Public key share</param>
        /// <param name="privateKeyShare">Private key share</param>
        /// <returns>Proof of correct decryption</returns>
        public static ProofOfDecryption Create(BigInteger a, BigInteger b, DHPublicKeyParameters publicKeyShare, DHPrivateKeyParameters privateKeyShare)
        {
            BigInteger p = publicKeyShare.Parameters.P;
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);
            BigInteger g = publicKeyShare.Parameters.G;
            BigInteger pk = publicKeyShare.Y;
            BigInteger sk = privateKeyShare.X;

            BigInteger r = new BigInteger(privateKeyShare.X.BitLength, SecureRandom).Mod(q);

            BigInteger u = a.ModPow(r, p).Mod(p);
            BigInteger v = g.ModPow(r, p).Mod(p);

            BigInteger c = HashHelper.GetHash(q, pk, a, b, u, v).Mod(q);

            BigInteger s = r.Add(c.Multiply(sk).Mod(q)).Mod(q);

            BigInteger d = a.ModPow(sk, p).Mod(p);

            return new ProofOfDecryption(d, u, v, s);
        }

        /// <summary>
        /// Allows to verify that the specified ciphertext has been decrypted correctly.
        /// </summary>
        /// <param name="a">First component of an ElGamal ciphertext</param>
        /// <param name="b">Second component of an ElGamal ciphertext</param>
        /// <param name="publicKeyShare">Public key share</param>
        /// <returns>True if decryption has been performed correctly, false otherwise.</returns>
        public bool Verify(BigInteger a, BigInteger b, DHPublicKeyParameters publicKeyShare)
        {
            BigInteger p = publicKeyShare.Parameters.P;
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);
            BigInteger g = publicKeyShare.Parameters.G;
            BigInteger pk = publicKeyShare.Y.Mod(p);

            BigInteger c = HashHelper.GetHash(q, pk, a, b, U, V).Mod(q);

            bool firstCondition = a.Mod(p).ModPow(S, p).Mod(p).Equals(U.Multiply(D.ModPow(c, p).Mod(p)).Mod(p));

            bool secondCondition = g.Mod(p).ModPow(S, p).Mod(p).Equals(V.Multiply(pk.ModPow(c, p).Mod(p)).Mod(p));

            return firstCondition && secondCondition;
        }
    }
}

