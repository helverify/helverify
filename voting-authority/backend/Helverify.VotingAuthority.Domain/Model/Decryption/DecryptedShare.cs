using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model.Decryption
{
    /// <summary>
    /// Represents a single decrypted share of a ciphertext.
    /// </summary>
    public struct DecryptedShare
    {
        /// <summary>
        /// Decrypted share value
        /// </summary>
        public BigInteger Share { get; set; }

        /// <summary>
        /// Proof of correct decryption
        /// </summary>
        public ProofOfDecryption ProofOfDecryption { get; set; }

        /// <summary>
        /// Public key share of the decrypting consensus node
        /// </summary>
        public BigInteger PublicKeyShare { get; set; }
    }
}
