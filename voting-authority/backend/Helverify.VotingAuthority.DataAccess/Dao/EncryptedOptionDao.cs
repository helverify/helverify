namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public struct EncryptedOptionDao
    {
        public EncryptedOptionDao()
        {
            ShortCode = null;
        }

        public string ShortCode { get; set; }
        public IList<EncryptedOptionValueDao> Values { get; set; } = new List<EncryptedOptionValueDao>();
    }
}
