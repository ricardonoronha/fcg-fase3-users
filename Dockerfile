# ============================================================================
# FIAP Cloud Games - Microsserviço de Usuários
# Dockerfile Otimizado para Kubernetes - Fase 4
# ============================================================================

# -----------------------------------------------------------------------------
# Stage 1: Build
# -----------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /src

# Copiar apenas arquivos de projeto primeiro (melhor cache de layers)
COPY FIAP.MicroService.Usuario.API/*.csproj ./FIAP.MicroService.Usuario.API/
COPY FIAP.MicroService.Usuario.Dominio/*.csproj ./FIAP.MicroService.Usuario.Dominio/
COPY FIAP.MicroService.Usuario.Infraestrutura/*.csproj ./FIAP.MicroService.Usuario.Infraestrutura/

# Restore de dependências
RUN dotnet restore ./FIAP.MicroService.Usuario.API/FIAP.MicroService.Usuario.API.csproj

# Copiar código fonte (exceto testes)
COPY FIAP.MicroService.Usuario.API/ ./FIAP.MicroService.Usuario.API/
COPY FIAP.MicroService.Usuario.Dominio/ ./FIAP.MicroService.Usuario.Dominio/
COPY FIAP.MicroService.Usuario.Infraestrutura/ ./FIAP.MicroService.Usuario.Infraestrutura/

# Build da aplicação
WORKDIR /src/FIAP.MicroService.Usuario.API
RUN dotnet build -c Release -o /app/build

# -----------------------------------------------------------------------------
# Stage 2: Publish
# -----------------------------------------------------------------------------
FROM build AS publish

RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# -----------------------------------------------------------------------------
# Stage 3: DataDog Tracer
# -----------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS datadog-installer

RUN apt-get update && apt-get install -y --no-install-recommends curl ca-certificates \
    && TRACER_VERSION=$(curl -s https://api.github.com/repos/DataDog/dd-trace-dotnet/releases/latest | grep tag_name | cut -d '"' -f 4 | cut -c2-) \
    && curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/download/v${TRACER_VERSION}/datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb \
    && dpkg -i ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb \
    && rm ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb \
    && apt-get purge -y curl \
    && apt-get autoremove -y \
    && rm -rf /var/lib/apt/lists/*

# -----------------------------------------------------------------------------
# Stage 4: Runtime (Imagem Final)
# -----------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

LABEL maintainer="Grupo 118 - FIAP Tech Challenge" \
      version="4.0" \
      description="FCG Usuarios API - Microsserviço de Usuários"

# Instalar dependências necessárias para DataDog em Alpine
RUN apk add --no-cache icu-libs libc6-compat libgcc libstdc++

# Copiar DataDog tracer
COPY --from=datadog-installer /opt/datadog /opt/datadog
COPY --from=datadog-installer /var/log/datadog /var/log/datadog

# Configurar variáveis de ambiente do DataDog
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8} \
    CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so \
    DD_DOTNET_TRACER_HOME=/opt/datadog \
    DD_INTEGRATIONS=/opt/datadog/integrations.json

# Criar usuário non-root
RUN addgroup -g 1000 appgroup && \
    adduser -u 1000 -G appgroup -D -s /bin/sh appuser

WORKDIR /app

# Copiar binários publicados
COPY --from=publish /app/publish .

# Ajustar permissões
RUN chown -R appuser:appgroup /app /var/log/datadog

USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "FIAP.MicroService.Usuario.API.dll"]
