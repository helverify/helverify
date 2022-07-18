using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Consensus;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Tests.Model
{
    internal class RegistrationTests
    {

        [Test]
        public void TestSetPublicKey()
        {
            // arrange
            string electionId = "45523fa121";

            string publicKeyValue = "e37039da9a7fefed6812290b704b2a185aed287f85867b3ccbebda28c75cea61ebbc0ee302775df616378300dedb9e52d489f5a7a0bac12d3e5159f0bcd144ab84f7df9eb59c41a679390050ad2ec6fb7f42a2ac2cfdfb4f9ef3368eb858c99e17a4c3f86446f184b27067962920c0653c02288e7eadf2e13ad5a990b2ca70b9781588680c8f7dc2608ec59c7bc66cf4d3890f5d1f40d9df38d0fbc92863236479be67e1008b88caa8b0d8713c2d4d0911bd571321dd8151fbbb301081a5e325e0742d1a7572f502a0f513e563d3282ccf634ffaf28d43f0a9e5ea68889b088863699faabd52cfa41dd3abbec4cd193bf09f97a0dd1bf45b3be5adc91138cda0";
            
            BigInteger pk = new BigInteger(publicKeyValue, 16);
            Registration registration = new Registration();
            
            Election election = new Election
            {
                Id = electionId,
                P = new BigInteger("ffffffffffffffffc90fdaa22168c234c4c6628b80dc1cd129024e088a67cc74020bbea63b139b22514a08798e3404ddef9519b3cd3a431b302b0a6df25f14374fe1356d6d51c245e485b576625e7ec6f44c42e9a637ed6b0bff5cb6f406b7edee386bfb5a899fa5ae9f24117c4b1fe649286651ece45b3dc2007cb8a163bf0598da48361c55d39a69163fa8fd24cf5f83655d23dca3ad961c62f356208552bb9ed529077096966d670c354e4abc9804f1746c08ca18217c32905e462e36ce3be39e772c180e86039b2783a2ec07a28fb5c55df06f4c52c9de2bcbf6955817183995497cea956ae515d2261898fa051015728e5a8aacaa68ffffffffffffffff", 16),
                G = new BigInteger("2", 16)
            };
            
            PublicKeyDto publicKey = new PublicKeyDto
            {
                PublicKey = publicKeyValue,
                ProofOfPrivateKey = new ProofOfPrivateKeyDto
                {
                    C = "a38a7b12c57224c33846c94e12bc21ae70cb68acb640bc3b405fd5ad5fcc5e81",
                    D = "61c391c6a69fcc9427164b60e4389fbea3952af8da993cf71e5be4ed623e1b4f1552a4a0e7b3df234a67b98ba81f2e088dd44b5147842b99fe7fa025f0e2c321057976f18e60fe4929bfc7b436dac9c18b9d12ce364a5d3d1854ec792cba6744641c30e6a2ef84f673dea136fb2206cf261ad92f3446bf1e8a01cd1141ad6271ac579b5b00a6d28301039f1b92021e3482d4dc3f9041aa8a14449e6342b83d4f6a110f9c24f7a78cf3663d238c8495bff50a07c856b6b019631ba3a00bb7a64a9e704e70e418e9af8229675b26e9ff00e6abda5c2cd11192d93262cc7b7e44f353d08aba21bb4745ffe4e8b1aac149e9c222c4be58fbe5ec9f8f23a0b4ddea40",
                }
            };

            // act
            registration.SetPublicKey(publicKey, election);

            // assert
            Assert.That(registration.PublicKeys[electionId], Is.EqualTo(pk));
        }

        [Test]
        public void TestSetPublicKeyInvalidProof()
        {
            // arrange
            string publicKeyValue = "49e5cbd73c0a4a92627b0a659c637e3b5bee84242278d17f3c2dcebfb124c79f7d5f32e7dce3e552fbb69b0bd894a3611a175c1ecb45afcf134cf14e45f54fd9c522301851ad5d1536394dffd882c70633ba5051252fadd43207ecba57ef2b84dfaf9b635072c80b4cde96cc4d8e7322459cb95003692ab301954d0cec13a0075bcfee943acaf74889e4d1d643dd24afbba57322324a0958e934e6b8b2b223bd78e5cd2247788c5b1316a61f924dbd34c7448e2273f1ad27e96595afd63dea6146454f31125fa29f0192bd7dd2255bb374af1784c8943caef2a3e618d894fa83a594c9d9dc9a73db52efdbabea9a05a0fb848c42cbba2cdb1fe9c88c79982d0b";

            string electionId = "a2a92819f1ab";
            BigInteger pk = publicKeyValue.ConvertToBigInteger();

            Registration registration = new Registration
            {
                PublicKeys = new Dictionary<string, BigInteger>
                {
                    {electionId, pk}
                }
            };

            Election election = new Election
            {
                Id = electionId,
                P = new BigInteger("87a8e61db4b6663cffbbd19c651959998ceef608660dd0f25d2ceed4435e3b00e00df8f1d61957d4faf7df4561b2aa3016c3d91134096faa3bf4296d830e9a7c209e0c6497517abd5a8a9d306bcf67ed91f9e6725b4758c022e0b1ef4275bf7b6c5bfc11d45f9088b941f54eb1e59bb8bc39a0bf12307f5c4fdb70c581b23f76b63acae1caa6b7902d52526735488a0ef13c6d9a51bfa4ab3ad8347796524d8ef6a167b5a41825d967e144e5140564251ccacb83e6b486f6b3ca3f7971506026c0b857f689962856ded4010abd0be621c3a3960a54e710c375f26375d7014103a4b54330c198af126116d2276e11715f693877fad7ef09cadb094ae91e1a1597", 16),
                G = new BigInteger("3fb32c9b73134d0b2e77506660edbd484ca7b18f21ef205407f4793a1a0ba12510dbc15077be463fff4fed4aac0bb555be3a6c1b0c6b47b1bc3773bf7e8c6f62901228f8c28cbb18a55ae31341000a650196f931c77a57f2ddf463e5e9ec144b777de62aaab8a8628ac376d282d6ed3864e67982428ebc831d14348f6f2f9193b5045af2767164e1dfc967c1fb3f2e55a4bd1bffe83b9c80d052b985d182ea0adb2a3b7313d3fe14c8484b1e052588b9b7d2bbd2df016199ecd06e1557cd0915b3353bbb64e0ec377fd028370df92b52c7891428cdc67eb6184b523d1db246c32f63078490f00ef8d647d148d47954515e2327cfef98c582664b4c0f6cc41659", 16)
            };

            PublicKeyDto publicKey = new PublicKeyDto
            {
                PublicKey = publicKeyValue,
                ProofOfPrivateKey = new ProofOfPrivateKeyDto
                {
                    C = "79f1c3eb8fd82411fb053d61e12d4a95f2fa3347491b78993e4b23c36cbc94ad",
                    D = "4086da8fcaec3f2a884f9c3aceeabb2967ff733805c35398dbedce55c4314dec0fe4119f665f025a1d9d6152a8ea7c83728503ff9d3dfee11dc29a8d06fb30c553edce9f288362c5cfb374039850d99726d6136d54da9ac0e811958ff4b419554d64c735894c6825bf01000388a437134b90c8858bc9688bd3a1e1530e12b878a17449d8e290b74a8217ebd3eb166d32c4219333b723772412ea06367b90729bad900eb97e5450ff8e4499c77ac552605dc0050b6b1351882bdc6d6e07cd2137c4c029141b9df5d7c4c2bcb7e8fd0f02f5b9265dfe05e90dda546780bfb130052954d05b2bb73d5c4ffe3132868c350947d2a28d20b3f0d40e45c4bbbc031cfb",
                }
            };

            // act, assert
            Assert.Throws<Exception>(() => registration.SetPublicKey(publicKey, election));
        }
    }
}
