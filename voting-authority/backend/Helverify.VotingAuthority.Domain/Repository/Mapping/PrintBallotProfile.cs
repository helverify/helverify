using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Org.BouncyCastle.Math;


namespace Helverify.VotingAuthority.Domain.Repository.Mapping
{
    /// <summary>
    /// Mapping profile for PrintBallots
    /// </summary>
    internal class PrintBallotProfile: Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrintBallotProfile()
        {
            CreatePrintBallotMap();
            CreateOptionsMap();
        }

        private void CreateOptionsMap()
        {
            CreateMap<PaperBallotOption, PrintOptionDao>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ShortCode1, opt => opt.MapFrom(src => src.ShortCode1))
                .ForMember(dest => dest.ShortCode2, opt => opt.MapFrom(src => src.ShortCode2))
                .ForMember(dest => dest.Randomness1, opt => opt.MapFrom(src => src.RandomValues1))
                .ForMember(dest => dest.Randomness2, opt => opt.MapFrom(src => src.RandomValues2));
                
            CreateMap<PrintOptionDao, PaperBallotOption>()
                .ConstructUsing((x, ctx) =>
                {
                    IList<BigInteger> randomness1 = ctx.Mapper.Map<IList<BigInteger>>(x.Randomness1);
                    IList<BigInteger> randomness2 = ctx.Mapper.Map<IList<BigInteger>>(x.Randomness2);

                    return new PaperBallotOption(x.Name, x.ShortCode1, x.ShortCode2, randomness1, randomness2);
                });
        }

        private void CreatePrintBallotMap()
        {
            CreateMap<PaperBallot, PrintBallotDao>()
                .ForMember(dest => dest.BallotId, opt => opt.MapFrom(src => src.BallotId))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.Election.Id))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.Printed, opt => opt.MapFrom(src => src.Printed))
		.ForMember(dest => dest.Casted, opt => opt.MapFrom(src => src.Casted)); 

            CreateMap<PrintBallotDao, PaperBallot>()
                .ConstructUsing((x, ctx) =>
                {
                    IList<PaperBallotOption> options = ctx.Mapper.Map<IList<PaperBallotOption>>(x.Options);

                    PaperBallot pb = new PaperBallot(x.BallotId, options)
                    {
                        Printed = x.Printed,
                        Casted = x.Casted
                    };

                    return pb;
                });
        }
    }
}
