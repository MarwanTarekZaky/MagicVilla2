// using Serilog;

using System.Collections.Immutable;
using MagicVilla_VillaAPI;
using MagicVilla2_API.Data;
using MagicVilla2_API.Repository;
using MagicVilla2_API.Repository.IRepostiory;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo
//     .File("log/villaLogs.txt",rollingInterval: RollingInterval.Day).CreateLogger();
// builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddControllers(
    option =>
    {
        // option.ReturnHttpNotAcceptable = true;
    }).AddNewtonsoftJson();
builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();