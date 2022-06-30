using System.IO.Abstractions;
using Helverify.ConsensusNode.Backend.Mapping;
using Helverify.ConsensusNode.Domain.Configuration;
using Helverify.ConsensusNode.Domain.Model;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Consensus Node API",
    });

    opt.IncludeXmlComments("Helverify.ConsensusNode.Backend.xml");
});
builder.Services.AddAutoMapper( cfg =>
{
    cfg.AddProfile<KeyPairProfile>();
    cfg.AddProfile<DecryptionProfile>();
});

builder.Services.AddDomainConfiguration();

var app = builder.Build();

ICliRunner cliRunner = app.Services.GetService<ICliRunner>() ?? throw new InvalidOperationException();
IFileSystem fileSystem = app.Services.GetService<IFileSystem>() ?? throw new InvalidOperationException();

// start RPC endpoint in case chain has already been configured (restart)
if (fileSystem.File.Exists("/home/eth/data/geth.ipc"))
{
    cliRunner.Execute("/app/scripts/init-genesis.sh", "");
    cliRunner.Execute("/app/scripts/start-consensusnode.sh", "");
    cliRunner.Execute("/app/scripts/start-mining.sh", "");
}

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
