namespace Helverify.VotingAuthority.DataAccess.Dao
{
    public class EncryptedOptionDao
    {
        public string ShortCode { get; set; }
        public IList<EncryptedOptionValueDao> Values { get; set; } = new List<EncryptedOptionValueDao>();
    }
}
