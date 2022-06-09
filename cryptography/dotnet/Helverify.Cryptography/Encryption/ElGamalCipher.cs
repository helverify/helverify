using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption
{
    public class ElGamalCipher
    {
        public BigInteger C { get; }
        public BigInteger D { get; }

        public BigInteger R { get; } // considered a secret value, but needed for proof of containing 1 and proof of containing 0 or 1

        public ElGamalCipher(BigInteger c, BigInteger d, BigInteger r)
        {
            C = c;
            D = d;
            R = r;
        }

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
