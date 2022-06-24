namespace Helverify.VotingAuthority.Domain.Model.Blockchain;

/// <summary>
/// Represents the genesis block of an Ethereum PoA (clique) blockchain.
/// </summary>
public sealed class Genesis
{
    /// <summary>
    /// Chain identifier for the blockchain network. Here, usually 13337.
    /// </summary>
    public int ChainId { get; }

    /// <summary>
    /// Contains the accounts of all authorities (sealers) of the blockchain.
    /// </summary>
    public IList<Account> Authorities { get; }

    /// <summary>
    /// Contains all accounts that need to be prefunded.
    /// </summary>
    public IList<Account> PrefundedAccounts { get; }
    
    /// <summary>
    /// Period parameter of the clique consensus algorithm. Defines the time between two subsequent blocks.
    /// </summary>
    public int CliquePeriod { get; }

    /// <summary>
    /// Epoch parameter of the clique consensus algorithm.
    /// </summary>
    public int CliqueEpoch { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="chainId">Chain identifier for the blockchain network</param>
    /// <param name="authorities">List of sealers</param>
    /// <param name="prefundedAccounts">Accounts that need to be prefunded</param>
    public Genesis(int chainId, IList<Account> authorities, IList<Account> prefundedAccounts)
    {
        ChainId = chainId;
        Authorities = authorities;
        PrefundedAccounts = prefundedAccounts;
        CliqueEpoch = 30000;
        CliquePeriod = 2;
    }
}