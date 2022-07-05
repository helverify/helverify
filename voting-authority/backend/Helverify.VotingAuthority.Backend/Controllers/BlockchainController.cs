using AutoMapper;
using Helverify.VotingAuthority.Application.Services;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Microsoft.AspNetCore.Mvc;

namespace Helverify.VotingAuthority.Backend.Controllers
{
    /// <summary>
    /// Blockchain initialization and status.
    /// </summary>
    [Route("/api/blockchain")]
    [ApiController]
    public class BlockchainController
    {
        private readonly IBlockchainService _blockchainService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="blockchainService">Facade for Blockchain domain logic.</param>
        /// <param name="mapper">Automapper</param>
        public BlockchainController(IBlockchainService blockchainService, IMapper mapper)
        {
            _blockchainService = blockchainService;
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

            blockchain = await _blockchainService.Initialize(blockchain);

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
            Blockchain? blockchain = await _blockchainService.GetInstance();

            BlockchainDto blockchainDto = _mapper.Map<BlockchainDto>(blockchain);

            return blockchainDto;
        }
    }
}
