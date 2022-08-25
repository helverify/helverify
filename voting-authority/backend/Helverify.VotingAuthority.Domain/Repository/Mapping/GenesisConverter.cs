using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.Domain.Model.Blockchain;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Converter for Genesis mapping
    /// </summary>
    internal class GenesisConverter : ITypeConverter<Genesis, GenesisDto>
    {
        private const string ExtradataPrefix = "0x0000000000000000000000000000000000000000000000000000000000000000";
        private const string ExtradataSuffix = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

        /// <summary>
        /// Converts a Genesis instance to a GenesisDto instance
        /// </summary>
        /// <param name="source">Genesis instance</param>
        /// <param name="destination">GenesisDto instance</param>
        /// <param name="context">Automapper context</param>
        /// <returns></returns>
        public GenesisDto Convert(Genesis source, GenesisDto destination, ResolutionContext context)
        {
            destination = new GenesisDto();

            IList<string> authorityAddresses = source.Authorities.Select(r => r.Address).ToList();

            destination.Extradata = ExtradataPrefix + string.Join(string.Empty, authorityAddresses) + ExtradataSuffix;

            foreach (Account prefundedAccount in source.PrefundedAccounts)
            {
                destination.Alloc.Add(prefundedAccount.Address, new AccountBalanceDto { Balance = prefundedAccount.Funds });
            }

            destination.Config = new ConfigDto
            {
                ChainId = source.ChainId,
                Clique = new CliqueDto
                {
                    Epoch = source.CliqueEpoch,
                    Period = source.CliquePeriod,
                },
                ByzantiumBlock = 0,
                ConstantinopleBlock = 0,
                Eip150Block = 0,
                Eip155Block = 0,
                Eip158Block = 0,
                HomesteadBlock = 0,
                PetersburgBlock = 0
            };
            destination.GasLimit = "180000000";
            
            destination.Difficulty = "1";

            return destination;
        }
    }
}

