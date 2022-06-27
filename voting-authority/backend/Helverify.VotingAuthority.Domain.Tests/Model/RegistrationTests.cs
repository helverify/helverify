using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
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

            string publicKeyValue = "49e5cbd73c0a4a92627b0a659c637e3b5bee84242278d17f3c2dcebfb124c79f7d5f32e7dce3e552fbb69b0bd894a3611a175c1ecb45afcf134cf14e45f54fd9c522301851ad5d1536394dffd882c70633ba5051252fadd43207ecba57ef2b84dfaf9b635072c80b4cde96cc4d8e7322459cb95003692ab301954d0cec13a0075bcfee943acaf74889e4d1d643dd24afbba57322324a0958e934e6b8b2b223bd78e5cd2247788c5b1316a61f924dbd34c7448e2273f1ad27e96595afd63dea6146454f31125fa29f0192bd7dd2255bb374af1784c8943caef2a3e618d894fa83a594c9d9dc9a73db52efdbabea9a05a0fb848c42cbba2cdb1fe9c88c79982d0b";
            
            
            BigInteger pk = new BigInteger(publicKeyValue, 16);
            Registration registration = new Registration();
            
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
                    C = "69f1c3eb8fd82411fb053d61e12d4a95f2fa3347491b78993e4b23c36cbc94ad",
                    D = "3086da8fcaec3f2a884f9c3aceeabb2967ff733805c35398dbedce55c4314dec0fe4119f665f025a1d9d6152a8ea7c83728503ff9d3dfee11dc29a8d06fb30c553edce9f288362c5cfb374039850d99726d6136d54da9ac0e811958ff4b419554d64c735894c6825bf01000388a437134b90c8858bc9688bd3a1e1530e12b878a17449d8e290b74a8217ebd3eb166d32c4219333b723772412ea06367b90729bad900eb97e5450ff8e4499c77ac552605dc0050b6b1351882bdc6d6e07cd2137c4c029141b9df5d7c4c2bcb7e8fd0f02f5b9265dfe05e90dda546780bfb130052954d05b2bb73d5c4ffe3132868c350947d2a28d20b3f0d40e45c4bbbc031cfb",
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
