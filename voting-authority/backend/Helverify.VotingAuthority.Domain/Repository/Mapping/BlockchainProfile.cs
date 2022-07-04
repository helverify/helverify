using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Model.Blockchain;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Mapping profile for Blockchain
    /// </summary>
    public class BlockchainProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BlockchainProfile()
        {
            CreateBlockchainMapping();
        }

        private void CreateBlockchainMapping()
        {
            CreateMap<Blockchain, BlockchainDao>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Registrations, opt => opt.MapFrom(src => src.Registrations))
                .ReverseMap();
        }
    }
}
