using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model.Blockchain;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    internal class GenesisProfile: Profile
    {
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
