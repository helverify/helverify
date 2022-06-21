using System.IO.Abstractions;
using System.Text.RegularExpressions;
using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Helverify.VotingAuthority.Domain.Service
{
    /// <inheritdoc cref="IBlockchainSetup"/>
    public class BlockchainSetup : IBlockchainSetup
    {
        private const string InitialFunds = "1000000000000000000000000000000000000";
        private const string ScriptDir = "/app/scripts/";
        private const string HomeDir = "/home/eth/";

        private const string StartRpcScript = "start-rpc.sh";
        private const string InitScript = "init.sh";
        private const string InitGenesisScript = "init-genesis.sh";

        private const string GenesisFile = "genesis.json";
        private const string NodesFile = "nodes.json";
        private const string AddressFile = "address";
        
        private const string NewLine = @"\n";

        private readonly IConsensusNodeService _consensusNodeService;
        private readonly ICliRunner _cliRunner;
        private readonly IMapper _mapper;
        private readonly IFileSystem _fileSystem;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consensusNodeService">Service facade for access to consensus node operations</param>
        /// <param name="cliRunner">Service to run commands</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="fileSystem">File system abstraction</param>
        public BlockchainSetup(IConsensusNodeService consensusNodeService, ICliRunner cliRunner, IMapper mapper, IFileSystem fileSystem)
        {
            _consensusNodeService = consensusNodeService;
            _cliRunner = cliRunner;
            _mapper = mapper;
            _fileSystem = fileSystem;
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        /// <inheritdoc cref="IBlockchainSetup.StartSealingAsync"/>
        public async Task StartSealingAsync(IList<Registration> registrations)
        {
            foreach (Registration registration in registrations)
            {
                await _consensusNodeService.StartSealing(registration.Endpoint);
            }
        }

        /// <inheritdoc cref="IBlockchainSetup.InitializeNodesAsync"/>
        public async Task InitializeNodesAsync(IList<Registration> registrations, NodesDto nodes)
        {
            foreach (Registration registration in registrations)
            {
                await _consensusNodeService.InitializeNodes(registration.Endpoint, nodes);
            }
        }

        /// <inheritdoc cref="IBlockchainSetup.StartPeersAsync"/>
        public async Task<NodesDto> StartPeersAsync(IList<Registration> registrations)
        {
            foreach (Registration registration in registrations)
            {
                registration.Enode = await _consensusNodeService.StartPeers(registration.Endpoint);
            }

            NodesDto nodes = new NodesDto
            {
                Nodes = registrations.Select(r => r.Enode).ToList()
            };

            return nodes;
        }

        /// <inheritdoc cref="IBlockchainSetup.PropagateGenesisBlockAsync"/>
        public async Task<Genesis> PropagateGenesisBlockAsync(IList<Registration> registrations, Account rpcAccount)
        {
            IList<Account> authorities = registrations.Select(r => r.Account).ToList();

            IList<Account> prefundedAccounts = new List<Account>(authorities);
            
            prefundedAccounts.Add(rpcAccount);

            Genesis genesis = new Genesis(13337, authorities, prefundedAccounts);

            foreach (Registration registration in registrations)
            {
                await _consensusNodeService.InitializeGenesisBlock(registration.Endpoint, genesis);
            }

            return genesis;
        }

        /// <inheritdoc cref="IBlockchainSetup.CreateAccountsAsync"/>
        public async Task<string> CreateAccountsAsync(IList<Registration> registrations)
        {
            // init remote nodes
            foreach (Registration registration in registrations)
            {
                string bcAddress = await _consensusNodeService.CreateBcAccount(registration.Endpoint);

                registration.Account = new Account(bcAddress, InitialFunds);
            }

            // init this node
            _cliRunner.Execute($"{ScriptDir}{InitScript}", string.Empty);

            string accountAddress = _fileSystem.File.ReadAllText($"{HomeDir}{AddressFile}");

            accountAddress = Regex.Replace(accountAddress, NewLine, string.Empty);

            return accountAddress;
        }

        /// <inheritdoc cref="IBlockchainSetup.RegisterRpcEndpoint"/>
        public void RegisterRpcEndpoint(Genesis genesis, NodesDto nodes)
        {
            GenesisDto genesisDto = _mapper.Map<GenesisDto>(genesis);

            string genesisJson = JsonConvert.SerializeObject(genesisDto, _jsonSerializerSettings);

            _fileSystem.File.WriteAllText($"{HomeDir}{GenesisFile}", genesisJson);

            _cliRunner.Execute($"{ScriptDir}{InitGenesisScript}", string.Empty);

            string nodesJson = JsonConvert.SerializeObject(nodes, _jsonSerializerSettings);

            _fileSystem.File.WriteAllText($"{HomeDir}{NodesFile}", nodesJson);

            _cliRunner.Execute($"{ScriptDir}{StartRpcScript}", string.Empty);
        }
    }
}
