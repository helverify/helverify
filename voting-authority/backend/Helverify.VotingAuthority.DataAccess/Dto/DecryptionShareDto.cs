namespace Helverify.VotingAuthority.DataAccess.Dto
{
    public class DecryptionShareDto
    {
        public string DecryptedShare { get; set; }

        public ProofOfDecryptionDto ProofOfDecryption { get; set; }
    }
}
