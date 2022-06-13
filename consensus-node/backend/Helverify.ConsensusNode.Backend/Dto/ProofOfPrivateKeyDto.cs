namespace Helverify.ConsensusNode.Backend.Dto
{
    /// <summary>
    /// Represents the Schnorr-Proof of owning a private key to a given public key.
    /// </summary>
    public class ProofOfPrivateKeyDto
    {
        /// <summary>
        /// c (Schnorr-Proof)
        /// </summary>
        public string C { get; set; }

        /// <summary>
        /// d (Schnorr-Proof)
        /// </summary>
        public string D { get; set; }
    }
}
