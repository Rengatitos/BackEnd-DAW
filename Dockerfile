# Etapa 1: Build (.NET 9 SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de solución
COPY Onboarding.sln ./
COPY Onboarding.CORE/ Onboarding.CORE/
COPY WebApplication1/ WebApplication1/

# Restaurar paquetes
RUN dotnet restore

# Publicar proyecto principal (Tu API)
RUN dotnet publish WebApplication1/Onboarding.Api.csproj -c Release -o /app/out

# Etapa 2: Runtime (.NET 9)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copiar el resultado publicado
COPY --from=build /app/out .

# Render usa un puerto dinámico, está bien solo exponer uno
EXPOSE 8080

# Iniciar API
ENTRYPOINT ["dotnet", "Onboarding.Api.dll"]

