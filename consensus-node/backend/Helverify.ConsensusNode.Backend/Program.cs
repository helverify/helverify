using Helverify.ConsensusNode.Backend.Mapping;
using Helverify.ConsensusNode.Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
