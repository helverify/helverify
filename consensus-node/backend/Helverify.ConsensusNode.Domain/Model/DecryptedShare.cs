using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;

namespace Helverify.ConsensusNode.Domain.Model
{
    /// <summary>
    /// Decrypted share with the corresponding proof of correct decryption
    /// </summary>
    public class DecryptedShare
    {
        /// <summary>
        /// Decrypted share for cooperative decryption
        /// </summary>
        public string Share { get; set; }

        public ElGamalCipher Cipher { get; set; }

        /// <summary>
        /// Chaum-Pedersen Proof of correct decryption
        /// </summary>
        public ProofOfDecryption ProofOfDecryption { get; set; }
    }
}
