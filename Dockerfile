# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar archivos de solución
COPY Onboarding.sln .
COPY Onboarding.CORE/ Onboarding.CORE/
COPY WebApplication1/ WebApplication1/

# Restaurar paquetes
RUN dotnet restore

# Publicar proyecto principal (Tu API)
RUN dotnet publish WebApplication1/Onboarding.Api.csproj -c Release -o /app/out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copiar el resultado publicado
COPY --from=build /app/out .

# Exponer el puerto (Render usa el PORT dinámico)
EXPOSE 10000

# Comando de inicio
ENTRYPOINT ["dotnet", "Onboarding.Api.dll"]
