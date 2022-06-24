using AutoMapper;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Model.Paper;


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
                .ForMember(dest => dest.ShortCode2, opt => opt.MapFrom(src => src.ShortCode2));

            CreateMap<PrintOptionDao, PaperBallotOption>()
                .ConstructUsing(x => new PaperBallotOption(x.Name, x.ShortCode1, x.ShortCode2));
        }

        private void CreatePrintBallotMap()
        {
            CreateMap<PaperBallot, PrintBallotDao>()
                .ForMember(dest => dest.BallotId, opt => opt.MapFrom(src => src.BallotId))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<PrintBallotDao, PaperBallot>()
                .ConstructUsing((x, ctx) =>
                {
                    IList<PaperBallotOption> options = ctx.Mapper.Map<IList<PaperBallotOption>>(x.Options);

                    return new PaperBallot(x.BallotId, options);
                });
        }
    }
}
