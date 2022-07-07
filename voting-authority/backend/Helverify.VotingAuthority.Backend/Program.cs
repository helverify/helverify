using System.IO.Abstractions;
using Helverify.VotingAuthority.Application.Configuration;
using Helverify.VotingAuthority.Backend.Mapping;
using Helverify.VotingAuthority.Domain.Service;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Voting Authority API",
    });

    opt.IncludeXmlComments("Helverify.VotingAuthority.Backend.xml");
});
builder.Services.AddApplicationConfiguration();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ElectionDtoProfile>();
    cfg.AddProfile<RegistrationDtoProfile>();
    cfg.AddProfile<BlockchainDtoProfile>();
    cfg.AddProfile<BalloDtoProfile>();
    cfg.AddProfile<ResultsDtoProfile>();
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("DEV", policy => policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

ICliRunner cliRunner = app.Services.GetService<ICliRunner>() ?? throw new InvalidOperationException();
IFileSystem fileSystem = app.Services.GetService<IFileSystem>() ?? throw new InvalidOperationException();

// start RPC endpoint in case chain has already been configured (restart)
if(fileSystem.File.Exists("/home/eth/data/geth.ipc")){
    cliRunner.Execute("/app/scripts/start-rpc.sh", "");
}



// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("DEV");

app.Run();