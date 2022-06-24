namespace Helverify.VotingAuthority.DataAccess.Dto
{
    public class GenesisDto
    {
        public ConfigDto Config { get; set; } = new();
        public string Difficulty { get; set; }
        public string GasLimit { get; set; }
        public string Extradata { get; set; }
        public IDictionary<string, AccountBalanceDto?> Alloc { get; set; } = new Dictionary<string, AccountBalanceDto?>();
    }
}
