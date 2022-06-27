using AutoMapper;
using Helverify.VotingAuthority.Domain.Extensions;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    public class BigIntegerProfile: Profile
    {
        public BigIntegerProfile()
        {
            CreateMap<BigInteger, string>()
                .ConvertUsing(bi => bi.ConvertToHexString());

            CreateMap<string, BigInteger>().ConvertUsing(str => str.ConvertToBigInteger());

            //CreateMap<KeyValuePair<string, BigInteger>, KeyValuePair<string, string>>()
            //    .ConstructUsing(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ConvertToHexString()));

            //CreateMap<KeyValuePair<string, string>, KeyValuePair<string, BigInteger>>()
            //    .ConstructUsing(kv => new KeyValuePair<string, BigInteger>(kv.Key, kv.Value.ConvertToBigInteger()));
        }
    }
}
