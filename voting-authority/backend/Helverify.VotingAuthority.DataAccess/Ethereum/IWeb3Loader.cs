using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Helverify.VotingAuthority.DataAccess.Ethereum;

/// <summary>
/// Manages the web3 instance and its connection to the geth node.
/// </summary>
public interface IWeb3Loader
{
    /// <summary>
    /// Blockchain Account
    /// </summary>
    public Account Account { get; }

    /// <summary>
    /// Password to the Blockchain Account
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Web3 instance (singleton)
    /// </summary>
    public IWeb3 Web3Instance { get; }
    
    /// <summary>
    /// Establishes a connection to the machine's geth IPC.
    /// </summary>
    void LoadInstance();
}