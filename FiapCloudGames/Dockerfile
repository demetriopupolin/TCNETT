# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de solução
COPY FiapCloudGames.sln ./

# Copia cada projeto em sua pasta correta
COPY Core/ Core/
COPY FiapCloudGames/ FiapCloudGames/
COPY Infrastructure/ Infrastructure/
COPY Tests/ Tests/

# Restaura as dependências da solução
RUN dotnet restore FiapCloudGames.sln

# Publica a aplicação principal
RUN dotnet publish FiapCloudGames/FiapCloudGamesApi.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /app/publish .

# Expõe a porta padrão da API
EXPOSE 5231

# Inicia a API
ENTRYPOINT ["dotnet", "FiapCloudGamesApi.dll"]
