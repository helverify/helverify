using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Ethereum.Contract;
using Helverify.VotingAuthority.Domain.Model;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Mapping profile for election results
    /// </summary>
    internal class ResultsProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResultsProfile()
        {
            CreateResultsMap();
        }

        private void CreateResultsMap()
        {
            CreateMap<Result, ElectionResult>()
                .ForMember(dest => dest.OptionName, opt => opt.MapFrom(src => src.Option))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Tally))
                .ReverseMap();
        }
    }
}
