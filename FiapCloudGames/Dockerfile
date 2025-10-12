# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Diretório de trabalho dentro do container
WORKDIR /src

# Copia todos os arquivos do projeto para o container
COPY . .

# Restaura os pacotes NuGet da solução
RUN dotnet restore FiapCloudGames.sln

# Publica a aplicação principal em Release
RUN dotnet publish FiapCloudGames/FiapCloudGamesApi.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Diretório de trabalho no container final
WORKDIR /app

# Copia a publicação da etapa de build
COPY --from=build /app/publish .

# Variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:5231
ENV ASPNETCORE_ENVIRONMENT=Development

# Expõe a porta que a aplicação vai rodar
EXPOSE 5231

# Comando para rodar a aplicação a partir do publish
ENTRYPOINT ["dotnet", "FiapCloudGamesApi.dll"]
