using AutoMapper;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.ConsensusNode.Domain.Model;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Math;

namespace Helverify.ConsensusNode.Backend.Mapping
{
    public class DecryptionProfile: Profile
    {
        public DecryptionProfile()
        {
            CreateDecryptionShareMapping();
            CreateProofOfDecryptionMapping();
            CreateEncryptedShareMapping();
        }

        private void CreateEncryptedShareMapping()
        {
            CreateMap<EncryptedShareRequestDto, EncryptedShare>()
                .ConstructUsing(dto => CreateEncryptedShare(dto));
        }

        private EncryptedShare CreateEncryptedShare(EncryptedShareRequestDto dto)
        {
            BigInteger c = new BigInteger(dto.Cipher.C, 16);
            BigInteger d = new BigInteger(dto.Cipher.D, 16);

            ElGamalCipher cipher = new ElGamalCipher(c, d, null);

            return new EncryptedShare(cipher);
        }

        private void CreateProofOfDecryptionMapping()
        {
            CreateMap<ProofOfDecryption, ProofOfDecryptionDto>()
                .ForMember(dest => dest.D, opt => opt.MapFrom(src => src.D.ToString(16)))
                .ForMember(dest => dest.U, opt => opt.MapFrom(src => src.U.ToString(16)))
                .ForMember(dest => dest.V, opt => opt.MapFrom(src => src.V.ToString(16)))
                .ForMember(dest => dest.S, opt => opt.MapFrom(src => src.S.ToString(16)));
        }

        private void CreateDecryptionShareMapping()
        {
            CreateMap<DecryptedShare, DecryptionShareDto>()
                .ForMember(dest => dest.DecryptedShare, opt => opt.MapFrom(src => src.Share))
                .ForMember(dest => dest.ProofOfDecryption, opt => opt.MapFrom(src => src.ProofOfDecryption));
        }
    }
}
