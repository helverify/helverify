using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Backend.Mapping
{
    /// <summary>
    /// Mapping profile for ElectionResultsDto
    /// </summary>
    public class ResultsDtoProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResultsDtoProfile()
        {
            CreateResultsDtoMap();
        }

        private void CreateResultsDtoMap()
        {
            CreateMap<ElectionResults, ElectionResultsDto>()
                .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Results));

            CreateMap<ElectionResult, ElectionResultDto>()
                .ForMember(dest => dest.OptionName, opt => opt.MapFrom(src => src.OptionName))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));
        }
    }
}
