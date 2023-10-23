FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

# build app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY ./src ./src

WORKDIR /src/ImageManager.NHibernate.InitTool
RUN dotnet restore
RUN dotnet publish "ImageManager.NHibernate.InitTool.csproj" -c Release -o /app/publish

# main container
FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT dotnet ImageManager.NHibernate.InitTool.dll