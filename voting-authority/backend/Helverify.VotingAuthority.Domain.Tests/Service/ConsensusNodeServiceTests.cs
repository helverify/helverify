using System.Collections.Specialized;
using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.DataAccess.Rest;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Repository.Mapping;
using Helverify.VotingAuthority.Domain.Service;
using Helverify.VotingAuthority.Domain.Tests.Fake;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Tests.Service
{
    public class ConsensusNodeServiceTests
    {
        private IMapper _mapper;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ElectionProfile>();
                cfg.AddProfile<RegistrationProfile>();
                cfg.AddProfile<GenesisProfile>();
            }));
        }

        [Test]
        public async Task TestGenerateKeyPairAsync()
        {
            // arrange
            IRestClient fakeRestClient = new FakeRestClient();

            IConsensusNodeService consensusNodeService = new ConsensusNodeService(fakeRestClient, _mapper);

            BigInteger p = new BigInteger(
                "87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597",
                16);
            BigInteger g = new BigInteger(
                "3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659",
                16);
            
            Election election = new Election
            {
                P = p,
                G = g
            };

            // act

            await consensusNodeService.GenerateKeyPairAsync(new Uri("http://helverify.test"), election);

            // assert
            KeyPairRequestDto dto = ((fakeRestClient as FakeRestClient)!.Items.Single() as KeyPairRequestDto)!;

            Assert.That(dto.P, Is.EqualTo(p.ConvertToHexString()));
            Assert.That(dto.G, Is.EqualTo(g.ConvertToHexString()));
        }

        [Test]
        public void TestGenerateKeyPairAsyncEndpointNull()
        {
            // arrange
            IRestClient fakeRestClient = new FakeRestClient();

            IConsensusNodeService consensusNodeService = new ConsensusNodeService(fakeRestClient, _mapper);

            BigInteger p = new BigInteger(
                "87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597",
                16);
            BigInteger g = new BigInteger(
                "3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659",
                16);

            Election election = new Election
            {
                P = p,
                G = g
            };

            // act, assert

            Assert.ThrowsAsync<ArgumentException>(() => consensusNodeService.GenerateKeyPairAsync(null, election));
        }

        [Test]
        public async Task TestDecryptShareAsync()
        {
            // arrange
            IRestClient fakeRestClient = new FakeRestClient();

            IConsensusNodeService consensusNodeService = new ConsensusNodeService(fakeRestClient, _mapper);

            string c = "af9839193bc";
            string d = "ff9100adbe2";

            // act

            await consensusNodeService.DecryptShareAsync(new Uri("http://helverify.test"), c, d);

            // assert
            EncryptedShareRequestDto dto = ((fakeRestClient as FakeRestClient)!.Items.Single() as EncryptedShareRequestDto)!;

            Assert.That(dto.Cipher.C, Is.EqualTo(c));
            Assert.That(dto.Cipher.D, Is.EqualTo(d));
        }

        [Test]
        public async Task TestInitializeGenesisBlockAsync()
        {
            // arrange
            IRestClient fakeRestClient = new FakeRestClient();

            IConsensusNodeService consensusNodeService = new ConsensusNodeService(fakeRestClient, _mapper);

            int chainId = 13337;

            IList<Account> authorities = new List<Account>
            {
                new ("0x123adf", "1000"),
                new ("0x2803fa", "400")
            };

            IList<Account> prefunded = new List<Account>
            {
                new ("0xffff1a", "3000"),
                new ("0x12345f", "600")
            };

            Genesis genesis = new Genesis(chainId, authorities, prefunded);

            // act
            await consensusNodeService.InitializeGenesisBlockAsync(new Uri("http://helverify.test"), genesis);

            // assert
            GenesisDto dto = ((fakeRestClient as FakeRestClient)!.Items.Single() as GenesisDto)!;

            dto.Alloc.TryGetValue("0xffff1a", out AccountBalanceDto? accBalance1);
            dto.Alloc.TryGetValue("0x12345f", out AccountBalanceDto? accBalance2);

            Assert.That(dto.Config.ChainId, Is.EqualTo(chainId));
            Assert.That(accBalance1!.Balance, Is.EqualTo("3000"));
            Assert.That(accBalance2!.Balance, Is.EqualTo("600"));
        }

        [Test]
        public async Task TestCreateBcAccountAsync()
        {
            // arrange
            IRestClient fakeRestClient = new FakeRestClient();

            IConsensusNodeService consensusNodeService = new ConsensusNodeService(fakeRestClient, _mapper);
            
            // act
            await consensusNodeService.CreateBcAccountAsync(new Uri("http://helverify.test"));

            // assert
            Assert.That((fakeRestClient as FakeRestClient)!.Items.Single(), Is.Null);
        }

        [Test]
        public async Task TestStartPeersAsync()
        {
            // arrange
            IRestClient fakeRestClient = new FakeRestClient();

            IConsensusNodeService consensusNodeService = new ConsensusNodeService(fakeRestClient, _mapper);
            
            // act
            await consensusNodeService.StartPeersAsync(new Uri("http://helverify.test"));

            // assert
            Assert.That((fakeRestClient as FakeRestClient)!.Items.Single(), Is.Null);
        }

        [Test]
        public async Task TestInitializeNodesAsync()
        {
            // arrange
            IRestClient fakeRestClient = new FakeRestClient();

            IConsensusNodeService consensusNodeService = new ConsensusNodeService(fakeRestClient, _mapper);

            NodesDto nodes = new NodesDto
            {
                Nodes = new List<string>
                {
                    "12098afdab",
                    "c0919021de"
                }
            };

            // act
            await consensusNodeService.InitializeNodesAsync(new Uri("http://helverify.test"), nodes);

            // assert
            NodesDto dto = ((fakeRestClient as FakeRestClient)!.Items.Single() as NodesDto)!;

            Assert.That(dto.Nodes, Is.EqualTo(nodes.Nodes));
        }

        [Test]
        public async Task TestStartSealingAsync()
        {
            // arrange
            IRestClient fakeRestClient = new FakeRestClient();

            IConsensusNodeService consensusNodeService = new ConsensusNodeService(fakeRestClient, _mapper);

            // act
            await consensusNodeService.StartSealingAsync(new Uri("http://helverify.test"));

            // assert
            Assert.That((fakeRestClient as FakeRestClient)!.Items.Single(), Is.Null);
        }

    }
}
