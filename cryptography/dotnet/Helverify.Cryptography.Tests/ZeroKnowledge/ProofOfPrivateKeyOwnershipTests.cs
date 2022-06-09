using Helverify.Cryptography.Common;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Tests.ZeroKnowledge
{
    internal class ProofOfPrivateKeyOwnershipTests
    {
        [Test, TestCaseSource(nameof(GetKeyInputs))]
        public void TestVerify(BigInteger pkCreate, BigInteger skCreate, BigInteger pkVerify, BigInteger p, BigInteger g, bool expected)
        {
            // arrange
            ProofOfPrivateKeyOwnership proof = ProofOfPrivateKeyOwnership.Create(pkCreate, skCreate, p, g);

            // act
            bool isValid = proof.Verify(pkVerify, p, g);

            // assert
            Assert.That(isValid, Is.EqualTo(expected));
        }

        private static IEnumerable<object[]> GetKeyInputs()
        {
            IElGamal elGamal = new ExponentialElGamal();

            DhGroup dhGroup = DhGroups.Get(DhGroups.Modp2048);

            BigInteger p = dhGroup.P;
            BigInteger g = dhGroup.G;
            AsymmetricCipherKeyPair keyPair1 = elGamal.KeyGen(p, g);
            AsymmetricCipherKeyPair keyPair2 = elGamal.KeyGen(p, g);

            BigInteger pk1 = (keyPair1.Public as DHPublicKeyParameters).Y;
            BigInteger sk1 = (keyPair1.Private as DHPrivateKeyParameters).X;
            BigInteger pk2 = (keyPair2.Public as DHPublicKeyParameters).Y;
            BigInteger sk2 = (keyPair2.Private as DHPrivateKeyParameters).X;

            return new[]
            {
                new object[] { pk1, sk1, pk1, p, g, true },
                new object[] { pk2, sk2, pk2, p, g, true },
                new object[] { pk1, sk1, pk2, p, g, false },
                new object[] { pk2, sk2, pk1, p, g, false },
            };
        }
    }
}
