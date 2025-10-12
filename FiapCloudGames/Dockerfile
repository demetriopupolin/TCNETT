# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Diretório de trabalho dentro do container
WORKDIR /src

# Copia todos os arquivos do projeto
COPY . .

# Restaura os pacotes NuGet
RUN dotnet restore FiapCloudGames.sln

# Publica a aplicação em Release para /app/publish
RUN dotnet publish FiapCloudGames/FiapCloudGamesApi.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Diretório de trabalho
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /app/publish .

# Variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:5231
ENV ASPNETCORE_ENVIRONMENT=Development

# Expõe a porta da aplicação
EXPOSE 5231

# Comando para rodar a DLL publicada
ENTRYPOINT ["dotnet", "FiapCloudGamesApi.dll"]
