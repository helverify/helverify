using AutoMapper;
using Helverify.ConsensusNode.DataAccess.Dao;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.Cryptography.Encryption;
using Org.BouncyCastle.Math;

namespace Helverify.ConsensusNode.Domain.Mapping
{
    /// <summary>
    /// Mapping profile for encrypted ballots
    /// </summary>
    internal class BallotEncryptionProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BallotEncryptionProfile()
        {
            CreateCiphertextMapping();
        }

        private void CreateCiphertextMapping()
        {
            CreateMap<CipherTextDao, CipherText>()
                .ConstructUsing((x, _) =>
                {
                    BigInteger c = new BigInteger(x.C, 16);
                    BigInteger d = new BigInteger(x.D, 16);

                    ElGamalCipher elGamalCipher = new ElGamalCipher(c, d, null);

                    return new CipherText(elGamalCipher);
                });
        }
    }
}
