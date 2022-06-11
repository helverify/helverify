using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Backend.Mapping
{
    public class RegistrationDtoProfile: Profile
    {
        public RegistrationDtoProfile()
        {
            CreateRegistrationDtoMapping();
        }

        private void CreateRegistrationDtoMapping()
        {
            CreateMap<Registration, RegistrationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.ElectionId))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ExportToHexString()));

            CreateMap<RegistrationDto, Registration>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.ElectionId))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ExportToBigInteger()));
        }
    }
}
