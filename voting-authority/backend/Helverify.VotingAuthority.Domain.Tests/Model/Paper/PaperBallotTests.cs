using AutoMapper;
using Helverify.Cryptography.Common;
using Helverify.Cryptography.Encryption;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Helverify.VotingAuthority.Domain.Repository.Mapping;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Tests.Model.Paper
{
    internal class PaperBallotTests
    {
        private Election _election;
        private VirtualBallot _ballot1;
        private VirtualBallot _ballot2;

        [SetUp]
        public void SetUp()
        {
            BigInteger p = new BigInteger(
                "87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597",
                16);
            BigInteger g = new BigInteger(
                "3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659",
                16);

            IElGamal elGamal = new ExponentialElGamal();
            AsymmetricCipherKeyPair keyPair = elGamal.KeyGen(p, g);
            DHPublicKeyParameters publicKey = (keyPair.Public as DHPublicKeyParameters)!;

            _election = new Election
            {
                Name = "Test",
                Question = "Vote for whom?",
                Options = new List<ElectionOption>
                {
                    new (){ Name = "Yes" },
                    new (){ Name = "No"},
                    new (){ Name = "Maybe" }
                },
                P = p,
                G = g,
                PublicKey = publicKey.Y
            };

            BallotTemplate ballotTemplate = new BallotTemplate(_election);

            _ballot1 = ballotTemplate.Encrypt();
            _ballot2 = ballotTemplate.Encrypt();
        }
        
        [Test]
        public void TestPaperBallotPerformance()
        {
            // arrange
            int testIterations = 100;

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BallotProfile>();
            }));

            DhGroup dhGroup = DhGroups.Get(DhGroups.Modp2048);

            BigInteger p = dhGroup.P;
            BigInteger g = dhGroup.G;

            IElGamal elGamal = new ExponentialElGamal();
            AsymmetricCipherKeyPair keyPair = elGamal.KeyGen(p, g);
            DHPublicKeyParameters publicKey = (keyPair.Public as DHPublicKeyParameters)!;

            Election election = new Election
            {
                Name = "Test",
                Question = "Vote for whom?",
                Options = new List<ElectionOption>
                {
                    new (){ Name = "Yes" },
                    new (){ Name = "No"},
                    new (){ Name = "Maybe" }
                },
                P = p,
                G = g,
                PublicKey = publicKey.Y
            };

            BallotTemplate ballotTemplate = new BallotTemplate(election);

            PaperBallot[] ballots = new PaperBallot[testIterations];

            Parallel.For(0, testIterations, (i) =>
            {
                VirtualBallot ballot1 = ballotTemplate.Encrypt();
                VirtualBallot ballot2 = ballotTemplate.Encrypt();

                VirtualBallotDao ballot1Dao = mapper.Map<VirtualBallotDao>(ballot1);
                VirtualBallotDao ballot2Dao = mapper.Map<VirtualBallotDao>(ballot2);

                // act
                PaperBallot paperBallot = new PaperBallot(election, ballot1, ballot2);

                ballots[i] = paperBallot;
            });
            
            // assert
            Assert.That(ballots.Count, Is.EqualTo(testIterations)); 
        }

        [Test]
        public void TestGetRandomness()
        {
            // arrange
            PaperBallot paperBallot = new PaperBallot(_election, _ballot1, _ballot2);

            // act
            IDictionary<string, IList<BigInteger>> randomness1 = paperBallot.GetRandomness(0);
            IDictionary<string, IList<BigInteger>> randomness2 = paperBallot.GetRandomness(1);

            // assert
            Assert.That(randomness1, Is.EqualTo(_ballot1.GetRandomness()));
            Assert.That(randomness2, Is.EqualTo(_ballot2.GetRandomness()));
        }

        [Test]
        public void TestGetRandomnessInvalidIndex()
        {
            // arrange
            PaperBallot paperBallot = new PaperBallot(_election, _ballot1, _ballot2);

            // act, assert
            Assert.Throws<ArgumentException>(() => paperBallot.GetRandomness(2));
        }

        [Test]
        public void TestHasShortCodes()
        {
            // arrange
            PaperBallot paperBallot = new PaperBallot(_election, _ballot1, _ballot2);

            IList<string> shortCodes1 = _ballot1.EncryptedOptions.Select(o => o.ShortCode).ToList();
            IList<string> shortCodes2 = _ballot2.EncryptedOptions.Select(o => o.ShortCode).ToList();

            string shortCode11 = shortCodes1[0];
            string shortCode12 = shortCodes1[1];
            string shortCode2 = shortCodes2[0];

            // act
            bool hasShortCodes1 = paperBallot.HasShortCodes(new List<string>{shortCode11, shortCode12});
            bool hasShortCodes2 = paperBallot.HasShortCodes(new List<string>{shortCode2});
            bool hasShortCodes3 = paperBallot.HasShortCodes(new List<string>{shortCode11, "inexistent shortcode"});

            // assert
            Assert.True(hasShortCodes1);
            Assert.True(hasShortCodes2);
            Assert.False(hasShortCodes3);
        }
    }
}
