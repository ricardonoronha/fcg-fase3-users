using FIAP.MicroService.Usuario.Dominio.Interfaces;
using FIAP.MicroService.Usuario.Infraestrutura.Data;
using FIAP.MicroService.Usuario.Infraestrutura.Repositories;
using FIAP.MicroService.Usuario.Infraestrutura.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Datadog.Trace.Configuration;
using Datadog.Trace;
using Serilog;
using Serilog.Events;


var settings = TracerSettings.FromDefaultSources();
Tracer.Configure(settings);

var defaultLogger = new LoggerConfiguration()
   .MinimumLevel.Information()
   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
   .Enrich.FromLogContext()
   .Enrich.WithEnvironmentName()
   .Enrich.WithMachineName()
   .Enrich.WithProcessId()
   .Enrich.WithThreadId()
   .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter(renderMessage: true))
   .CreateLogger();

Log.Logger = defaultLogger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();

#region "DbContext"

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connectionString));

#endregion

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FIAP.MicroService.Usuario.API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

