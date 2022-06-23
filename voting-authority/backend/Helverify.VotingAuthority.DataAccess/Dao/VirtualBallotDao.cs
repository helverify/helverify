namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public struct VirtualBallotDao
    {
        public VirtualBallotDao()
        {
            Code = null;
        }

        public string Code { get; set; }
        public IList<SumProofDao> RowProofs { get; set; } = new List<SumProofDao>();
        public IList<SumProofDao> ColumnProofs { get; set; } = new List<SumProofDao>();

        public IList<EncryptedOptionDao> EncryptedOptions { get; set; } = new List<EncryptedOptionDao>();
    }
}
