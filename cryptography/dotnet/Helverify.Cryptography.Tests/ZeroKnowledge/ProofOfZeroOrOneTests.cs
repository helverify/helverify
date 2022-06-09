using Helverify.Cryptography.Common;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Tests.ZeroKnowledge
{
    internal class ProofOfZeroOrOneTests
    {
        [Test]
        public void TestVerify()
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

            int message1 = 0;
            int message2 = 1;
            int message3 = 2;


            ElGamalCipher cipher1 = elGamal.Encrypt(message1, combinedPublicKey);
            ElGamalCipher cipher2 = elGamal.Encrypt(message2, combinedPublicKey);
            ElGamalCipher cipher3 = elGamal.Encrypt(message3, combinedPublicKey);

            ProofOfZeroOrOne proof1 = ProofOfZeroOrOne.Create(message1, cipher1.C, cipher1.D, combinedPublicKey.Y, cipher1.R, p, g);
            ProofOfZeroOrOne proof2 = ProofOfZeroOrOne.Create(message2, cipher2.C, cipher2.D, combinedPublicKey.Y, cipher2.R, p, g);

            // act
            bool isValid1 = proof1.Verify(cipher1.C, cipher1.D, combinedPublicKey.Y, p, g);
            bool isValid2 = proof2.Verify(cipher2.C, cipher2.D, combinedPublicKey.Y, p, g);
            bool isValid3 = proof1.Verify(cipher3.C, cipher3.D, combinedPublicKey.Y, p, g);
            bool isValid4 = proof1.Verify(cipher2.C, cipher2.D, combinedPublicKey.Y, p, g);
            bool isValid5 = proof2.Verify(cipher1.C, cipher1.D, combinedPublicKey.Y, p, g);

            // assert
            Assert.True(isValid1);
            Assert.True(isValid2);
            Assert.False(isValid3);
            Assert.False(isValid4);
            Assert.False(isValid5);
        }

        [Test]
        public void TestCreateInvalidMessage()
        {
            // arrange
            BigInteger x = BigInteger.One;

            // arrange, act, assert
            Assert.Throws<Exception>(() => ProofOfZeroOrOne.Create(42, x, x, x, x, x, x));
        }
    }
}
