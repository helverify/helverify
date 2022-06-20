using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Service;
using Moq;

namespace Helverify.VotingAuthority.Domain.Tests.Model.Blockchain
{
    public class BlockchainSetupTests
    {
        [Test]
        public void TestStartSealingAsync()
        {
            // arrange
            IList<Registration> registrations = new List<Registration>
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

            Mock<IConsensusNodeService> consensusNodeService = new Mock<IConsensusNodeService>();
            
            BlockchainSetup setup = new BlockchainSetup(consensusNodeService.Object);
            
            // act, assert
            Assert.DoesNotThrowAsync(() => setup.StartSealingAsync(registrations));
        }

        [Test]
        public void TestInitializeNodesAsync()
        {
            // arrange
            IList<Registration> registrations = new List<Registration>
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

            NodesDto nodes = new NodesDto();

            Mock<IConsensusNodeService> consensusNodeService = new Mock<IConsensusNodeService>();

            BlockchainSetup setup = new BlockchainSetup(consensusNodeService.Object);

            // act, assert
            Assert.DoesNotThrowAsync(() => setup.InitializeNodesAsync(registrations, nodes));
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

            Mock<IConsensusNodeService> consensusNodeService = new Mock<IConsensusNodeService>();
            consensusNodeService.Setup(c => c.StartPeers(endpoint1)).ReturnsAsync(enode1);
            consensusNodeService.Setup(c => c.StartPeers(endpoint2)).ReturnsAsync(enode2);

            BlockchainSetup setup = new BlockchainSetup(consensusNodeService.Object);

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
            IList<Registration> registrations = new List<Registration>
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

            Mock<IConsensusNodeService> consensusNodeService = new Mock<IConsensusNodeService>();

            BlockchainSetup setup = new BlockchainSetup(consensusNodeService.Object);

            // act, assert
            Assert.DoesNotThrowAsync(() => setup.PropagateGenesisBlockAsync(registrations));
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

            Mock<IConsensusNodeService> consensusNodeService = new Mock<IConsensusNodeService>();
            consensusNodeService.Setup(c => c.CreateBcAccount(endpoint1)).ReturnsAsync(bcAddress1);
            consensusNodeService.Setup(c => c.CreateBcAccount(endpoint2)).ReturnsAsync(bcAddress2);

            BlockchainSetup setup = new BlockchainSetup(consensusNodeService.Object);

            // act
            await setup.CreateAccountsAsync(registrations);

            // assert
            Assert.That(registrations[0].Account.Address, Is.EqualTo(bcAddress1));
            Assert.That(registrations[1].Account.Address, Is.EqualTo(bcAddress2));
        }
    }
}
