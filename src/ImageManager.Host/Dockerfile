FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# build app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY ./src ./src

WORKDIR /src/ImageManager.Host
RUN dotnet publish "ImageManager.Host.csproj" -c Release -o /app/publish

# main container
FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT dotnet ImageManager.Host.dll
