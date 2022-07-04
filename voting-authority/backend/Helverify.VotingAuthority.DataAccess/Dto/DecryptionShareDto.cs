namespace Helverify.VotingAuthority.DataAccess.Dto
{
    public class DecryptionShareDto
    {
        public string DecryptedShare { get; set; }

        /// <summary>
        /// Public key share of the consensus node
        /// </summary>
        public string PublicKey { get; set; }

        public ProofOfDecryptionDto ProofOfDecryption { get; set; }
    }
}
