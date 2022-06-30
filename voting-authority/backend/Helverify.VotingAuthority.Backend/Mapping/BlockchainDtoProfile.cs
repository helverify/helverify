using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Backend.Mapping
{
    public class BlockchainDtoProfile: Profile
    {
        public BlockchainDtoProfile()
        {
            CreateBlockchainDtoProfile();
        }

        private void CreateBlockchainDtoProfile()
        {
            CreateMap<BlockchainDto, Blockchain>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Registrations, opt => opt.MapFrom(src => src.Registrations))
                .ReverseMap();
        }
    }
}
