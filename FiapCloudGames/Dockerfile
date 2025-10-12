# Imagem base com SDK para poder compilar e rodar o projeto dentro do container
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Diretório de trabalho dentro do container
WORKDIR /app

# Variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:5231
ENV ASPNETCORE_ENVIRONMENT=Development

# Expõe a porta da aplicação
EXPOSE 5231

# Comando para rodar a aplicação diretamente do projeto
ENTRYPOINT ["dotnet", "run", "--project", "FiapCloudGames/FiapCloudGamesApi.csproj", "--urls", "http://+:5231"]