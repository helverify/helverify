using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository
{
    internal class RegistrationRepository : IRepository<Registration>
    {
        private readonly IMongoService<RegistrationDao> _mongoService;
        private readonly IMapper _mapper;

        public RegistrationRepository(IMongoService<RegistrationDao> mongoService, IMapper mapper)
        {
            _mongoService = mongoService;
            _mapper = mapper;
        }

        public async Task<Registration> CreateAsync(Registration registration)
        {
            RegistrationDao registrationDao = _mapper.Map<RegistrationDao>(registration);

            await _mongoService.CreateAsync(registrationDao);

            return _mapper.Map<Registration>(registrationDao);
        }

        public async Task<Registration> GetAsync(string id)
        {
            RegistrationDao registrationDao = await _mongoService.GetAsync(id);

            Registration registration = _mapper.Map<Registration>(registrationDao);

            return registration;
        }

        public async Task<IList<Registration>> GetAsync()
        {
            IList<RegistrationDao> registrationDaos = (await _mongoService.GetAsync()).ToList();

            IList<Registration> registrations = _mapper.Map<IList<Registration>>(registrationDaos);

            return registrations;
        }

        public async Task<Registration> UpdateAsync(string id, Registration registration)
        {
            RegistrationDao registrationDao = _mapper.Map<RegistrationDao>(registration);

            await _mongoService.UpdateAsync(id, registrationDao);

            return _mapper.Map<Registration>(registrationDao);
        }

        public async Task DeleteAsync(string id)
        {
            await _mongoService.RemoveAsync(id);
        }
    }
}
