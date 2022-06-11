using Helverify.VotingAuthority.Backend.Mapping;
using Helverify.VotingAuthority.Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDomainConfiguration();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ElectionDtoProfile>();
    cfg.AddProfile<RegistrationDtoProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();