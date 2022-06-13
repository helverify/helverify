using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    public class RegistrationProfile: Profile
    {
        public RegistrationProfile()
        {
            CreateRegistrationMapping();
        }

        private void CreateRegistrationMapping()
        {
            CreateMap<RegistrationDao, Registration>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.ElectionId))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ConvertToBigInteger()));

            CreateMap<Registration, RegistrationDao>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.ElectionId))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ConvertToHexString()));
        }
    }
}
