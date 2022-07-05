using AutoMapper;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model.Decryption;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    public class DecryptionProfile: Profile
    {
        public DecryptionProfile()
        {
            CreateDecryptedShareMap();
        }

        private void CreateDecryptedShareMap()
        {
            CreateMap<DecryptedValue, DecryptedValueDao>()
                .ForMember(dest => dest.PlainText, opt => opt.MapFrom(src => src.PlainText))
                .ForMember(dest => dest.CipherText, opt => opt.MapFrom(src => src.CipherText))
                .ForMember(dest => dest.Shares, opt => opt.MapFrom(src => src.Shares));

            CreateMap<DecryptedShare, DecryptedShareDao>()
                .ForMember(dest => dest.Share, opt => opt.MapFrom(src => src.Share))
                .ForMember(dest => dest.ProofOfDecryption, opt => opt.MapFrom(src => src.ProofOfDecryption))
                .ForMember(dest => dest.PublicKeyShare, opt => opt.MapFrom(src => src.PublicKeyShare));

            CreateMap<ProofOfDecryption, ProofOfDecryptionDto>()
                .ForMember(dest => dest.D, opt => opt.MapFrom(src => src.D))
                .ForMember(dest => dest.U, opt => opt.MapFrom(src => src.U))
                .ForMember(dest => dest.V, opt => opt.MapFrom(src => src.V))
                .ForMember(dest => dest.S, opt => opt.MapFrom(src => src.S));
        }
    }
}
