using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.ConsensusNode.Domain.Tests.Model
{
    internal class KeyPairHandlerTests
    {
        private IFileSystem _fileSystem;
        private readonly BigInteger _p = new ("87A8E61DB4B6663CFFBBD19C651959998CEEF608660DD0F25D2CEED4435E3B00E00DF8F1D61957D4FAF7DF4561B2AA3016C3D91134096FAA3BF4296D830E9A7C209E0C6497517ABD5A8A9D306BCF67ED91F9E6725B4758C022E0B1EF4275BF7B6C5BFC11D45F9088B941F54EB1E59BB8BC39A0BF12307F5C4FDB70C581B23F76B63ACAE1CAA6B7902D52526735488A0EF13C6D9A51BFA4AB3AD8347796524D8EF6A167B5A41825D967E144E5140564251CCACB83E6B486F6B3CA3F7971506026C0B857F689962856DED4010ABD0BE621C3A3960A54E710C375F26375D7014103A4B54330C198AF126116D2276E11715F693877FAD7EF09CADB094AE91E1A1597",
            16);
        private readonly BigInteger _g = new ("3FB32C9B73134D0B2E77506660EDBD484CA7B18F21EF205407F4793A1A0BA12510DBC15077BE463FFF4FED4AAC0BB555BE3A6C1B0C6B47B1BC3773BF7E8C6F62901228F8C28CBB18A55AE31341000A650196F931C77A57F2DDF463E5E9EC144B777DE62AAAB8A8628AC376D282D6ED3864E67982428EBC831D14348F6F2F9193B5045AF2767164E1DFC967C1FB3F2E55A4BD1BFFE83B9C80D052B985D182EA0ADB2A3B7313D3FE14C8484B1E052588B9B7D2BBD2DF016199ECD06E1557CD0915B3353BBB64E0EC377FD028370DF92B52C7891428CDC67EB6184B523D1DB246C32F63078490F00EF8D647D148D47954515E2327CFEF98C582664B4C0F6CC41659", 16);
        private IKeyPairHandler _keyPairHandler;

        [SetUp]
        public void SetUp()
        {
            _fileSystem = new MockFileSystem();
            _keyPairHandler = new KeyPairHandler(_fileSystem);
        }

        [Test]
        public void TestCreateKeyPair()
        {
            // act
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.CreateKeyPair(_p, _g);

            // assert
            Assert.That(keyPair.Private, Is.Not.Null);
            Assert.That(keyPair.Public, Is.Not.Null);
        }

        [Test]
        public void TestGenerateKeyPairProof()
        {
            // arrange
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.CreateKeyPair(_p, _g);
            
            // act
            ProofOfPrivateKeyOwnership proof = _keyPairHandler.GeneratePrivateKeyProof(keyPair);

            // assert
            BigInteger h = (keyPair.Public as DHPublicKeyParameters).Y;
            
            Assert.That(proof.Verify(h, _p, _g), Is.True);
        }

        [Test]
        public void TestGenerateKeyPairProofInvalid()
        {
            // arrange
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.CreateKeyPair(_p, _g);

            // act
            ProofOfPrivateKeyOwnership proof = _keyPairHandler.GeneratePrivateKeyProof(keyPair);

            // assert
            AsymmetricCipherKeyPair anotherKeyPair = _keyPairHandler.CreateKeyPair(_p, _g);
            BigInteger h = (anotherKeyPair.Public as DHPublicKeyParameters).Y;

            Assert.That(proof.Verify(h, _p, _g), Is.False);
        }

        [Test]
        public void TestSaveToDisk()
        {
            // arrange
            AsymmetricCipherKeyPair keyPair = _keyPairHandler.CreateKeyPair(_p, _g);

            // act
            _keyPairHandler.SaveToDisk(keyPair);

            // assert
            Assert.That(_fileSystem.File.Exists(_fileSystem.Path.Combine(KeyPairHandler.KeyPath, KeyPairHandler.PrivateKeyFileName)), Is.True);
            Assert.That(_fileSystem.File.Exists(_fileSystem.Path.Combine(KeyPairHandler.KeyPath, KeyPairHandler.PublicKeyFileName)), Is.True);
        }

        [Test]
        public void TestLoadFromDisk()
        {
            // arrange
            AsymmetricCipherKeyPair originalKeyPair = _keyPairHandler.CreateKeyPair(_p, _g);
            
            BigInteger originalPublicKey = (originalKeyPair.Public as DHPublicKeyParameters).Y;
            BigInteger originalPrivateKey = (originalKeyPair.Private as DHPrivateKeyParameters).X;
            
            _keyPairHandler.SaveToDisk(originalKeyPair);
            
            // act
            AsymmetricCipherKeyPair restoredKeyPair = _keyPairHandler.LoadFromDisk();

            // assert
            BigInteger restoredPublicKey = (restoredKeyPair.Public as DHPublicKeyParameters).Y;
            BigInteger restoredPrivateKey = (restoredKeyPair.Private as DHPrivateKeyParameters).X;

            Assert.That(restoredPrivateKey, Is.EqualTo(originalPrivateKey));
            Assert.That(restoredPublicKey, Is.EqualTo(originalPublicKey));
        }
    }
}
