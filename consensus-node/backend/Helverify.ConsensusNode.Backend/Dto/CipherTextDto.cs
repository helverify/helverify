namespace Helverify.ConsensusNode.Backend.Dto
{
    /// <summary>
    /// Represents an ElGamal ciphertext
    /// </summary>
    public class CipherTextDto
    {
        /// <summary>
        /// First component of an ElGamal ciphertext (c)
        /// </summary>
        public string C { get; set; }

        /// <summary>
        /// Second component of an ElGamal ciphertext (d)
        /// </summary>
        public string D { get; set; }
    }
}
