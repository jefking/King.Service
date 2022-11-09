FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy and build
COPY ./King.Service ./King.Service
COPY ./King.Service.Tests ./King.Service.Tests
COPY ./King.Service.Demo ./Demo

# Unit Test Project
RUN dotnet test King.Service.Tests/King.Service.Tests.csproj

# Public Project
RUN dotnet publish Demo/King.Service.Demo.csproj -c release

# Create Output Container Image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app

# Copy Demo
COPY --from=build-env /app/Demo/bin/release/net6.0/publish/. .

# Temp Entry
ENTRYPOINT [ "dotnet",  "King.Service.Demo.dll"]