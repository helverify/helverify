using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using MongoDB.Driver;

namespace Helverify.VotingAuthority.Domain.Repository
{
    /// <inheritdoc cref="IRepository{T}"/>
    public class PaperBallotRepository: IRepository<PaperBallot>
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
            // https://kevsoft.net/2020/02/28/finding-documents-in-mongodb-using-csharp.html
            
            PrintBallotDao? printBallotDao = await GetSinglePrintBallot(id);

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

        /// <summary>
        /// Load paper ballots by election
        /// </summary>
        /// <param name="election">Current election</param>
        /// <returns></returns>
        public async Task<IList<PaperBallot>> GetByElectionAsync(Election election)
        {
            IList<PrintBallotDao> printBallotDaos = (await _mongoService.Collection.FindAsync(pb => pb.ElectionId == election.Id)).ToList();

            IList<PaperBallot> paperBallots = _mapper.Map<IList<PaperBallot>>(printBallotDaos);

            return paperBallots;
        }

        /// <inheritdoc cref="IRepository{T}.UpdateAsync"/>
        public async Task<PaperBallot> UpdateAsync(string id, PaperBallot entity)
        {
            PrintBallotDao printBallotDao = _mapper.Map<PrintBallotDao>(entity);

            string mongoId = (await GetSinglePrintBallot(id)).Id;

            printBallotDao.Id = mongoId;

            await _mongoService.UpdateAsync(mongoId, printBallotDao);

            return _mapper.Map<PaperBallot>(printBallotDao);
        }

        /// <inheritdoc cref="IRepository{T}.DeleteAsync"/>
        public async Task DeleteAsync(string id)
        {
            string mongoId = (await GetSinglePrintBallot(id)).Id;
            
            await _mongoService.RemoveAsync(mongoId);
        }

        /// <summary>
        /// Bulk insert for mass ballot insertion.
        /// </summary>
        /// <param name="ballots">Paper ballots</param>
        /// <returns></returns>
        public async Task InsertMany(PaperBallot[] ballots)
        {
            IList<PrintBallotDao> printBallotDaos = _mapper.Map<IList<PrintBallotDao>>(ballots);

            await _mongoService.Collection.InsertManyAsync(printBallotDaos);
        }

        private async Task<PrintBallotDao> GetSinglePrintBallot(string id)
        {
            return await _mongoService.Collection.Find(p => p.BallotId == id).SingleAsync();
        }
    }
}
