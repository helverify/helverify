﻿using System.IO.Abstractions;
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
        public Web3 Web3Instance { get; private set; }

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

            Web3 instance = new Web3(Account, new UnixIpcClient(IpcPath));
            
            instance.Eth.TransactionManager.UseLegacyAsDefault = true;
            
            Web3Instance = instance;
        }
    }
}
