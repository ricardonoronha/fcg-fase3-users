# ============================================================================
# FIAP Cloud Games - Microsserviço de Users
# Dockerfile Otimizado para Kubernetes - Fase 4
# ============================================================================

# -----------------------------------------------------------------------------
# Stage 1: Build
# -----------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /src

RUN mkdir -p FIAP.MicroService.Usuario \
			 FIAP.MicroService.Usuario.API \
			 FIAP.MicroService.Usuario.Dominio \
			 FIAP.MicroService.Usuario.Infraestrutura
			 
# Copiar apenas arquivos de projeto primeiro (melhor cache de layers)
COPY FIAP.MicroService.Usuario/FIAP.MicroService.Usuario.csproj FIAP.MicroService.Usuario/
COPY FIAP.MicroService.Usuario.API/FIAP.MicroService.Usuario.API.csproj FIAP.MicroService.Usuario.API/
COPY FIAP.MicroService.Usuario.Dominio/FIAP.MicroService.Usuario.Dominio.csproj FIAP.MicroService.Usuario.Dominio/
COPY FIAP.MicroService.Usuario.Infraestrutura/FIAP.MicroService.Usuario.Infraestrutura.csproj FIAP.MicroService.Usuario.Infraestrutura/


# Restore de dependências
RUN dotnet restore ./FIAP.MicroService.Usuario.API/FIAP.MicroService.Usuario.API.csproj

# Copiar código fonte
COPY FIAP.MicroService.Usuario/ FIAP.MicroService.Usuario/
COPY FIAP.MicroService.Usuario.API/ FIAP.MicroService.Usuario.API/
COPY FIAP.MicroService.Usuario.Dominio/ FIAP.MicroService.Usuario.Dominio/
COPY FIAP.MicroService.Usuario.Infraestrutura/ FIAP.MicroService.Usuario.Infraestrutura/

# Build da aplicação

RUN dotnet build ./FIAP.MicroService.Usuario.API/FIAP.MicroService.Usuario.API.csproj -c Release -o /app/build

# -----------------------------------------------------------------------------
# Stage 2: Publish
# -----------------------------------------------------------------------------
FROM build AS publish

RUN dotnet publish ./FIAP.MicroService.Usuario.API/FIAP.MicroService.Usuario.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# -----------------------------------------------------------------------------
# Stage 3: Runtime (Imagem Final)
# -----------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

LABEL maintainer="Grupo 118 - FIAP Tech Challenge" \
      version="4.0" \
      description="FCG Users API - Microsserviço de Usuários"

# Instalar dependências necessárias em Alpine
RUN apk add --no-cache icu-libs libc6-compat libgcc libstdc++

# Criar usuário non-root
RUN addgroup -g 1000 appgroup && \
    adduser -u 1000 -G appgroup -D -s /bin/sh appuser

WORKDIR /app

# Copiar binários publicados
COPY --from=publish /app/publish .

# Ajustar permissões

USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

ENTRYPOINT ["dotnet", "FIAP.MicroService.Usuario.API.dll"]
