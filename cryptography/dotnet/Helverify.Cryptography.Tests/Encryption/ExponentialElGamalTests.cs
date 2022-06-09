using Helverify.Cryptography.Common;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.Encryption.Strategy;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Tests.Encryption
{
    internal class ExponentialElGamalTests
    {
        private IElGamal _elGamal;

        [OneTimeSetUp]
        public void Init()
        {
            _elGamal = new ExponentialElGamal(new LinearDecryption());
        }

        [Test, TestCaseSource(nameof(_getAdditionParams))]
        public void TestHomomorphicAddition(List<int> messages, int expectedDecryption)
        {
            // arrange
            AsymmetricKeyParameter privateKey = _elGamal.GetKey("dhkey.pem");
            AsymmetricKeyParameter publicKey = _elGamal.GetKey("dhpub.pem");

            AsymmetricCipherKeyPair keyPair = new AsymmetricCipherKeyPair(publicKey, privateKey);

            List<ElGamalCipher> ciphers = messages.Select(m => _elGamal.Encrypt(m, keyPair.Public)).ToList();
            ElGamalCipher combined = null;

            // act

            for (int i = 0; i < messages.Count; i++)
            {
                ElGamalCipher cipher = ciphers[i];

                if (combined != null)
                {
                    combined = combined.Add(cipher, (publicKey as DHPublicKeyParameters)?.Parameters.P);
                }
                else
                {
                    combined = cipher;
                }
            }

            int actualDecryption = _elGamal.Decrypt(combined, keyPair.Private);

            // assert
            Assert.That(actualDecryption, Is.EqualTo(expectedDecryption));
        }

        [Test]
        public void TestCooperativeDecryption()
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

            int message1 = 1;
            int message2 = 0;
            int message3 = 1;

            ElGamalCipher cipher1 = elGamal.Encrypt(message1, combinedPublicKey);
            ElGamalCipher cipher2 = elGamal.Encrypt(message2, combinedPublicKey);
            ElGamalCipher cipher3 = elGamal.Encrypt(message3, combinedPublicKey);
            ElGamalCipher combinedCipher = cipher1.Add(cipher2, p).Add(cipher3, p);

            // act
            BigInteger share1 = elGamal.DecryptShare(combinedCipher, keyPair1.Private as DHPrivateKeyParameters, p);
            BigInteger share2 = elGamal.DecryptShare(combinedCipher, keyPair2.Private as DHPrivateKeyParameters, p);
            BigInteger share3 = elGamal.DecryptShare(combinedCipher, keyPair3.Private as DHPrivateKeyParameters, p);
            int message =
                elGamal.CombineShares(new List<BigInteger> { share1, share2, share3 }, combinedCipher.D, p, g);

            // assert
            Assert.That(message, Is.EqualTo(2));
        }
        
        private static object[] _getAdditionParams =
        {
            new object[]{new List<int>{7, 12, 36}, 55},
            new object[]{new List<int>{130, 42, 203}, 375},
            new object[]{new List<int>{1200, 321, 15}, 1536},
        };
    }
}
