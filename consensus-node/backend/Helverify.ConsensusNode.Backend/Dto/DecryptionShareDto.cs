namespace Helverify.ConsensusNode.Backend.Dto
{
    /// <summary>
    /// Represents a decrypted share of an ElGamal ciphertext
    /// </summary>
    public class DecryptionShareDto
    {
        /// <summary>
        /// Content of the decrypted share
        /// </summary>
        public string DecryptedShare { get; set; }

        /// <summary>
        /// Chaum-Pedersen proof that the encryption has been done correctly.
        /// </summary>
        public ProofOfDecryptionDto ProofOfDecryption { get; set; }
    }
}
