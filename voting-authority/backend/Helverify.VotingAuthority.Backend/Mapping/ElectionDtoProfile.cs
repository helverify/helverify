using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Backend.Mapping
{
    public class ElectionDtoProfile: Profile
    {
        public ElectionDtoProfile()
        {
            CreateElectionMapping();
        }

        private void CreateElectionMapping()
        {
            CreateMap<Election, ElectionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.P, opt => opt.MapFrom(src => src.P.ExportToHexString()))
                .ForMember(dest => dest.G, opt => opt.MapFrom(src => src.G.ExportToHexString()))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ExportToHexString()));

            CreateMap<ElectionDto, Election>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.P, opt => opt.MapFrom(src => src.P.ExportToBigInteger()))
                .ForMember(dest => dest.G, opt => opt.MapFrom(src => src.G.ExportToBigInteger()))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ExportToBigInteger()));

            CreateMap<ElectionOptionDto, ElectionOption>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
