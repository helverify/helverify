using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.ConsensusNode.Domain.Model
{
    public class EncryptedShare
    {
        private readonly IElGamal _elGamal;
        private readonly ElGamalCipher _cipher;

        public EncryptedShare(ElGamalCipher cipher)
        {
            _elGamal = new ExponentialElGamal();
            _cipher = cipher;
        }

        public DecryptedShare Decrypt(AsymmetricCipherKeyPair keyPair)
        {
            DHPublicKeyParameters publicKey = keyPair.Public as DHPublicKeyParameters ?? throw new Exception("No public key found.");
            DHPrivateKeyParameters privateKey = keyPair.Private as DHPrivateKeyParameters ?? throw new Exception("No private key found.");

            string decrypted = _elGamal.DecryptShare(_cipher, privateKey, privateKey.Parameters.P).ToString(16);
            
            return new DecryptedShare
            {
                Share = decrypted,
                ProofOfDecryption = ProofOfDecryption.Create(_cipher.C, _cipher.D, publicKey, privateKey)
            };
        }
    }
}
