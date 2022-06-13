namespace Helverify.ConsensusNode.Backend.Dto
{
    /// <summary>
    /// Represents a Chaum-Pedersen proof which proves the correct decryption of a message.
    /// </summary>
    public class ProofOfDecryptionDto
    {
        /// <summary>
        /// d (Chaum-Pedersen)
        /// </summary>
        public string D { get; set; }

        /// <summary>
        /// u (Chaum-Pedersen)
        /// </summary>
        public string U { get; set; }

        /// <summary>
        /// v (Chaum-Pedersen)
        /// </summary>
        public string V { get; set; }

        /// <summary>
        /// s (Chaum-Pedersen)
        /// </summary>
        public string S { get; set; }
    }
}
