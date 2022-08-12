using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Backend.Mapping
{
    /// <summary>
    /// Election statistics AutoMapper profile
    /// </summary>
    public class ElectionStatisticsProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ElectionStatisticsProfile()
        {
            CreateMapping();
        }

        private void CreateMapping()
        {
            CreateMap<ElectionNumbers, ElectionStatisticsDto>()
                .ForMember(dest => dest.NumberOfBallotsCast, opt => opt.MapFrom(src => src.NumberOfBallotsCast))
                .ForMember(dest => dest.NumberOfBallotsTotal, opt => opt.MapFrom(src => src.NumberOfBallotsTotal));
        }
    }
}
