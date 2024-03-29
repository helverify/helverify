﻿using AutoMapper;
using Helverify.VotingAuthority.Backend.Dto;
using Helverify.VotingAuthority.Domain.Model.Paper;

namespace Helverify.VotingAuthority.Backend.Mapping
{
    /// <summary>
    /// Mapping profile for Ballots
    /// </summary>
    public class BalloDtoProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BalloDtoProfile()
        {
            CreateBallotMap();
        }

        private void CreateBallotMap()
        {
            CreateMap<PaperBallot, PrintBallotDto>()
                .ForMember(dest => dest.BallotId, opt => opt.MapFrom(src => src.BallotId))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<PaperBallotOption, PrintOptionDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ShortCode1, opt => opt.MapFrom(src => src.ShortCode1))
                .ForMember(dest => dest.ShortCode2, opt => opt.MapFrom(src => src.ShortCode2));
        }
    }
}
