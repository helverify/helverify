using System.Diagnostics;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.ConsensusNode.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Helverify.ConsensusNode.Backend.Controllers
{
    /// <summary>
    /// Controller for configuring a Proof-of-Authority Ethereum Blockchain
    /// </summary>
    [Route("api/blockchain")]
    [ApiController]
    public class BlockchainController: ControllerBase
    {
        private const string ScriptDir = "/app/scripts/";
        private const string HomeDir = "/home/eth/";

        private const string BinSh = "/bin/sh";
        private const string InitScript = "init.sh";
        private const string InitGenesisScript = "init-genesis.sh";
        private const string StartConsensusNodeScript = "start-consensusnode.sh";
        private const string StartMiningScript = "start-mining.sh";
        private const string EnodeScript = "enode.sh";

        private const string AddressFile = "address";
        private const string EnodeFile = "enode";
        private const string GenesisFile = "genesis.json";
        private const string NodesFile = "nodes.json";

        private const string DoubleQuotes = "\"";
        private const string NewLine = @"\n";

        private readonly ICliRunner _cliRunner;
        private readonly IFileSystem _fileSystem;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cliRunner">Service for running commands on the command line</param>
        /// <param name="fileSystem">Service for accessing the file system</param>
        public BlockchainController(ICliRunner cliRunner, IFileSystem fileSystem)
        {
            _cliRunner = cliRunner;
            _fileSystem = fileSystem;
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        /// <summary>
        /// Creates a new Ethereum account and returns its address
        /// </summary>
        /// <returns>Ethereum address of this consensus node</returns>
        [HttpPost]
        [Route("account")]
        public ActionResult PostAccount()
        {
            _cliRunner.Execute($"{ScriptDir}{InitScript}", string.Empty).WaitForExit();

            string nodeAddress = _fileSystem.File.ReadAllText($"{HomeDir}{AddressFile}");

            nodeAddress = Regex.Replace(nodeAddress, NewLine, string.Empty);

            return Ok(nodeAddress);
        }

        /// <summary>
        /// Initializes the specified genesis block.
        /// </summary>
        /// <param name="genesisDto">Genesis block of the Ethereum PoA chain.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("genesis")]
        public ActionResult PostGenesis(GenesisDto genesisDto)
        {
            string genesisJson = JsonConvert.SerializeObject(genesisDto, _jsonSerializerSettings);

            _fileSystem.File.WriteAllText($"{HomeDir}{GenesisFile}", genesisJson);

            _cliRunner.Execute($"{ScriptDir}{InitGenesisScript}", string.Empty).WaitForExit();
            
            return Ok();
        }
        
        /// <summary>
        /// Starts connects this consensus node to the Ethereum PoA blockchain.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("peer")]
        public ActionResult PostPeer()
        {
            Process startConsensusNode = _cliRunner.Execute($"{ScriptDir}{StartConsensusNodeScript}", string.Empty);

            startConsensusNode.WaitForExit();

            _cliRunner.Execute(BinSh, $"{ScriptDir}{EnodeScript}").WaitForExit();

            string enode = _fileSystem.File.ReadAllText($"{HomeDir}{EnodeFile}");

            enode = Regex.Replace(enode, NewLine, string.Empty);
            enode = Regex.Replace(enode, DoubleQuotes, string.Empty);

            return Ok(enode);
        }

        /// <summary>
        /// Announces the other nodes in this Ethereum network to this consensus node.
        /// </summary>
        /// <param name="nodes">Enode identifiers of all sealer nodes in the Ethereum PoA network.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("nodes")]
        public ActionResult PostNodes(NodesDto nodes)
        {
            string nodesJson = JsonConvert.SerializeObject(nodes, _jsonSerializerSettings);

            _fileSystem.File.WriteAllText($"{HomeDir}{NodesFile}", nodesJson);

            return Ok();
        }

        /// <summary>
        /// Start the sealing process on this consensus node.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("sealing")]
        public ActionResult PostSealing()
        {
            _cliRunner.Execute($"{ScriptDir}{StartMiningScript}", string.Empty).WaitForExit();

            return Ok();
        }
    }
}
