using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository
{
    /// <inheritdoc cref="IRepository{T}"/>
    internal class RegistrationRepository : IRepository<Registration>
    {
        private readonly IMongoService<RegistrationDao> _mongoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mongoService">MongoDB accessor</param>
        /// <param name="mapper">Automapper</param>
        public RegistrationRepository(IMongoService<RegistrationDao> mongoService, IMapper mapper)
        {
            _mongoService = mongoService;
            _mapper = mapper;
        }

        /// <inheritdoc cref="IRepository{T}.CreateAsync"/>
        public async Task<Registration> CreateAsync(Registration entity)
        {
            RegistrationDao registrationDao = _mapper.Map<RegistrationDao>(entity);

            await _mongoService.CreateAsync(registrationDao);

            return _mapper.Map<Registration>(registrationDao);
        }

        /// <inheritdoc cref="IRepository{T}.GetAsync(string {id})"/>
        public async Task<Registration> GetAsync(string id)
        {
            RegistrationDao registrationDao = await _mongoService.GetAsync(id);

            Registration registration = _mapper.Map<Registration>(registrationDao);

            return registration;
        }

        /// <inheritdoc cref="IRepository{T}.GetAsync()"/>
        public async Task<IList<Registration>> GetAsync()
        {
            IList<RegistrationDao> registrationDaos = (await _mongoService.GetAsync()).ToList();

            IList<Registration> registrations = _mapper.Map<IList<Registration>>(registrationDaos);

            return registrations;
        }

        /// <inheritdoc cref="IRepository{T}.UpdateAsync"/>
        public async Task<Registration> UpdateAsync(string id, Registration entity)
        {
            RegistrationDao registrationDao = _mapper.Map<RegistrationDao>(entity);

            await _mongoService.UpdateAsync(id, registrationDao);

            return _mapper.Map<Registration>(registrationDao);
        }

        /// <inheritdoc cref="IRepository{T}.DeleteAsync"/>
        public async Task DeleteAsync(string id)
        {
            await _mongoService.RemoveAsync(id);
        }
    }
}
