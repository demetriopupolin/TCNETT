# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas arquivos de solução e projetos para aproveitar cache
COPY FiapCloudGames/*.sln ./FiapCloudGames/
COPY FiapCloudGames/**/*.csproj ./FiapCloudGames/

# Restaura as dependências
RUN dotnet restore "FiapCloudGames/FiapCloudGames.sln"

# Copia todo o restante do código
COPY FiapCloudGames/. ./FiapCloudGames/

# Build em Release
RUN dotnet build "FiapCloudGames/FiapCloudGames.sln" --configuration Release --no-restore
RUN dotnet publish "FiapCloudGames/FiapCloudGames.sln" --configuration Release --output /app/publish --no-restore

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /app/publish .

# Expõe a porta padrão da API
EXPOSE 80

# Inicia a API
ENTRYPOINT ["dotnet", "FiapCloudGames.dll"]
