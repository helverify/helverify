using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.ConsensusNode.Domain.Model
{
    /// <summary>
    /// ElGamal CipherText
    /// </summary>
    public class CipherText
    {
        private readonly IElGamal _elGamal;
        private readonly ElGamalCipher _cipher;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cipher">Original ElGamal ciphertext</param>
        public CipherText(ElGamalCipher cipher)
        {
            _elGamal = new ExponentialElGamal();
            _cipher = cipher;
        }

        /// <summary>
        /// Decrypts this consensus node's share of the ciphertext.
        /// </summary>
        /// <param name="keyPair">ElGamal key pair</param>
        /// <returns>Decrypted share</returns>
        /// <exception cref="Exception"></exception>
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
