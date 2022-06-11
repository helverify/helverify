using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository
{
    /// <inheritdoc cref="IRepository{T}"/>
    internal class ElectionRepository : IRepository<Election>
    {
        private readonly IMongoService<ElectionDao> _mongoService;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mongoService">MongoDB accessor</param>
        /// <param name="registrationRepository">Consensus node registration repository</param>
        /// <param name="mapper">Automapper</param>
        public ElectionRepository(IMongoService<ElectionDao> mongoService, IRepository<Registration> registrationRepository, IMapper mapper)
        {
            _mongoService = mongoService;
            _registrationRepository = registrationRepository;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IRepository{T}.CreateAsync"/>
        public async Task<Election> CreateAsync(Election entity)
        {
            ElectionDao electionDao = _mapper.Map<ElectionDao>(entity);

            await _mongoService.CreateAsync(electionDao);

            return _mapper.Map<Election>(electionDao);
        }

        /// <inheritdoc cref="IRepository{T}.GetAsync(string {id})"/>
        public async Task<Election> GetAsync(string id)
        {
            ElectionDao electionDao = await _mongoService.GetAsync(id);

            Election election = _mapper.Map<Election>(electionDao);

            election.Registrations = (await _registrationRepository.GetAsync()).Where(r => r.ElectionId == id).ToList();

            return election;
        }

        /// <inheritdoc cref="IRepository{T}.GetAsync()"/>
        public async Task<IList<Election>> GetAsync()
        {
            IList<ElectionDao> electionDaos = (await _mongoService.GetAsync()).ToList();

            IList<Election> elections = _mapper.Map<IList<Election>>(electionDaos);

            return elections;
        }

        /// <inheritdoc cref="IRepository{T}.UpdateAsync"/>
        public async Task<Election> UpdateAsync(string id, Election entity)
        {
            ElectionDao electionDao = _mapper.Map<ElectionDao>(entity);

            await _mongoService.UpdateAsync(id, electionDao);

            return _mapper.Map<Election>(electionDao);
        }

        /// <inheritdoc cref="IRepository{T}.DeleteAsync"/>
        public async Task DeleteAsync(string id)
        {
            await _mongoService.RemoveAsync(id);
        }
    }
}
