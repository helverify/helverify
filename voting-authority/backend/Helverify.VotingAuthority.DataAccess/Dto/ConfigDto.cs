namespace Helverify.VotingAuthority.DataAccess.Dto;

public class ConfigDto
{
    public int ChainId { get; set; }
    public int HomesteadBlock { get; set; }
    public int Eip150Block { get; set; }
    public int Eip155Block { get; set; }
    public int Eip158Block { get; set; }
    public int ByzantiumBlock { get; set; }
    public int ConstantinopleBlock { get; set; }
    public int PetersburgBlock { get; set; }
    public CliqueDto Clique { get; set; } = new();
}