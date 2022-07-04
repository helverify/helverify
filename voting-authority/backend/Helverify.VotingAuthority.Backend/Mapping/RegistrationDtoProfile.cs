using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Backend.Mapping
{
    /// <summary>
    /// Mapping profile for RegistrationDto.
    /// </summary>
    public class RegistrationDtoProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RegistrationDtoProfile()
        {
            CreateRegistrationDtoMapping();
        }

        private void CreateRegistrationDtoMapping()
        {
            CreateMap<Registration, RegistrationDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
                .ForMember(dest => dest.PublicKeys, opt => opt.MapFrom(src => src.PublicKeys));

            CreateMap<RegistrationDto, Registration>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
                .ForMember(dest => dest.PublicKeys, opt => opt.MapFrom(src => src.PublicKeys));
        }
    }
}
