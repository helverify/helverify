using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;

namespace Helverify.VotingAuthority.Domain.Repository
{
    /// <inheritdoc cref="IRepository{T}"/>
    internal class GenericRepository<TDomain, TDao>: IRepository<TDomain> where TDao: IEntity
    {
        private readonly IMongoService<TDao> _mongoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mongoService">MongoDB accessor</param>
        /// <param name="mapper">Automapper</param>
        public GenericRepository(IMongoService<TDao> mongoService, IMapper mapper)
        {
            _mongoService = mongoService;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IRepository{T}.CreateAsync"/>
        public async Task<TDomain> CreateAsync(TDomain entity)
        {
            TDao dao = _mapper.Map<TDao>(entity);

            await _mongoService.CreateAsync(dao);

            return _mapper.Map<TDomain>(dao);
        }

        /// <inheritdoc cref="IRepository{T}.GetAsync(string {id})"/>
        public async Task<TDomain> GetAsync(string id)
        {
            TDao dao = await _mongoService.GetAsync(id);

            TDomain domain = _mapper.Map<TDomain>(dao);

            return domain;
        }

        /// <inheritdoc cref="IRepository{T}.GetAsync()"/>
        public async Task<IList<TDomain>> GetAsync()
        {
            IList<TDao> daos = (await _mongoService.GetAsync()).ToList();

            IList<TDomain> domains = _mapper.Map<IList<TDomain>>(daos);

            return domains;
        }

        /// <inheritdoc cref="IRepository{T}.UpdateAsync"/>
        public async Task<TDomain> UpdateAsync(string id, TDomain entity)
        {
            TDao dao = _mapper.Map<TDao>(entity);

            await _mongoService.UpdateAsync(id, dao);

            return _mapper.Map<TDomain>(dao);
        }

        /// <inheritdoc cref="IRepository{T}.DeleteAsync"/>
        public async Task DeleteAsync(string id)
        {
            await _mongoService.RemoveAsync(id);
        }
    }
}
