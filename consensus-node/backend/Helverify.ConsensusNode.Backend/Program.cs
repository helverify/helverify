using Helverify.ConsensusNode.Backend.Mapping;
using Helverify.ConsensusNode.Domain.Configuration;
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

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
