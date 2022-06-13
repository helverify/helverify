using AutoMapper;
using Helverify.ConsensusNode.Backend.Dto;
using Helverify.Cryptography.ZeroKnowledge;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Helverify.ConsensusNode.Backend.Mapping
{
    /// <summary>
    /// Automapper mapping profile for public key dto.
    /// </summary>
    public class KeyPairProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public KeyPairProfile()
        {
            CreatePublicKeyMapping();
            CreateProofMapping();
        }
        private void CreateProofMapping()
        {
            CreateMap<ProofOfPrivateKeyOwnership, ProofOfPrivateKeyDto>()
                .ForMember(dest => dest.C, opt => opt.MapFrom(src => src.C.ToString(16)))
                .ForMember(dest => dest.D, opt => opt.MapFrom(src => src.D.ToString(16)));
        }

        /// <summary>
        /// Tuple trick from https://stackoverflow.com/questions/21413273/automapper-convert-from-multiple-sources
        /// </summary>
        private void CreatePublicKeyMapping()
        {
            CreateMap<(AsymmetricKeyParameter publicKey, ProofOfPrivateKeyOwnership proof), PublicKeyDto>()
                .ForMember(dest => dest.ProofOfPrivateKey, opt => opt.MapFrom(src => src.proof))
                .ForMember(dest => dest.PublicKey, opt => opt.MapFrom(src => (src.publicKey as DHPublicKeyParameters).Y.ToString(16)));
        }
    }
}
