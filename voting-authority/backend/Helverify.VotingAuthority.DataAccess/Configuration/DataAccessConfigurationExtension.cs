﻿using Helverify.VotingAuthority.DataAccess.Database;
using Helverify.VotingAuthority.DataAccess.Rest;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Helverify.VotingAuthority.DataAccess.Configuration
{
    public static class DataAccessConfigurationExtension
    {
        public static IServiceCollection AddDataAccessConfiguration(this IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("MongoDbConnectionString") ?? throw new InvalidOperationException();
            
            services.AddHttpClient();
            services.AddSingleton<IRestClient, RestClient>();

            services.AddScoped<IMongoClient>(_ => new MongoClient(connectionString));
            services.AddScoped(typeof(IMongoService<>), typeof(MongoService<>));
            
            return services;
        }
    }
}
