using Serilog;
using Datadog.Trace;
using Serilog.Events;
using RabbitMQ.Client;
using Microsoft.OpenApi.Models;
using Datadog.Trace.Configuration;
using Microsoft.EntityFrameworkCore;
using FIAP.MicroService.Usuario.Dominio.Interfaces;
using FIAP.MicroService.Usuario.Infraestrutura.Data;
using FIAP.MicroService.Usuario.Infraestrutura.Repositories;
using FIAP.MicroService.Usuario.Infraestrutura.Services;

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

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<RabbitMQService>();

builder.Services.AddAutoMapper(typeof(Program));

#region RabbitMQ

var rabbitMQ = builder.Configuration.GetSection("RabbitMQConfigurations");

builder.Services.AddSingleton<IConnection>(t =>
{
    var factory = new ConnectionFactory()
    {
        HostName = rabbitMQ["HostName"],
        UserName = rabbitMQ["UserName"],
        Password = rabbitMQ["Password"],
        ConsumerDispatchConcurrency = 1,
    };

    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});

#endregion

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

