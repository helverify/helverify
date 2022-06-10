using System.IO.Abstractions;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;

namespace Helverify.ConsensusNode.Domain.Model
{
    internal class KeyPairHandler : IKeyPairHandler
    {
        private const string PrivateKeyFileName = "private.pem";
        private const string PublicKeyFileName = "public.pem";
        private const string KeyPath = "/usr/local/keys";

        private readonly IFileSystem _fileSystem;
        private readonly IElGamal _elGamal;

        public KeyPairHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _elGamal = new ExponentialElGamal();
        }
        
        public AsymmetricCipherKeyPair CreateKeyPair(BigInteger p, BigInteger g)
        {
            return _elGamal.KeyGen(p, g);
        }

        public ProofOfPrivateKeyOwnership GeneratePrivateKeyProof(AsymmetricCipherKeyPair keyPair)
        {
            DHPublicKeyParameters publicKey = keyPair.Public as DHPublicKeyParameters;
            DHPrivateKeyParameters privateKey = keyPair.Private as DHPrivateKeyParameters;

            BigInteger pk = publicKey.Y;
            BigInteger sk = privateKey.X;

            return ProofOfPrivateKeyOwnership.Create(pk, sk, publicKey.Parameters.P, publicKey.Parameters.G);
        }

        public AsymmetricCipherKeyPair LoadFromDisk()
        {
            AsymmetricKeyParameter publicKey = LoadFromDisk(PublicKeyFileName);
            AsymmetricKeyParameter privateKey = LoadFromDisk(PrivateKeyFileName);

            return new AsymmetricCipherKeyPair(publicKey, privateKey);
        }

        private AsymmetricKeyParameter LoadFromDisk(string fileName)
        {
            using StreamReader streamReader = _fileSystem.File.OpenText(_fileSystem.Path.Combine(KeyPath, fileName));
            
            PemReader pemReader = new PemReader(streamReader);
            
            AsymmetricKeyParameter key = (AsymmetricKeyParameter)pemReader.ReadObject();

            return key;
        }

        public void SaveToDisk(AsymmetricCipherKeyPair keyPair)
        {
            SaveToDisk(keyPair.Public, "public.pem");
            SaveToDisk(keyPair.Private, "private.pem");
        }

        private void SaveToDisk(AsymmetricKeyParameter key, string fileName)
        {
            if (!_fileSystem.Directory.Exists(KeyPath))
            {
                _fileSystem.Directory.CreateDirectory(KeyPath);
            }

            using TextWriter writer = _fileSystem.File.CreateText(_fileSystem.Path.Combine(KeyPath, fileName));

            PemWriter pemWriter = new PemWriter(writer);

            pemWriter.WriteObject(key);

            pemWriter.Writer.Flush();
        }
        
    }
}
