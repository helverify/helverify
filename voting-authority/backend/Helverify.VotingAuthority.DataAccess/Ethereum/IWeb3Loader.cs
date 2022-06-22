using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Helverify.VotingAuthority.DataAccess.Ethereum;

public interface IWeb3Loader
{
    public Account Account { get; }

    public string Password { get; }

    public Web3 Web3Instance { get; }
    
    void LoadInstance();
}