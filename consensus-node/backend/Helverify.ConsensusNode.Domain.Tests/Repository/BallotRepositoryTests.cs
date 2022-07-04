using System.IO.Abstractions.TestingHelpers;
using AutoMapper;
using Helverify.ConsensusNode.DataAccess.Dao;
using Helverify.ConsensusNode.DataAccess.Ipfs;
using Helverify.ConsensusNode.Domain.Mapping;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.ConsensusNode.Domain.Repository;
using Moq;

namespace Helverify.ConsensusNode.Domain.Tests.Repository
{
    internal class BallotRepositoryTests
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BallotEncryptionProfile>();
            }));
        }

        [Test]
        public async Task TestGetBallotEncryption()
        {
            // arrange
            Mock<IStorageClient> storageClient = new Mock<IStorageClient>();
            storageClient.Setup(s => s.Retrieve<VirtualBallotDao>(It.IsAny<string>()))
                .ReturnsAsync(new VirtualBallotDao
                {
                    EncryptedOptions = new List<EncryptedOptionDao>
                    {
                        new()
                        {
                            Values = new List<EncryptedOptionValueDao>
                            {
                                new()
                                {
                                    Cipher = new CipherTextDao
                                    {
                                        C =
                                            "bfddc41260240d5e9b0501500fd98765006d47ef9428f5dd1ae08feae43b62e98055b94b6aa43abd53e05b38da68e129656829a5da1d2ccf1d0c437b4f1568bb74d94222771076f42eea9846adc211b464465a613b74d9cd34aa312fe4976e16128229d4d6a5ea57dbe05317c94ade02f14124beaa88bfe1c4fbd1d511adc4affcc298f684f4d4fa9ebb4ed0a473a700022b791930fe90c935a8912214948a9f512f1eac2d53ba0d1877b942b5395526fcbf2ffd23911bfbe233a174646628e3889035de83864ba005d645ad2394561933c1ab0f519b10ad9bde116de6ed9a2f204e20f83033f543ccff3c3a57b14a028978eeaf01aa32ce8d4121444db45b5b",
                                        D =
                                            "f60b5df3b265e8cc8f0791ed50f4e34781cc0df7a8342c130b23108388d68f296278faf96fc476a7183cd845c7d0dc560f2fdf39773778c7c5a100a460a43396763189102ef1c9418e0f6b8fc2cdb6d1c63e3b3e37c64dc6cb6e6936585bf93279119b898c8310b017674f8b8dcff47035b532fa16b97d212acd84e7d052bcf7a0797cf6049e4bdda862f556e7e49a1464f684bc846e1500e2d5d5cc8edf949277cbd48aafddb6cce83e7ed7ec4a9812aea5658380708ac97e9aac97c07fa4e6963b74f021081611b06b22c284cd0fdac8dc8c4c4ad79f5e6cd570f233959d40ce00f494b760de110e862960f431666698ad642ac30f0d4e32a4a88cc37eaccd"
                                    }
                                },
                                new()
                                {
                                    Cipher = new CipherTextDao
                                    {
                                        C =
                                            "96722e5488f7c00eb61632ca96ea44136f20294b3e00378646c9d4d191dd2984948e47195e73bb37f40dd71e47433f6e729baa68379769bba6b25bc36f2d809b94ff0427265dd0655e1af5c2121abf69e07fc24049cbea524ecf796e8111c65c94e6fe0b2d93b5f81866d95c1b61bfd9f999844fbca379f9f09ca73133a3f73c33910b32137783e525be4661cd11a8f929f10fc27dc22ce661e4a2af6b660a327c1095292c0e8c06af64e20abce19d97ffdf9e01bfd2413ea846082e752f154621e86b7ec4faf056dee807fa9a724d6108fc98c46437644c1bc0b819662be8efbae73dc1df5ce9cd6f15183ff41f3d89870d187f695072ad4a7972788fd9c3c",
                                        D =
                                            "3d7d034538fa860f517f067cb287d2301a94082ae0ac200ea3042baebe2600e4c20ff8d948674db52e66d90abf64890ddcc85465897f80f04fecb5d47150bda6107f9e3552a95bedc0af585e3380e254050b9ce04bc0cd779a4d7f178a9ddab631830756d92c4b9526c240dea6113149a65d157042e4a492c3a99c5413e30412a6266d03f1b348e4c97cd669dd9f8477006f3123fb4a13a33dcee8d84f52f8432d62f3c1f9b4bc70dc555537fb51d862290d3b74b06db997ee5d019f5fa3fef88fecc2397bb8740a8fe811f104bd30ee023e116682fdfdf9bf68a8b4fb9bf53109ff1d9c814e754bf755ac26c1cd90cd2acfa10c69b891bf17013e1d5df7ca5"
                                    }
                                }
                            },
                            ShortCode = "50"
                        },
                        new()
                        {
                            Values = new List<EncryptedOptionValueDao>
                            {
                                new()
                                {
                                    Cipher = new CipherTextDao
                                    {
                                        C =
                                            "a50921c0a781ef62a9dac2425427823f04e3a2ee161feb9d0b9ccabd0abb34556996c11dfb96694399787ddd1af8c97d119e1a3251be360cad4813ec809e6efb323d844d49ba03cbdcb98c9297328ec8fb1b141483d62c66fdca888fe69fdc8a48b98817ab9505f3f7ca12a461d0fbb56f13d34efaff9a995e189b3d36aeee1485eb2345710f6d48a8f9d5dff83bc66427160ec0341ceb6faf66a10446c529be242cf6f798fcdb9bffc5146501f73dbffb81ba7e05ad1b85f28976686443f8ec94d97c9dc0f28dd854907241757e8ee91335ff5c888e273d7369c6fc99683e8b3d344cf8a5bdc3d6afaaa0cf6fccf00d9306835600f9004ac277c42af1482569",
                                        D =
                                            "2b7a6e5d352315817b8bc46720a0701cfef0a3ad9bc67a288f5b5a2165bc15518d80436bbb73d55f958830ce5df9f4e6415be5d014ee44ddaee8e2faad7c156c1e7c1509b050d06398a4a6fe715f765568e218e632edfc20e98d014bb8f20efda919042b17691dd4dcaad772a18b5bf9ffb206f2b1021f93881ece17d39f839ab6fcd5fa6b18cf3bfbde8b1586b6fb47642753b697df55496b96021d517e1af06a71c837f244ee69bf8b67ddfc18a7e72d2039b9475514b2b3e2d6c0c8d027cbe7fa61334fc14dc5182406b390320d03c868fffa3b5259b98a862d562b81039557641712be9b398f0aca1527e2a88ff4f7685df27ee44990a125492afe3b6fc9"
                                    }
                                },
                                new()
                                {
                                    Cipher = new CipherTextDao
                                    {
                                        C =
                                            "b551f3dc85ef14e5b5be634fca7b44c2fc1a2389b482a81a0517a9883377e7926c6d79328318a02a0386a9c8244dcc778c8ae9cc1e239a4e4be0bd6d42dd10be2a4ca3eb27029fd50018267e9c150652a48caec4315cdbc516332fe6fe7c6e1549c05fb446a429e1ba5c6e87ad432adc3b506a2b4c2305bb898c1ac61eeb283ded236aed70b5d719308a482a2333799ff75be36237a65241a641caf7056799cfd62ba30a78442e07aadfdce81716a4117a058f3c89b6783c427a3ce0ea06fa427956d51c5a78af2c6b4b8bf016e41fba1c61d6e73b5f123e4db39a1dd4d402616cbdffd7cc505f362c7cef434d8593c67ba58c57ae3cb8e680ae78d11a5ebfeb",
                                        D =
                                            "e29414f8873e625f20637b7a89174e04da9e41ce0a466216601b893420094ddf28615476fe2e9dea1d6273051ec9e656207c8e186707301c2a55504baa2fde958ab33199d66e0eceabb73479f8a32cc8a7bffea8f22f0b2828e6822edf8e00fae770136ba1611688056b13120621cde84be2fcf818de61aca866d33613fdf1ed09f1c955cee5f0bc95838bfa1b5de2ed2cc598a43890e092db76cf70fbad16db01a2b2b6cf241b0170836b2bf31e9457b7524b22c5cd090edec596857d77950d33c89026efbf9cf364e1c9f55d491388447407845583703b2be2ab2221597692031fed327f2d2e4c1606570360c20e08712c0b18d627e4bbfe1c3aa50d29eadd"
                                    }
                                }
                            },
                            ShortCode = "72"
                        }
                    }
                });
            
            IBallotRepository repository = new BallotRepository(storageClient.Object, _mapper);

            // act
            BallotEncryption ballotEncryption = await repository.GetBallotEncryptionAsync("someid");

            // assert
            ICollection<string> keys = ballotEncryption.Encryptions.Keys;
            
            Assert.That(keys.Count, Is.EqualTo(2));
            Assert.True(keys.Contains("50"));
            Assert.True(keys.Contains("72"));
        }
    }
}
