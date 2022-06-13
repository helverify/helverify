using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Repository.Mapping;
using Helverify.VotingAuthority.Domain.Tests.Fake;

namespace Helverify.VotingAuthority.Domain.Tests.Repository
{
    internal class RegistrationRepositoryTests
    {
        private IMapper _mapper;
        private IMongoService<RegistrationDao> _mongoService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ElectionProfile>();
                cfg.AddProfile<RegistrationProfile>();
            }));
        }

        [SetUp]
        public void SetUp()
        {
            _mongoService = new FakeMongoService<RegistrationDao>();

            SetUpMongoService();
        }

        [Test]
        public async Task TestGetAsync()
        {
            // arrange
            IRepository<Registration> repository = new RegistrationRepository(_mongoService, _mapper);

            // act
            IList<Registration> registrations = await repository.GetAsync();

            // assert
            Assert.That(registrations.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task TestGetAsyncId()
        {
            // arrange
            IRepository<Registration> repository = new RegistrationRepository(_mongoService, _mapper);
            string id = "98281f7898021a8";

            // act
            Registration registration = await repository.GetAsync(id);

            // assert
            Assert.That(registration.Name, Is.EqualTo("Test Node 1"));
        }

        [Test]
        public async Task TestCreateAsync()
        {
            // arrange
            _mongoService = new FakeMongoService<RegistrationDao>();

            IRepository<Registration> repository = new RegistrationRepository(_mongoService,  _mapper);

            Registration registration = new Registration
            {
                Id = "3413513a5135b3e",
                Name = "Test Node 1",
                ElectionId = "ffce21",
                Endpoint = new Uri("http://localhost:12345"),
                PublicKey = "1354168afe2".ConvertToBigInteger()
            };

            // act
            await repository.CreateAsync(registration);

            // assert
            Assert.That((_mongoService as FakeMongoService<RegistrationDao>)!.Entities.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TestUpdateAsync()
        {
            // arrange
            IRepository<Registration> repository = new RegistrationRepository(_mongoService, _mapper);

            string id = "98281f7898021a8";

            Registration registration = new Registration
            {
                Id = id,
                Name = "Test Node 42",
                ElectionId = "ffce21",
                Endpoint = new Uri("http://localhost:12345"),
                PublicKey = "af29121befc".ConvertToBigInteger()
            };

            // act
            await repository.UpdateAsync(id, registration);
            
            // assert
            Assert.That((await _mongoService.GetAsync(id)).Name, Is.EqualTo("Test Node 42"));
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            // arrange
            IRepository<Registration> repository = new RegistrationRepository(_mongoService, _mapper);

            string id = "98281f7898021a8";

            // act
            await repository.DeleteAsync(id);

            // assert
            Assert.That((_mongoService as FakeMongoService<RegistrationDao>).Entities.Count(e => e.Id == id), Is.EqualTo(0));
        }

        private void SetUpMongoService()
        {
            FakeMongoService<RegistrationDao> fakeMongoService = (_mongoService as FakeMongoService<RegistrationDao>)!;

            fakeMongoService.Entities.Add(new RegistrationDao
            {
                Id = "98281f7898021a8",
                Name = "Test Node 1",
                ElectionId = "ffce21",
                Endpoint = new Uri("http://localhost:12345"),
                PublicKey = "af29121befc"
            });

            fakeMongoService.Entities.Add(new RegistrationDao
            {
                Id = "98281f7898021a7",
                Name = "Test Node 2",
                ElectionId = "ffce21",
                Endpoint = new Uri("http://localhost:12346"),
                PublicKey = "cf29121befc"
            });
        }
    }
}
