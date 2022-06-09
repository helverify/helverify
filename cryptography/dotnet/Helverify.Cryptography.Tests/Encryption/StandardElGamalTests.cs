using Helverify.Cryptography.Encryption;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.Cryptography.Tests.Encryption
{
    internal class StandardElGamalTests
    {
        private IElGamal _elGamal;

        [OneTimeSetUp]
        public void Init()
        {
            _elGamal = new StandardElGamal();
        }

        [Test, TestCaseSource(nameof(_getMultiplicationParams))]
        public void TestHomomorphicMultiplication(List<int> messages, int expectedDecryption)
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

        private static object[] _getMultiplicationParams =
        {
            new object[]{new List<int>{3, 6, 2}, 36},
            new object[]{new List<int>{10, 2, 3}, 60},
            new object[]{new List<int>{25, 40, 80}, 80000},
        };
    }
}
