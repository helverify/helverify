namespace Helverify.ConsensusNode.DataAccess.Dao
{
    public struct VirtualBallotDao
    {
        public VirtualBallotDao()
        {
            EncryptedOptions = new List<EncryptedOptionDao>();
        }

        public IList<EncryptedOptionDao> EncryptedOptions { get; set; }
    }
}
