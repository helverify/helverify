namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public class VirtualBallotDao
    {
        public string Code { get; set; }
        public IList<SumProofDao> RowProofs { get; set; } = new List<SumProofDao>();
        public IList<SumProofDao> ColumnProofs { get; set; } = new List<SumProofDao>();

        public IList<EncryptedOptionDao> EncryptedOptions { get; set; } = new List<EncryptedOptionDao>();
    }
}
