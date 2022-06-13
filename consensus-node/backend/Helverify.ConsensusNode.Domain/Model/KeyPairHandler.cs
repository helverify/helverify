using System.IO.Abstractions;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;

namespace Helverify.ConsensusNode.Domain.Model
{
    /// <inheritdoc cref="IKeyPairHandler"/>
    internal class KeyPairHandler : IKeyPairHandler
    {
        internal const string PrivateKeyFileName = "private.pem";
        internal const string PublicKeyFileName = "public.pem";
        internal const string KeyPath = "/usr/local/keys";

        private readonly IFileSystem _fileSystem;
        private readonly IElGamal _elGamal;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileSystem">File system abstraction</param>
        public KeyPairHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _elGamal = new ExponentialElGamal();
        }

        /// <inheritdoc cref="IKeyPairHandler.CreateKeyPair"/>
        public AsymmetricCipherKeyPair CreateKeyPair(BigInteger p, BigInteger g)
        {
            return _elGamal.KeyGen(p, g);
        }

        /// <inheritdoc cref="IKeyPairHandler.GeneratePrivateKeyProof"/>
        public ProofOfPrivateKeyOwnership GeneratePrivateKeyProof(AsymmetricCipherKeyPair keyPair)
        {
            DHPublicKeyParameters publicKey = keyPair.Public as DHPublicKeyParameters;
            DHPrivateKeyParameters privateKey = keyPair.Private as DHPrivateKeyParameters;

            BigInteger pk = publicKey.Y;
            BigInteger sk = privateKey.X;

            return ProofOfPrivateKeyOwnership.Create(pk, sk, publicKey.Parameters.P, publicKey.Parameters.G);
        }

        /// <inheritdoc cref="IKeyPairHandler.LoadFromDisk"/>
        public AsymmetricCipherKeyPair LoadFromDisk()
        {
            AsymmetricKeyParameter publicKey = LoadFromDisk(PublicKeyFileName);
            AsymmetricKeyParameter privateKey = LoadFromDisk(PrivateKeyFileName);

            return new AsymmetricCipherKeyPair(publicKey, privateKey);
        }
        
        /// <inheritdoc cref="IKeyPairHandler.SaveToDisk"/>
        public void SaveToDisk(AsymmetricCipherKeyPair keyPair)
        {
            SaveToDisk(keyPair.Public, "public.pem");
            SaveToDisk(keyPair.Private, "private.pem");
        }

        private AsymmetricKeyParameter LoadFromDisk(string fileName)
        {
            using StreamReader streamReader = _fileSystem.File.OpenText(_fileSystem.Path.Combine(KeyPath, fileName));

            PemReader pemReader = new PemReader(streamReader);

            AsymmetricKeyParameter key = (AsymmetricKeyParameter)pemReader.ReadObject();

            return key;
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
