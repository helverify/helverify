using Helverify.VotingAuthority.DataAccess.Dto;

namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public struct DecryptedResultsDao
    {
        public IList<DecryptedValueDao> DecryptedResults { get; set; }

        public DecryptedResultsDao()
        {
            DecryptedResults = new List<DecryptedValueDao>();
        }
    }

    public struct DecryptedValueDao
    {
        public CipherTextDto CipherText { get; set; }
        public int PlainText { get; set; }
        public IList<DecryptedShareDao> Shares { get; set; }
    }

    public struct DecryptedShareDao
    {
        public string Share { get; set; }
        public ProofOfDecryptionDto ProofOfDecryption { get; set; }
        public string PublicKeyShare { get; set; }
    }
}
