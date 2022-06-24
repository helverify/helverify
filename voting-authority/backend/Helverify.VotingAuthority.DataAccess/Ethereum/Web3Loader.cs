using System.IO.Abstractions;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Helverify.VotingAuthority.DataAccess.Ethereum
{
    public class Web3Loader : IWeb3Loader
    {
        private const string PrivateKeyPath = "/home/eth/private.key";
        private const string IpcPath = "/home/eth/data/geth.ipc";

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
            if (!_fileSystem.File.Exists(PrivateKeyPath))
            {
                return;
            }

            string keyFilePath = _fileSystem.File.ReadAllText(PrivateKeyPath);

            Account = Account.LoadFromKeyStore(keyFilePath, Password, 13337);

            Web3 instance = new Web3(Account, new UnixIpcClient(IpcPath));
            
            instance.Eth.TransactionManager.UseLegacyAsDefault = true;
            
            Web3Instance = instance;
        }
    }
}
