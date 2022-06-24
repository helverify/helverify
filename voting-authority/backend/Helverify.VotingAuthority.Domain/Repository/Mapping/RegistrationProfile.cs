using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Mapping profile for Registrations
    /// </summary>
    internal class RegistrationProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RegistrationProfile()
        {
            CreateRegistrationMapping();
        }

        private void CreateRegistrationMapping()
        {
            CreateMap<RegistrationDao, Registration>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.ElectionId))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => new Account(src.AccountAddress, "0") ))
                .ForMember(dest => dest.Enode, opt => opt.MapFrom(src => src.Enode))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ConvertToBigInteger()));

            CreateMap<Registration, RegistrationDao>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.ElectionId))
                .ForMember(dest => dest.AccountAddress, opt => opt.MapFrom(src => src.Account.Address))
                .ForMember(dest => dest.Enode, opt => opt.MapFrom(src => src.Enode))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => src.PublicKey.ConvertToHexString()));
        }
    }
}
