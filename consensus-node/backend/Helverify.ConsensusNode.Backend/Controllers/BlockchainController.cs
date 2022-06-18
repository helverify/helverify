using System.Diagnostics;
using System.IO.Abstractions;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.ConsensusNode.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Helverify.ConsensusNode.Backend.Controllers
{
    [Route("api/blockchain")]
    [ApiController]
    public class BlockchainController: ControllerBase
    {
        private const string ScriptDir = "/app/scripts/";
        private const string HomeDir = "/home/eth/";
        
        private const string InitScript = "init.sh";
        private const string InitGenesisScript = "init-genesis.sh";
        private const string StartConsensusNodeScript = "start-consensusnode.sh";
        private const string StartMiningScript = "start-mining.sh";
        private const string EnodeScript = "enode.sh";

        private const string AddressFile = "address";
        private const string EnodeFile = "enode";
        private const string GenesisFile = "genesis.json";
        private const string NodesFile = "nodes.json";

        private readonly ICliRunner _cliRunner;
        private readonly IFileSystem _fileSystem;

        public BlockchainController(ICliRunner cliRunner, IFileSystem fileSystem)
        {
            _cliRunner = cliRunner;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Creates a new Ethereum account and returns its address
        /// </summary>
        /// <returns>Ethereum address of this consensus node</returns>
        [HttpPost]
        [Route("account")]
        public ActionResult PostAccount()
        {
            Process initialization = _cliRunner.Execute($"{ScriptDir}{InitScript}", string.Empty);

            initialization.WaitForExit();

            string nodeAddress = _fileSystem.File.ReadAllText($"{HomeDir}{AddressFile}");

            nodeAddress = Regex.Replace(nodeAddress, @"\n", "");

            return Ok(nodeAddress);
        }

        [HttpPost]
        [Route("genesis")]
        public ActionResult PostGenesis(GenesisDto genesisDto)
        {
            string genesisJson = JsonConvert.SerializeObject(genesisDto, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            _fileSystem.File.WriteAllText($"{HomeDir}{GenesisFile}", genesisJson);

            Process genesisInit = _cliRunner.Execute($"{ScriptDir}{InitGenesisScript}", string.Empty);

            genesisInit.WaitForExit();

            return Ok();
        }
        
        [HttpPost]
        [Route("peer")]
        public ActionResult PostPeer()
        {
            Process startConsensusNode = _cliRunner.Execute($"{ScriptDir}{StartConsensusNodeScript}", string.Empty);

            startConsensusNode.WaitForExit();

            Process getEnode = _cliRunner.Execute($"/bin/sh", $"{ScriptDir}{EnodeScript}");

            getEnode.WaitForExit();

            string enode = _fileSystem.File.ReadAllText($"{HomeDir}{EnodeFile}");

            enode = Regex.Replace(enode, @"\n", "");
            enode = Regex.Replace(enode, "\"", "");

            return Ok(enode);
        }

        [HttpPost]
        [Route("nodes")]
        public ActionResult PostNodes(NodesDto nodes)
        {
            string nodesJson = JsonConvert.SerializeObject(nodes, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            _fileSystem.File.WriteAllText($"{HomeDir}{NodesFile}", nodesJson);

            return Ok();
        }

        [HttpPost]
        [Route("sealing")]
        public ActionResult PostSealing()
        {
            Process genesisInit = _cliRunner.Execute($"{ScriptDir}{StartMiningScript}", string.Empty);

            genesisInit.WaitForExit();
            
            return Ok();
        }
    }
}
