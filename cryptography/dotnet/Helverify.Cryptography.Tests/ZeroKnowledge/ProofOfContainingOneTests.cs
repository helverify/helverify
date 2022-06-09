using Helverify.Cryptography.Common;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Tests.ZeroKnowledge
{
    internal class ProofOfContainingOneTests
    {
        [Test, TestCaseSource(nameof(_getMessages))]
        public void TestVerify(int message1, int message2, int message3, bool expectedValidity)
        {
            // arrange
            IElGamal elGamal = new ExponentialElGamal();

            DhGroup dhGroup = DhGroups.Get(DhGroups.Modp2048);
            
            BigInteger p = dhGroup.P;
            BigInteger g = dhGroup.G;

            AsymmetricCipherKeyPair keyPair1 = elGamal.KeyGen(p, g);
            AsymmetricCipherKeyPair keyPair2 = elGamal.KeyGen(p, g);
            AsymmetricCipherKeyPair keyPair3 = elGamal.KeyGen(p, g);

            DHPublicKeyParameters combinedPublicKey = elGamal.CombinePublicKeys(new List<DHPublicKeyParameters>
                {
                    (keyPair1.Public as DHPublicKeyParameters)!,
                    (keyPair2.Public as DHPublicKeyParameters)!,
                    (keyPair3.Public as DHPublicKeyParameters)!
                },
                elGamal.GetParameters(p, g));

            ElGamalCipher cipher1 = elGamal.Encrypt(message1, combinedPublicKey);
            ElGamalCipher cipher2 = elGamal.Encrypt(message2, combinedPublicKey);
            ElGamalCipher cipher3 = elGamal.Encrypt(message3, combinedPublicKey);

            ElGamalCipher combinedCipher = cipher1.Add(cipher2, p).Add(cipher3, p);

            ProofOfContainingOne proof = ProofOfContainingOne.Create(combinedCipher.C, combinedCipher.D,
                combinedPublicKey.Y, combinedCipher.R, p, g);

            // act
            bool isValid = proof.Verify(combinedCipher.C, combinedCipher.D, combinedPublicKey.Y, p, g);

            // assert
            Assert.That(isValid, Is.EqualTo(expectedValidity));
        }

        private static object[] _getMessages =
        {
            new object[]{0, 0, 0, false},
            new object[]{1, 0, 0, true},
            new object[]{0, 1, 0, true},
            new object[]{0, 0, 1, true},
            new object[]{0, 1, 1, false},
            new object[]{1, 0, 1, false},
            new object[]{1, 1, 0, false},
            new object[]{1, 1, 1, false},
            new object[]{2, 1, 7, false},
        };
    }
}
