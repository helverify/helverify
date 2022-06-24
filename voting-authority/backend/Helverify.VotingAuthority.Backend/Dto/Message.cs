namespace Helverify.VotingAuthority.Backend.Dto
{
    /// <summary>
    /// Represents a plaintext message
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Numeric message
        /// </summary>
        public int M { get; set; }
    }

    /// <summary>
    /// Represents the ciphertext of an ElGamal cryptosystem
    /// </summary>
    public class Cipher
    {
        /// <summary>
        /// First parameter of an ElGamal ciphertext
        /// </summary>
        public string C { get; set; } = string.Empty;
        
        /// <summary>
        /// Second parameter of an ElGamal ciphertext
        /// </summary>
        public string D { get; set; } = string.Empty;
    }
}
