using System.ComponentModel;
using System.IO.Abstractions;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Helverify.VotingAuthority.DataAccess.Ethereum
{
    /// <inheritdoc cref="IWeb3Loader"/>
    public class Web3Loader : IWeb3Loader
    {
        private const string PrivateKeyPath = "/home/eth/private.key";
        private const string IpcPath = "/home/eth/data/geth.ipc";

        private readonly IFileSystem _fileSystem;

        /// <inheritdoc cref="IWeb3Loader.Account"/>
        public Account Account { get; private set; }

        /// <inheritdoc cref="IWeb3Loader.Password"/>
        public string Password { get; }

        /// <inheritdoc cref="IWeb3Loader.Web3Instance"/>
        public IWeb3 Web3Instance { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileSystem">File system abstraction</param>
        /// <param name="password">Password to the Blockchain account</param>
        public Web3Loader(IFileSystem fileSystem, string password)
        {
            Password = password;
            _fileSystem = fileSystem;
        }

        /// <inheritdoc cref="IWeb3Loader.LoadInstance"/>
        public void LoadInstance()
        {
            if (!_fileSystem.File.Exists(PrivateKeyPath))
            {
                return;
            }

            string keyFilePath = _fileSystem.File.ReadAllText(PrivateKeyPath);

            Account = Account.LoadFromKeyStore(keyFilePath, Password, 13337);

            IWeb3 instance = new Web3(Account, new UnixIpcClient(IpcPath));
            
            instance.Eth.TransactionManager.UseLegacyAsDefault = true;
            instance.Eth.TransactionManager.DefaultGas = 49000000;
            instance.Eth.TransactionManager.DefaultGasPrice = 1;

            Web3Instance = instance;

            Task.Run(() => Web3Instance.Personal.UnlockAccount.SendRequestAsync(Account.Address, Password, 0)).Wait();
        }
    }
}
