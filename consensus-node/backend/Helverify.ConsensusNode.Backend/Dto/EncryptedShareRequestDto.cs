namespace Helverify.ConsensusNode.Backend.Dto
{
    /// <summary>
    /// Represents an ElGamal ciphertext for cooperative decryption.
    /// </summary>
    public class EncryptedShareRequestDto
    {
        /// <summary>
        /// ElGamal ciphertext
        /// </summary>
        public CipherTextDto Cipher { get; set; }

        /// <summary>
        /// Election identifier
        /// </summary>
        public string ElectionId { get; set; }
    }
}
