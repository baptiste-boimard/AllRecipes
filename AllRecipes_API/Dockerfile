# Utilisez l'image de base de .NET Core SDK pour construire l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiez les fichiers csproj et restaurez les dépendances
#COPY *.csproj ./

# Copiez le reste des fichiers et construisez l'application
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Utilisez l'image de base de .NET Core Runtime pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Démarrez l'application
ENTRYPOINT ["dotnet", "Dotnet.Docker.dll"]