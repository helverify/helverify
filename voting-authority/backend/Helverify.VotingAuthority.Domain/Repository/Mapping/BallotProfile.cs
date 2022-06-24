using AutoMapper;
using Helverify.Cryptography.Encryption;
using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Extensions;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Model.Virtual;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Mapping profile for Ballots
    /// </summary>
    internal class BallotProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BallotProfile()
        {
            CreateBallotMap();
            CreateProofOfContainingOneMap();
            CreateSumProofMap();
            CreateVirtualBallotMap();
            CreateEncryptedOptionMap();
            CreateEncryptedOptionValueMap();
            CreateCiphertextMap();
            CreateProofOfZeroOrOneMap();
        }

        private void CreateSumProofMap()
        {
            CreateMap<SumProof, SumProofDao>()
                .ForMember(dest => dest.Cipher, opt => opt.MapFrom(src => src.Cipher))
                .ForMember(dest => dest.ProofOfContainingOne, opt => opt.MapFrom(src => src.Proof));
        }

        private void CreateProofOfZeroOrOneMap()
        {
            CreateMap<ProofOfZeroOrOne, ProofOfZeroOrOneDao>()
                .ForMember(dest => dest.C0, opt => opt.MapFrom(src => src.C0.ConvertToHexString()))
                .ForMember(dest => dest.C1, opt => opt.MapFrom(src => src.C1.ConvertToHexString()))
                .ForMember(dest => dest.R0, opt => opt.MapFrom(src => src.R0.ConvertToHexString()))
                .ForMember(dest => dest.R1, opt => opt.MapFrom(src => src.R1.ConvertToHexString()))
                .ForMember(dest => dest.U0, opt => opt.MapFrom(src => src.U0.ConvertToHexString()))
                .ForMember(dest => dest.U1, opt => opt.MapFrom(src => src.U1.ConvertToHexString()))
                .ForMember(dest => dest.V0, opt => opt.MapFrom(src => src.V0.ConvertToHexString()))
                .ForMember(dest => dest.V1, opt => opt.MapFrom(src => src.V1.ConvertToHexString()));
        }

        private void CreateCiphertextMap()
        {
            CreateMap<ElGamalCipher, CipherTextDto>()
                .ForMember(dest => dest.C, opt => opt.MapFrom(src => src.C.ConvertToHexString()))
                .ForMember(dest => dest.D, opt => opt.MapFrom(src => src.D.ConvertToHexString()));
        }

        private void CreateEncryptedOptionValueMap()
        {
            CreateMap<EncryptedOptionValue, EncryptedOptionValueDao>()
                .ForMember(dest => dest.Cipher, opt => opt.MapFrom(src => src.Cipher))
                .ForMember(dest => dest.ProofOfZeroOrOne, opt => opt.MapFrom(src => src.ProofOfZeroOrOne));
        }

        private void CreateEncryptedOptionMap()
        {
            CreateMap<EncryptedOption, EncryptedOptionDao>()
                .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(src => src.ShortCode))
                .ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.Values));
        }

        private void CreateProofOfContainingOneMap()
        {
            CreateMap<ProofOfContainingOne, ProofOfContainingOneDao>()
                .ForMember(dest => dest.S, opt => opt.MapFrom(src => src.S.ConvertToHexString()))
                .ForMember(dest => dest.U, opt => opt.MapFrom(src => src.U.ConvertToHexString()))
                .ForMember(dest => dest.V, opt => opt.MapFrom(src => src.V.ConvertToHexString()));
        }

        private void CreateVirtualBallotMap()
        {
            CreateMap<VirtualBallot, VirtualBallotDao>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ColumnProofs, opt => opt.MapFrom(src => src.ColumnProofs))
                .ForMember(dest => dest.RowProofs, opt => opt.MapFrom(src => src.RowProofs))
                .ForMember(dest => dest.EncryptedOptions, src => src.MapFrom(src => src.EncryptedOptions));
        }
        

        private void CreateBallotMap()
        {
            CreateMap<PaperBallot, PaperBallotDao>()
                .ForMember(dest => dest.BallotId, opt => opt.MapFrom(src => src.BallotId))
                .ForMember(dest => dest.Ballots, opt => opt.MapFrom(src => src.Ballots));
        }
    }
}
