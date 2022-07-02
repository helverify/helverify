﻿using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model.Decryption;
using Helverify.VotingAuthority.Domain.Repository.Mapping.Converter;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    internal class OptionShareProfile: Profile
    {
        public OptionShareProfile()
        {
            CreateOptionShareMap();
        }

        private void CreateOptionShareMap()
        {
            CreateMap<DecryptedBallotShareDto, IList<OptionShare>>()
                .ConvertUsing<OptionShareConverter>();
        }
    }
}
