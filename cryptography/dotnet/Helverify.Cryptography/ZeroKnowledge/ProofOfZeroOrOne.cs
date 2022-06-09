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
    ///
    /// Disjunctive Chaum-Pedersen Proof
    /// </summary>
    public class ProofOfZeroOrOne
    {
        private static readonly SecureRandom SecureRandom = new(new DigestRandomGenerator(new Sha256Digest()));

        public BigInteger U0 { get; }
        public BigInteger U1 { get; }
        public BigInteger V0 { get; }
        public BigInteger V1 { get; }
        public BigInteger C0 { get; }
        public BigInteger C1 { get; }
        public BigInteger R0 { get; }
        public BigInteger R1 { get; }

        private ProofOfZeroOrOne(BigInteger u0, BigInteger u1, BigInteger v0, BigInteger v1, BigInteger c0, BigInteger c1, BigInteger r0, BigInteger r1)
        {
            U0 = u0;
            U1 = u1;
            V0 = v0;
            V1 = v1;
            C0 = c0;
            C1 = c1;
            R0 = r0;
            R1 = r1;
        }

        public static ProofOfZeroOrOne Create(int message, BigInteger a, BigInteger b, BigInteger h, BigInteger x,
            BigInteger p, BigInteger g)
        {
            if (message == 0)
            {
                return CreateZero(a, b, h, x, p, g);
            }

            if (message == 1)
            {
                return CreateOne(a, b, h, x, p, g);
            }

            throw new Exception($"Message must be either 0 or 1, not {message}");
        }

        private static ProofOfZeroOrOne CreateOne(BigInteger a, BigInteger b, BigInteger h, BigInteger x, BigInteger p, BigInteger g)
        {
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);
            BigInteger r0 = new BigInteger(x.BitLength, SecureRandom).Mod(q);
            BigInteger c0 = new BigInteger(x.BitLength, SecureRandom).Mod(q);

            // simulated proof for vote = 0
            BigInteger u0 = g.ModPow(r0, p).Multiply(a.ModInverse(p).ModPow(c0, p)).Mod(p);
            BigInteger v0 = h.ModPow(r0, p).Multiply(b.ModInverse(p).ModPow(c0, p)).Mod(p);

            // actual proof for vote = 1
            BigInteger rnd1 = new BigInteger(x.BitLength, SecureRandom).Mod(q);
            BigInteger u1 = g.ModPow(rnd1, p);
            BigInteger v1 = h.ModPow(rnd1, p);

            // challenge
            BigInteger c = HashHelper.GetHash(q, h, a, b, u0, v0, u1, v1);
            BigInteger c1 = c.Subtract(c0).Mod(q);
            BigInteger r1 = rnd1.Add(c1.Multiply(x).Mod(q)).Mod(q);

            return new ProofOfZeroOrOne(u0, u1, v0, v1, c0, c1, r0, r1);
        }

        private static ProofOfZeroOrOne CreateZero(BigInteger a, BigInteger b, BigInteger h, BigInteger x, BigInteger p, BigInteger g)
        {
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);
            BigInteger r1 = new BigInteger(x.BitLength, SecureRandom).Mod(q);
            BigInteger c1 = new BigInteger(x.BitLength, SecureRandom).Mod(q);

            // simulated proof for vote = 1
            BigInteger bPrime = b.Multiply(g.ModInverse(p)).Mod(p);
            BigInteger u1 = g.ModPow(r1, p).Multiply(a.ModInverse(p).ModPow(c1, p)).Mod(p);
            BigInteger v1 = h.ModPow(r1, p).Multiply(bPrime.ModPow(c1, p).ModInverse(p)).Mod(p);

            // actual proof for vote = 0
            BigInteger rnd0 = new BigInteger(x.BitLength, SecureRandom).Mod(q);
            BigInteger u0 = g.ModPow(rnd0, p);
            BigInteger v0 = h.ModPow(rnd0, p);

            // challenge
            BigInteger c = HashHelper.GetHash(q, h, a, b, u0, v0, u1, v1);
            BigInteger c0 = c.Subtract(c1).Mod(q);
            BigInteger r0 = rnd0.Add(c0.Multiply(x).Mod(q)).Mod(q);

            return new ProofOfZeroOrOne(u0, u1, v0, v1, c0, c1, r0, r1);
        }

        public bool Verify(BigInteger a, BigInteger b, BigInteger h, BigInteger p, BigInteger g)
        {
            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);

            BigInteger c = HashHelper.GetHash(q, h, a, b, U0, V0, U1, V1);

            BigInteger gInverse = g.ModInverse(p).Mod(p);

            bool condition1 = g.ModPow(R0, p).Equals(U0.Multiply(a.ModPow(C0, p).Mod(p)).Mod(p));
            bool condition2 = g.ModPow(R1, p).Equals(U1.Multiply(a.ModPow(C1, p).Mod(p)).Mod(p));
            bool condition3 = h.ModPow(R0, p).Equals(V0.Multiply(b.ModPow(C0, p).Mod(p)).Mod(p));
            bool condition4 = h.ModPow(R1, p).Equals(V1.Multiply(b.Multiply(gInverse).Mod(p).ModPow(C1, p)).Mod(p));
            bool condition5 = C0.Add(C1).Mod(q).Equals(c);

            return condition1 && condition2 && condition3 && condition4 && condition5;
        }
    }
}
