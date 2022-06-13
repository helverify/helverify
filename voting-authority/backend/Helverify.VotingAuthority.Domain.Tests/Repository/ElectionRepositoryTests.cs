using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Repository.Mapping;
using Helverify.VotingAuthority.Domain.Tests.Fake;
using Moq;

namespace Helverify.VotingAuthority.Domain.Tests.Repository
{
    internal class ElectionRepositoryTests
    {
        private IMapper _mapper;
        private Mock<IRepository<Registration>> _registrationRepository;
        private IMongoService<ElectionDao> _mongoService;

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
            _registrationRepository = new Mock<IRepository<Registration>>();
            _mongoService = new FakeMongoService<ElectionDao>();

            SetUpMongoService();
        }


        [Test]
        public async Task TestGetAsync()
        {
            // arrange
            IRepository<Election> repository = new ElectionRepository(_mongoService, _registrationRepository.Object, _mapper);

            // act
            IList<Election> elections = await repository.GetAsync();

            // assert
            Assert.That(elections.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task TestGetAsyncId()
        {
            // arrange
            string id = "12abf728";
            
            SetUpRegistration(id);
            
            IRepository<Election> repository = new ElectionRepository(_mongoService, _registrationRepository.Object, _mapper);

            // act
            Election election = await repository.GetAsync(id);

            // assert
            Assert.That(election.Id, Is.EqualTo(id));
            Assert.That(election.Name, Is.EqualTo("Test 1"));
            Assert.That(election.Registrations.Count, Is.EqualTo(1));
            Assert.That(election.Registrations.First().Id, Is.EqualTo("ffff"));
        }

        [Test]
        public async Task TestCreateAsync()
        {
            // arrange
            _mongoService = new FakeMongoService<ElectionDao>();

            IRepository<Election> repository = new ElectionRepository(_mongoService, _registrationRepository.Object, _mapper);

            Election election = new Election
            {
                Name = "Test",
                P =
                    "87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597"
                        .ConvertToBigInteger(),
                G =
                    "3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659"
                        .ConvertToBigInteger(),
                Question = "Test?",
                Options = new List<ElectionOption>
                {
                    new()
                    {
                        Name = "Yes"
                    },
                    new()
                    {
                        Name = "No"
                    }
                },
            };
            
            // act
            await repository.CreateAsync(election);
            
            // assert
            Assert.That((_mongoService as FakeMongoService<ElectionDao>)!.Entities.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TestUpdateAsync()
        {
            // arrange
            IRepository<Election> repository = new ElectionRepository(_mongoService, _registrationRepository.Object, _mapper);

            string id = "12abf728";

            Election election = new Election
            {
                Id = id,
                Name = "Test 42",
                Options = new List<ElectionOption>
                {
                    new()
                    {
                        Name = "Yes"
                    },
                    new()
                    {
                        Name = "No"
                    }
                },
                PublicKey = "a8bf7121829fd".ConvertToBigInteger(),
                P =
                    "87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597".ConvertToBigInteger(),
                G =
                    "3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659".ConvertToBigInteger(),
                Question = "?"
            };

            // act
            await repository.UpdateAsync(id, election);

            // assert
            Assert.That((await _mongoService.GetAsync(id)).Name, Is.EqualTo("Test 42"));
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            // arrange
            IRepository<Election> repository = new ElectionRepository(_mongoService, _registrationRepository.Object, _mapper);
            
            string id = "12abf728";

            // act
            await repository.DeleteAsync(id);

            // assert
            Assert.That((_mongoService as FakeMongoService<ElectionDao>).Entities.Count(e => e.Id == id), Is.EqualTo(0));
        }

        private void SetUpMongoService()
        {
            FakeMongoService<ElectionDao> fakeMongoService = (_mongoService as FakeMongoService<ElectionDao>)!;

            fakeMongoService.Entities.Add(new ElectionDao
                {
                    Id = "12abf728",
                    Name = "Test 1",
                    Options = new List<ElectionOptionDao>
                    {
                        new()
                        {
                            Name = "Yes"
                        },
                        new()
                        {
                            Name = "No"
                        }
                    },
                    PublicKey = "a8bf7121829fd",
                    P =
                        "87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597",
                    G =
                        "3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659",
                    Question = "?"
                });
                
            fakeMongoService.Entities.Add(new()
            {
                Id = "a2abf728",
                Name = "Test 1",
                Options = new List<ElectionOptionDao>
                {
                    new()
                    {
                        Name = "Yes"
                    },
                    new()
                    {
                        Name = "No"
                    }
                },
                PublicKey = "a8bf7121829fd",
                P =
                    "87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597",
                G =
                    "3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659",
                Question = "?"
            });

            fakeMongoService.Entities.Add(new()
            {
                Id = "fa215bf728",
                Name = "Test 2",
                Options = new List<ElectionOptionDao>
                {
                    new()
                    {
                        Name = "Yes"
                    },
                    new()
                    {
                        Name = "No"
                    },
                    new()
                    {
                        Name = "Maybe"
                    }
                },
                PublicKey = "f8bf7121829fd",
                P =
                    "87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597",
                G =
                    "3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659",
                Question = "???"
            });
        }

        private void SetUpRegistration(string electionId)
        {
            _registrationRepository.Setup(x => x.GetAsync()).ReturnsAsync(new List<Registration>
            {
                new()
                {
                    ElectionId = electionId,
                    Name = "Node 1",
                    PublicKey = "b82829fd".ConvertToBigInteger(),
                    Endpoint = new Uri("http://localhost:12345"),
                    Id = "ffff"
                },
                new()
                {
                    ElectionId = "fba121",
                    Name = "Node 2",
                    PublicKey = "182829fd".ConvertToBigInteger(),
                    Endpoint = new Uri("http://localhost:12345"),
                    Id = "eeee"
                }
            });
        }
    }
}
