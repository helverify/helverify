using System.IO.Abstractions;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Helverify.VotingAuthority.DataAccess.Ethereum
{
    public class Web3Loader : IWeb3Loader
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _connectionString;

        public Account Account { get; private set; }

        public string Password { get; }

        public Web3 Web3Instance { get; private set; }

        public Web3Loader(IFileSystem fileSystem, string connectionString, string password)
        {
            Password = password;
            _fileSystem = fileSystem;
            _connectionString = connectionString;
        }

        public void LoadInstance()
        {
            string keyFilePath = _fileSystem.File.ReadAllText("/home/eth/private.key");

            Account = Account.LoadFromKeyStore(keyFilePath, Password, 13337);

            Web3 instance = new Web3(Account, _connectionString);
            
            instance.Eth.TransactionManager.UseLegacyAsDefault = true;
            
            Web3Instance = instance;
        }
    }
}
