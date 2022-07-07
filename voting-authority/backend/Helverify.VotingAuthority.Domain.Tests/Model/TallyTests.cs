using Helverify.Cryptography.Encryption;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Virtual;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Tests.Model
{
    public class TallyTests
    {
        [Test]
        public void TestCalculateCipherResult()
        {
            // arrange
            Election election = new Election
            {
                P = new BigInteger("ffffffffffffffffc90fdaa22168c234c4c6628b80dc1cd129024e088a67cc74020bbea63b139b22514a08798e3404ddef9519b3cd3a431b302b0a6df25f14374fe1356d6d51c245e485b576625e7ec6f44c42e9a637ed6b0bff5cb6f406b7edee386bfb5a899fa5ae9f24117c4b1fe649286651ece45b3dc2007cb8a163bf0598da48361c55d39a69163fa8fd24cf5f83655d23dca3ad961c62f356208552bb9ed529077096966d670c354e4abc9804f1746c08ca18217c32905e462e36ce3be39e772c180e86039b2783a2ec07a28fb5c55df06f4c52c9de2bcbf6955817183995497cea956ae515d2261898fa051015728e5a8aacaa68ffffffffffffffff", 16)
            };

            ElGamalCipher c1 = new ElGamalCipher(
                new BigInteger(
                    "20a5c88829f017f02150bfafe96c46b58f54e1e35f86adc43224dce58083bcb77e24f0184343e970cf23dac526f8b60267a7730d38591714febcd0716b2f510d35007699ba7be553530e5355d0a63438bc44a3cca89b59a14a6f893d48e4bfa093db70da8378135d9e73ab2aa9e55b13699e5a16d0edd215e1615cad83ee2761d358439708659e23393d12a0d7df5be00d1024691953dd2cd46a5ecbf3d5031e7c3516dedc02fff95a97582355fd6d59fb479272bfbc3482122a5ebedf94329b81126ebfd7f814ba8d8291159e198de2e058d3a2b559b5a4c640ad7896f99f776c8d66a6f63bfd5595ff013fe3959029ecb13ec6421b60db3fd072a7fa93f21d",
                    16),
                new BigInteger(
                    "53c0941851b321b5ce4a301d857cfab1054a0016e7aad7eb7ea8cc17e70be8479bd0f6e9b937d7c9fa1e1313af26b75f8d608f02c896655c5839a2c1d557e4541b31090d35e8b9e237f2f792ec6de83939814806c899e5d7d3394913c0ba156a3c034e3d24ca7f314096e42def753e8581fb977d23ee85f88776685c5ac95f4c171d51143c37ba8782cdfa77dec2e398090651ee9ef61f54fe25227a4227064679cc6e3355eb348e490dae633bda3d39377af42749a9da351ccddfb18e8ee34294f3258d089db08435aefb51a67ad373e13fcd175b877de77b9307a90fde6fff5f5def36289480a54cc415899a0795bd4cf9b196245c1680247be76ef15f388d",
                    16),
                null);

            ElGamalCipher c2 = new ElGamalCipher(
                new BigInteger(
                    "35b04e6cc3c98431e9ddd507fe9b49ba787f2e83b497a3ca3e39c3f8e65ad8e744d2218fda3af779e7479026ec6e6083beaa7c4b42cb92d32bf81ab2834949d0ee553ca58e2b1372ee1a50d49a1c0c675e9bf6b3b56f5b71240627c22f0f2b050e07f1143b291f06cd8f7e3f3672aed4571d99aecfbbc1112446fe037c48edf87d07b332e5cc10780f7bf41006e8bc5f8cec8cf5ce1e6e69506d50099d01368014014c40c36b617d1d02eee415b85466ff3e5fc110b52e98bf70e215c976500c7b4d9813d7c4185d186f27ef5bb030191f3b9277492f6296e55e9a27fe42aa43b73dd3087f5874075d9abefbe13254d400112d78e39c55a3fe1bcace8c114cba",
                    16),
                new BigInteger(
                    "c2533050c87398d702fd0570c2fc369492a5ffb2b2eb68473f2211437f47acbcab79ca107d9bc84669d6cb736968ccfdeaf3d442a348ac418e4a4a6f7428190fa63e049d33cb1be5c7ed81edce173f8dc5d444d7fefe4aaef7f3a09d07c643bc8d130a8e73a0826cb1c96885348ace57493a842748e819070c625d482252964bf190ee6ce34447f3ed103c2050b8c2bb6c2ca89ffe4c915a42b3b1c7f30ccd159220bb55f53bca2f7de2bb4d7ba2b5af9b5372944829f3d66f462780913a5f98cab25d228bf5c13e37106b228262de210e97dc329b7510b2c311e4c3399abd205a4baf9d3921ecf5e2f095f39abda7c097a9e02f944213f88f010e978039b95",
                    16),
                null);

            ElGamalCipher c3 = new ElGamalCipher(
                new BigInteger(
                    "d2f0ab76e4b9c7994efde95c67a0a473446597eea8fdc0db5b087c97185c964b40cc8187f159d7a9d4beacb6e363942febec9556fec66e086af6cb3510e9ced460f1c46a35be14f4a7374db5edc29518cfd421ec78bf090b5e9ecfdf5d2c20e5b54456520b41cb26bb8cfa62dd0ea1dfa7fde68f53cd077b96b3149a202776c91d2fbb9738277a2d560b693b0987bf24de64d36d918dfe60ed1404a3e14722638b6eba0e871bef098a298b25f599aee8dc80db498d61bae48628b227de82a7d60858b43bb11181eb359419112b88e5decbbe585bf4d79c7d66b32a61932a0827343247208d9f8d330f1762cb06efada3a5338af2d7959e65f61b036d18a3a941",
                    16),
                new BigInteger(
                    "8bd455fada4dff8b098a221860d6cd1c6e8dde732d17ec38554408eab1900682b354daf27ffdae865c6442339f586a4141ba2a9ee8abc6a1807be84047d98d729cfb5aa1a1fb30b9e51cbbed3aa7c589100f5ad2453d8b8e5d0d86c356fff7601fe0e956315f7a2fb044ffad86fc5df15f0ea74b9db0f3e8f6ee8c879a95b43eb8bbc9a9d943e19e948bf610139567372e36ad1ca694db8bd118e54abeb764a171da24670bdd2c418a74129ec01230ac954c2fef52f3980c12f1b46d2addcbd3d7246c80522030fbcfb3c9efb60b0b51baae3f93fbc445b5fdb85eb47c4e1190771ad6e3c14bc7e45290889708d1027fe71a9ff486b588d9fdb463f9b1d88a3c",
                    16),
                null);

            ElGamalCipher c4 = new ElGamalCipher(
                new BigInteger(
                    "6a98ea927947b349727f719c36cae0b79e19b5f8dbd899d101cd68fbbaf61f96d3211563aa1b3e0205545f6d668dd1cf8635273f50ff2bf3d0ef931061bf95482602f9780be7c1fb3df4c6826330fbffe208ba9776ad503229ca5da79b2f62be7e4a11cf757b8b0730f8c93b738e26e3c227053043c72d348b6a3357ce3ce92be4700d32219ebdeb5a4f1d6e3a7e1723056a9ac5578d34a2c6220b7fbe0d7a94db94d768b956f9ca5b6fb11e776705777d0ef0984a12b6692405bcfb134c5084536df6d573f8783f29b77c7eca481848c13155e51aea0b8721e9ba3f4d4956c62462ded258cc999d94f4d05f8a957165ca532a82c9cf7a981708870966c41be4",
                    16),
                new BigInteger(
                    "438bc06627e6e574023a37e8b3e40f5f5b9e77db87301fd47a475f9016c40ed8f0279a44e6e501cdbbe77bb5aa8d2fb16035b6dc9d9759ccdae6c9fc13ea47dc461f46c62239dbb2319c0cc3596ce169b666e31291c8f384f32e3f77ecf7c6827d640d303cc6b5af27fd11cce74680e0f53ce82d9d2c46e34452e3678c131935cca6ae22b750cba976f6e4abbc4dcd7b3f2c18e0e7d73ca020c6bbe21ca7b9b73dee41eb0817fd922c75305727f7b4db5ee9f35911615616477b6392a1f99c36ae551b68d32dbcc9daef24cd673474894a5fe531572703f89a214c30fceabae57f9bd3ff9c0d9ecf50547ed172ca9b2c6e42d2ec6fc7aa1d2f21d817e3ddcf8d",
                    16),
                null);

            IList<EncryptedOption> encryptedOptions = new List<EncryptedOption>
            {
                new()
                {
                    ShortCode = "96",
                    Values =
                    {
                        new EncryptedOptionValue
                        {
                            Cipher = c1
                        },
                        new EncryptedOptionValue
                        {
                            Cipher = c2
                        }
                    }
                },
                new()
                {
                    ShortCode = "bd",
                    Values =
                    {
                        new EncryptedOptionValue
                        {
                            Cipher = c3
                        },
                        new EncryptedOptionValue
                        {
                            Cipher = c4
                        }
                    }
                }
            };

            Tally tally = new Tally(encryptedOptions);

            // act
            IList<ElGamalCipher> ciphers = tally.CalculateCipherResult(election);

            // assert
            ElGamalCipher reference1 = c1.Add(c3, election.P);
            ElGamalCipher reference2 = c2.Add(c4, election.P);

            Assert.That(ciphers.Count, Is.EqualTo(2));
            Assert.That(ciphers[0].C, Is.EqualTo(reference1.C));
            Assert.That(ciphers[0].D, Is.EqualTo(reference1.D));
            Assert.That(ciphers[1].C, Is.EqualTo(reference2.C));
            Assert.That(ciphers[1].D, Is.EqualTo(reference2.D));
        }
    }
}
