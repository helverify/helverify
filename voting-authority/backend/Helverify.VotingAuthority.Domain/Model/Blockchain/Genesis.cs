namespace Helverify.VotingAuthority.Domain.Model.Blockchain;

public class Genesis
{
    public int ChainId { get; }
    public IList<Account> Authorities { get; }
    public IList<Account> PrefundedAccounts { get; }
    public int CliquePeriod { get; }
    public int CliqueEpoch { get; }

    public Genesis(int chainId, IList<Account> authorities, IList<Account> prefundedAccounts)
    {
        ChainId = chainId;
        Authorities = authorities;
        PrefundedAccounts = prefundedAccounts;
        CliqueEpoch = 30000;
        CliquePeriod = 0;
    }
}