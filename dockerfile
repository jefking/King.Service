FROM mcr.microsoft.com/dotnet/sdk AS build-env

WORKDIR /app

# Copy and build
COPY ./King.Service ./King.Service
COPY ./King.Service.Demo ./Demo

# Public Project
RUN dotnet publish Demo/King.Service.Demo.csproj -c release

# Create Output Container Image
FROM mcr.microsoft.com/dotnet/runtime
WORKDIR /app

# Copy Demo
COPY --from=build-env /app/Demo/bin/release/netcoreapp2.2/publish/. .

# Temp Entry
ENTRYPOINT [ "dotnet",  "King.Service.Demo.dll"]