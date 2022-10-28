FROM mcr.microsoft.com/dotnet/sdk AS build-env
WORKDIR /app

# Copy and build
COPY ./King.Service ./King.Service
COPY ./King.Service.Azure ./King.Service.Azure
COPY ./King.Service.Tests ./King.Service.Tests
COPY ./King.Service.Azure.Tests ./King.Service.Azure.Tests
COPY ./King.Service.Demo ./Demo

# Unit Test Project
RUN dotnet test King.Service.Tests/King.Service.Tests.csproj
RUN dotnet test King.Service.Azure.Tests/King.Service.Azure.Tests.csproj

# Public Project
RUN dotnet publish Demo/King.Service.Demo.csproj -c release

# Create Output Container Image
FROM mcr.microsoft.com/dotnet/runtime
WORKDIR /app

# Copy Demo
COPY --from=build-env /app/Demo/bin/release/netcoreapp5.0/publish/. .

# Temp Entry
ENTRYPOINT [ "dotnet",  "King.Service.Demo.dll"]