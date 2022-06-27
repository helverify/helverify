namespace Helverify.ConsensusNode.Backend.Dto
{
    /// <summary>
    /// Represents the public parameters of an ElGamal cryptosystem for key generation.
    /// </summary>
    public class KeyPairRequestDto
    {
        /// <summary>
        /// Public prime p of an ElGamal cryptosystem
        /// </summary>
        public string P { get; set; }

        /// <summary>
        /// Generator g of an ElGamal cryptosystem
        /// </summary>
        public string G { get; set; }

        /// <summary>
        /// Election identifier
        /// </summary>
        public string ElectionId { get; set; }
    }
}
