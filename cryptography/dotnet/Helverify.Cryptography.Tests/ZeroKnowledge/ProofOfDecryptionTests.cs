using Helverify.Cryptography.Common;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Tests.ZeroKnowledge
{
    internal class ProofOfDecryptionTests
    {
        [Test]
        public void TestCreate()
        {
            // arrange
            IElGamal elGamal = new StandardElGamal();

            DhGroup dhGroup = DhGroups.Get(DhGroups.Modp2048With256PrimeOrderSubgroup);
            
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

            int message = 1;

            ElGamalCipher cipher = elGamal.Encrypt(message, combinedPublicKey);

            // act
            ProofOfDecryption proofOfDecryption = ProofOfDecryption.Create(cipher.C, cipher.D,
                keyPair1.Public as DHPublicKeyParameters, keyPair1.Private as DHPrivateKeyParameters);


            // assert
            BigInteger share = elGamal.DecryptShare(cipher, keyPair1.Private as DHPrivateKeyParameters, p);

            Assert.That(proofOfDecryption.D, Is.EqualTo(share));
        }

        [Test]
        public void TestVerify()
        {
            // arrange
            IElGamal elGamal = new StandardElGamal();

            DhGroup dhGroup = DhGroups.Get(DhGroups.Modp2048With256PrimeOrderSubgroup);
            
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

            int message = 1;

            ElGamalCipher cipher = elGamal.Encrypt(message, combinedPublicKey);

            ProofOfDecryption proofOfDecryption = ProofOfDecryption.Create(cipher.C, cipher.D,
                keyPair1.Public as DHPublicKeyParameters, keyPair1.Private as DHPrivateKeyParameters);

            // act
            bool proof1 = proofOfDecryption.Verify(cipher.C, cipher.D, keyPair1.Public as DHPublicKeyParameters);
            bool proof2 = proofOfDecryption.Verify(cipher.C, cipher.D, keyPair2.Public as DHPublicKeyParameters);
            bool proof3 = proofOfDecryption.Verify(cipher.C, cipher.D, keyPair3.Public as DHPublicKeyParameters);

            // assert
            Assert.IsTrue(proof1);
            Assert.IsFalse(proof2);
            Assert.IsFalse(proof3);
        }
    }
}
