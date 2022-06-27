namespace Helverify.VotingAuthority.DataAccess.Dto
{
    public class EncryptedShareRequestDto
    {
        public CipherTextDto Cipher { get; set; }
        public string ElectionId { get; set; }
    }
}
