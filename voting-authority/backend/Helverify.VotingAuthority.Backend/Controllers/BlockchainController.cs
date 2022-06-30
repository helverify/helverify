using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    [Route("/api/blockchain")]
    [ApiController]
    public class BlockchainController
    {
        private readonly IBlockchainSetup _blockchainSetup;
        private readonly IRepository<Blockchain> _bcRepository;
        
        private readonly IMapper _mapper;

        public BlockchainController(IBlockchainSetup blockchainSetup,
            IRepository<Blockchain> bcRepository,
            IMapper mapper)
        {
            _blockchainSetup = blockchainSetup;
            _bcRepository = bcRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Initializes the Proof-of-Authority blockchain using the consensus nodes registered.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<BlockchainDto> Post(BlockchainDto requestDto)
        {
            Blockchain blockchain = _mapper.Map<Blockchain>(requestDto);

            IList<Registration> registrations = blockchain.Registrations;

            string nodeAddress = await _blockchainSetup.CreateAccountsAsync(registrations);

            Genesis genesis = await _blockchainSetup.PropagateGenesisBlockAsync(registrations, new Account(nodeAddress, "1000000000000000000000000000000000000000000000"));

            NodesDto nodes = await _blockchainSetup.StartPeersAsync(registrations);

            await _blockchainSetup.InitializeNodesAsync(registrations, nodes);

            await _blockchainSetup.StartSealingAsync(registrations);

            _blockchainSetup.RegisterRpcEndpoint(genesis, nodes);

            blockchain = await _bcRepository.CreateAsync(blockchain);

            BlockchainDto blockchainDto = _mapper.Map<BlockchainDto>(blockchain);

            return blockchainDto;
        }

        /// <summary>
        /// Returns the blockchain instance. Exists only once.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BlockchainDto> Get()
        {
            Blockchain blockchain = (await _bcRepository.GetAsync()).OrderByDescending(bc => bc.Id).FirstOrDefault();

            BlockchainDto blockchainDto = _mapper.Map<BlockchainDto>(blockchain);

            return blockchainDto;
        }
    }
}
