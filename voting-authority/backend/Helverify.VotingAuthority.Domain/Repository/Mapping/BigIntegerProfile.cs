using AutoMapper;
using Helverify.VotingAuthority.Domain.Extensions;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Mapping profile for BigInteger
    /// </summary>
    public class BigIntegerProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigIntegerProfile()
        {
            CreateMap<BigInteger, string>()
                .ConvertUsing(bi => bi.ConvertToHexString());

            CreateMap<string, BigInteger>().ConvertUsing(str => str.ConvertToBigInteger());
        }
    }
}
