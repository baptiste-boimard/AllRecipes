FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /App/out .

# Exposer le port 80 (port par défaut pour les applications ASP.NET Core dans les conteneurs)
EXPOSE 3001
# Exposer le port 8080 pour redirection
#EXPOSE 8080

ENTRYPOINT ["dotnet", "AllRecipes_API.dll"]