using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Repository.Mapping;
using Helverify.VotingAuthority.Domain.Service;
using Moq;

namespace Helverify.VotingAuthority.Domain.Tests.Service
{
    public class BlockchainSetupTests
    {
        private IMapper _mapper = null!;
        private IList<Registration> _registrations = null!;
        private Mock<IConsensusNodeService> _consensusNodeService = null!;
        private ICliRunner _cliRunner = null!;
        private IFileSystem _fileSystem = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GenesisProfile>();
            }));

            _registrations = new List<Registration>
            {
                new ()
                {
                    Endpoint = new Uri("http://localhost:5555")
                },
                new ()
                {
                    Endpoint = new Uri("http://localhost:6666")
                }
            };
        }

        [SetUp]
        public void SetUp()
        {
            _consensusNodeService = new Mock<IConsensusNodeService>();
            _cliRunner = new CliRunner();
            _fileSystem = new MockFileSystem();
        }

        [Test]
        public void TestStartSealingAsync()
        {
            // arrange
            IBlockchainSetup setup = new BlockchainSetup(_consensusNodeService.Object, _cliRunner, _mapper, _fileSystem);

            // act, assert
            Assert.DoesNotThrowAsync(() => setup.StartSealingAsync(_registrations));
        }

        [Test]
        public void TestInitializeNodesAsync()
        {
            // arrange
            NodesDto nodes = new NodesDto();

            IBlockchainSetup setup = new BlockchainSetup(_consensusNodeService.Object, _cliRunner, _mapper, _fileSystem);

            // act, assert
            Assert.DoesNotThrowAsync(() => setup.InitializeNodesAsync(_registrations, nodes));
        }

        [Test]
        public async Task TestStartPeersAsync()
        {
            // arrange
            Uri endpoint1 = new Uri("http://localhost:5555");
            Uri endpoint2 = new Uri("http://localhost:6666");
            string enode1 = "123456";
            string enode2 = "654321";

            IList<Registration> registrations = new List<Registration>
            {
                new ()
                {
                    Endpoint = endpoint1,
                },
                new ()
                {
                    Endpoint = endpoint2,
                }
            };

            _consensusNodeService.Setup(c => c.StartPeers(endpoint1)).ReturnsAsync(enode1);
            _consensusNodeService.Setup(c => c.StartPeers(endpoint2)).ReturnsAsync(enode2);

            IBlockchainSetup setup = new BlockchainSetup(_consensusNodeService.Object, _cliRunner, _mapper, _fileSystem);

            // act
            NodesDto nodes = await setup.StartPeersAsync(registrations);

            // assert
            Assert.That(nodes.Nodes[0], Is.EqualTo(enode1));
            Assert.That(nodes.Nodes[1], Is.EqualTo(enode2));
            Assert.That(registrations[0].Enode, Is.EqualTo(enode1));
            Assert.That(registrations[1].Enode, Is.EqualTo(enode2));
        }

        [Test]
        public void TestPropagateGenesisAsync()
        {
            // arrange
            IBlockchainSetup setup = new BlockchainSetup(_consensusNodeService.Object, _cliRunner, _mapper, _fileSystem);

            // act, assert
            Assert.DoesNotThrowAsync(() => setup.PropagateGenesisBlockAsync(_registrations));
        }

        [Test]
        public async Task TestCreateAccountsAsync()
        {
            Uri endpoint1 = new Uri("http://localhost:5555");
            Uri endpoint2 = new Uri("http://localhost:6666");
            string bcAddress1 = "0x123456789";
            string bcAddress2 = "0x987654321";
            
            IList<Registration> registrations = new List<Registration>
            {
                new ()
                {
                    Endpoint = endpoint1,
                },
                new ()
                {
                    Endpoint = endpoint2,
                }
            };

            _consensusNodeService.Setup(c => c.CreateBcAccount(endpoint1)).ReturnsAsync(bcAddress1);
            _consensusNodeService.Setup(c => c.CreateBcAccount(endpoint2)).ReturnsAsync(bcAddress2);

            BlockchainSetup setup = new BlockchainSetup(_consensusNodeService.Object, _cliRunner, _mapper, _fileSystem);

            // act
            await setup.CreateAccountsAsync(registrations);

            // assert
            Assert.That(registrations[0].Account.Address, Is.EqualTo(bcAddress1));
            Assert.That(registrations[1].Account.Address, Is.EqualTo(bcAddress2));
        }

        [Test]
        public void TestRegisterRpcEndpoint()
        {
            // arrange
            _fileSystem.Directory.CreateDirectory("/home/eth");
            Mock<ICliRunner> cliRunner = new Mock<ICliRunner>();
            cliRunner.Setup(c => c.Execute(It.IsAny<string>(), It.IsAny<string>()));

            IBlockchainSetup setup = new BlockchainSetup(_consensusNodeService.Object, cliRunner.Object, _mapper, _fileSystem);
            Genesis genesis = new Genesis(13337, new List<Account>(), new List<Account>());
            NodesDto nodes = new NodesDto();

            // act
            setup.RegisterRpcEndpoint(genesis, nodes);
            
            // assert
            Assert.That(_fileSystem.File.Exists("/home/eth/genesis.json"), Is.True);
            Assert.That(_fileSystem.File.Exists("/home/eth/nodes.json"), Is.True);
        }
    }
}
