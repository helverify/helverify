using System.Runtime.CompilerServices;
using Helverify.VotingAuthority.DataAccess.Configuration;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Blockchain;
using Helverify.VotingAuthority.Domain.Model.Paper;
using Helverify.VotingAuthority.Domain.Repository;
using Helverify.VotingAuthority.Domain.Repository.Mapping;
using Helverify.VotingAuthority.Domain.Repository.Mapping.Converter;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Helverify.VotingAuthority.Domain.Tests")]
namespace Helverify.VotingAuthority.Domain.Configuration
{
    /// <summary>
    /// Configuration extension for the Domain layer.
    /// </summary>
    public static class DomainConfigurationExtension
    {
        /// <summary>
        /// Registers all services exposed by the Domain layer.
        /// </summary>
        /// <param name="services">ServiceCollection for DI</param>
        /// <returns></returns>
        public static IServiceCollection AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddTransient<GenesisConverter>();
            services.AddTransient<OptionShareConverter>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<BigIntegerProfile>();
                cfg.AddProfile<ElectionProfile>();
                cfg.AddProfile<RegistrationProfile>();
                cfg.AddProfile<GenesisProfile>();
                cfg.AddProfile<BallotProfile>();
                cfg.AddProfile<PrintBallotProfile>();
                cfg.AddProfile<BlockchainProfile>();
                cfg.AddProfile<OptionShareProfile>();
            });
            services.AddDataAccessConfiguration();
            services.AddSingleton<IConsensusNodeService, ConsensusNodeService>();
            services.AddSingleton<ICliRunner, CliRunner>();
            services.AddScoped<IRepository<Election>, GenericRepository<Election, ElectionDao>>();
            services.AddScoped<IRepository<Blockchain>, GenericRepository<Blockchain, BlockchainDao>>();
            services.AddScoped<IRepository<PaperBallot>, PaperBallotRepository>();
            services.AddScoped<IPublishedBallotRepository, PublishedBallotRepository>();
            services.AddScoped<IBlockchainSetup, BlockchainSetup>();
            services.AddScoped<IElectionContractRepository, ElectionContractRepository>();
            services.AddScoped<IBallotPdfService, BallotPdfService>();
            services.AddScoped<IZipFileService, ZipFileService>();

            return services;
        }
    }
}
