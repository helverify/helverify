using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.Encryption.Strategy;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

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

        private static object[] _getAdditionParams =
        {
            new object[]{new List<int>{7, 12, 36}, 55},
            new object[]{new List<int>{130, 42, 203}, 375},
            new object[]{new List<int>{1200, 321, 15}, 1536},
        };
    }
}
