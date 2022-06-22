using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.DataAccess.Ethereum.Contract;

namespace Helverify.VotingAuthority.Domain.Repository
{
    /// <inheritdoc cref="IRepository{T}"/>
    internal class PaperBallotRepository: IRepository<PaperBallot>
    {
        private readonly IMongoService<PrintBallotDao> _mongoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mongoService">MongoDB accessor</param>
        /// <param name="mapper">Automapper</param>
        public PaperBallotRepository(IMongoService<PrintBallotDao> mongoService, IMapper mapper)
        {
            _mongoService = mongoService;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IRepository{T}.CreateAsync"/>
        public async Task<PaperBallot> CreateAsync(PaperBallot entity)
        {
            PrintBallotDao printBallot = _mapper.Map<PrintBallotDao>(entity);

            await _mongoService.CreateAsync(printBallot);

            return entity;
        }

        /// <inheritdoc cref="IRepository{T}.GetAsync(string {id})"/>
        public async Task<PaperBallot> GetAsync(string id)
        {
            PrintBallotDao printBallotDao = await _mongoService.GetAsync(id);

            PaperBallot paperBallot = _mapper.Map<PaperBallot>(printBallotDao);

            return paperBallot;
        }

        /// <inheritdoc cref="IRepository{T}.GetAsync()"/>
        public async Task<IList<PaperBallot>> GetAsync()
        {
            IList<PrintBallotDao> printBallotDaos = await _mongoService.GetAsync();

            IList<PaperBallot> paperBallots = _mapper.Map<IList<PaperBallot>>(printBallotDaos);
            
            return paperBallots;
        }

        /// <inheritdoc cref="IRepository{T}.UpdateAsync"/>
        public async Task<PaperBallot> UpdateAsync(string id, PaperBallot entity)
        {
            PrintBallotDao printBallotDao = _mapper.Map<PrintBallotDao>(entity);

            await _mongoService.UpdateAsync(id, printBallotDao);

            return _mapper.Map<PaperBallot>(printBallotDao);
        }

        /// <inheritdoc cref="IRepository{T}.DeleteAsync"/>
        public async Task DeleteAsync(string id)
        {
            await _mongoService.RemoveAsync(id);
        }
    }
}
