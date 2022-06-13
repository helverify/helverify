namespace Helverify.ConsensusNode.Backend.Dto
{
    /// <summary>
    /// Represents a public key with the corresponding proof of private key ownership.
    /// </summary>
    public class PublicKeyDto
    {
        /// <summary>
        /// Public key
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// Schnorr-Proof of private key ownership
        /// </summary>
        public ProofOfPrivateKeyDto ProofOfPrivateKey { get; set; }
    }
}
