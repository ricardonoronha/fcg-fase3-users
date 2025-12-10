using FIAP.MicroService.Usuario.Dominio.Interfaces;
using FIAP.MicroService.Usuario.Infraestrutura.Data;
using FIAP.MicroService.Usuario.Infraestrutura.Repositories;
using FIAP.MicroService.Usuario.Infraestrutura.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTRO DE SERVIÇOS (Injeção de Dependência) ---

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada no appsettings.json ou está vazia.");
}

// CORREÇÃO 1: Usando UseNpgsql para PostgreSQL
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString));

// CORREÇÃO 2: Registrando a implementação correta do repositório
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Registro do serviço de token
builder.Services.AddScoped<ITokenService, TokenService>();

// Registro do AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FIAP.MicroService.Usuario.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] {}
        }
    });
});

// Configuração da Autenticação JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(bearer =>
{
    bearer.RequireHttpsMetadata = false;
    bearer.SaveToken = true;
    bearer.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
});

#region Health Checks

// ------------------------------
// Configuração de Health Checks para Kubernetes
// ------------------------------

builder.Services.AddHealthChecks()
    // Verifica conexão com PostgreSQL
    .AddNpgSql(
        connectionString: connectionString,
        name: "postgresql",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "postgresql", "ready" })
    // Verifica se a aplicação está respondendo (liveness básico)
    .AddCheck("self", () => HealthCheckResult.Healthy("API is running"),
        tags: new[] { "live" });

#endregion

// --- 2. CONFIGURAÇÃO DO PIPELINE HTTP ---

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

#region Health Check Endpoints

// ------------------------------
// Endpoints de Health Check
// ------------------------------

// Endpoint principal - verifica tudo (para readinessProbe do K8s)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = WriteHealthCheckResponse
});

// Endpoint de liveness - verifica apenas se a API está viva (para livenessProbe do K8s)
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live"),
    ResponseWriter = WriteHealthCheckResponse
});

// Endpoint de readiness - verifica dependências (para readinessProbe do K8s)
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = WriteHealthCheckResponse
});

// Função para formatar resposta JSON dos health checks
static Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json; charset=utf-8";

    var options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    var response = new
    {
        status = report.Status.ToString(),
        totalDuration = report.TotalDuration.TotalMilliseconds,
        checks = report.Entries.Select(e => new
        {
            name = e.Key,
            status = e.Value.Status.ToString(),
            duration = e.Value.Duration.TotalMilliseconds,
            description = e.Value.Description,
            exception = e.Value.Exception?.Message,
            tags = e.Value.Tags
        })
    };

    return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
}

#endregion

app.Run();
