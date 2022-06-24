using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Mapping profile for Elections
    /// </summary>
    internal class ElectionProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ElectionProfile()
        {
            CreateElectionMapping();
        }

        private void CreateElectionMapping()
        {
            CreateMap<ElectionDao, Election>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.P, opt => opt.MapFrom(src => src.P.ConvertToBigInteger()))
                .ForMember(dest => dest.G, opt => opt.MapFrom(src => src.G.ConvertToBigInteger()))
                .ForMember(dest => dest.ContractAddress, opt => opt.MapFrom(src => src.ContractAddress))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ConvertToBigInteger()));

            CreateMap<Election, ElectionDao>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.P, opt => opt.MapFrom(src => src.P.ConvertToHexString()))
                .ForMember(dest => dest.G, opt => opt.MapFrom(src => src.G.ConvertToHexString()))
                .ForMember(dest => dest.ContractAddress, opt => opt.MapFrom(src => src.ContractAddress))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ConvertToHexString()));

            CreateMap<ElectionOptionDao, ElectionOption>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
