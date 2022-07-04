namespace Helverify.ConsensusNode.DataAccess.Dao;

public struct EncryptedOptionDao
{
    public EncryptedOptionDao()
    {
        ShortCode = null!;
        Values = new List<EncryptedOptionValueDao>();
    }

    public string ShortCode { get; set; }
    public IList<EncryptedOptionValueDao> Values { get; set; }
}