using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model.Blockchain;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Mapping profile for Genesis
    /// </summary>
    internal class GenesisProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenesisProfile()
        {
            CreateGenesisDtoMap();
        }

        private void CreateGenesisDtoMap()
        {
            CreateMap<Genesis, GenesisDto>()
                .ConvertUsing<GenesisConverter>();
        }
    }
}
