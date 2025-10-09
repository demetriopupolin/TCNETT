# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia a solução e os projetos
COPY *.sln ./
COPY *.csproj ./

# Restaura as dependências
RUN dotnet restore

# Copia todo o restante do código
COPY . ./

# Publica o projeto (compila e prepara para rodar)
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados da etapa anterior
COPY --from=build /app/publish ./

# Expõe a porta padrão da API
EXPOSE 80

# Inicia a API
ENTRYPOINT ["dotnet", "FiapCloudGames.dll"]
