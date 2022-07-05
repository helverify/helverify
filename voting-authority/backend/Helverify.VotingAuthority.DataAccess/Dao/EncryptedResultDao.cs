using Helverify.VotingAuthority.DataAccess.Dto;

namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public struct EncryptedResultDao
    {
        public CipherTextDto? CipherText { get; set; } = null;
        public IList<ProofOfDecryptionDto> ProofsOfDecryption { get; set; }

        public EncryptedResultDao()
        {
            ProofsOfDecryption = new List<ProofOfDecryptionDto>();
        }
    }
}
