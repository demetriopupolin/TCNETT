# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia a solução
COPY FiapCloudGames.sln ./

# Copia todos os projetos em suas respectivas pastas
COPY FiapCloudGamesApi/ FiapCloudGamesApi/
COPY Core/ Core/
COPY Infrastructure/ Infrastructure/
COPY Tests/ Tests/

# Restaura as dependências
RUN dotnet restore

# Copia todo o restante do código (se houver arquivos adicionais)
COPY . ./

# Publica o projeto principal (API)
RUN dotnet publish FiapCloudGamesApi/FiapCloudGamesApi.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish ./

# Expõe a porta da API
EXPOSE 80

# Inicia a API
ENTRYPOINT ["dotnet", "FiapCloudGamesApi.dll"]
