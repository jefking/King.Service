FROM microsoft/dotnet:2.1-sdk AS build-env

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY ./King.Service ./King.Service
COPY ./King.Service.Unit.Tests ./King.Service.Unit.Tests
COPY ./King.Service.Demo ./King.Service.Demo
RUN dotnet restore

# Build Projects
RUN dotnet publish King.Service/King.Service.csproj -c release
RUN dotnet publish King.Service.Tests/King.Service.Tests.csproj -c release
RUN dotnet publish King.Service.Demo/King.Service.Demo.csproj -c release

#Create Output Container Image
FROM microsoft/dotnet:runtime
WORKDIR /app

COPY --from=build-env /app/King.Service.ServiceBus.Demo/bin/release/netcoreapp2.2/ .

# Temp Entry
ENTRYPOINT [ "dotnet",  "King.Service.ServiceBus.Demo.dll"]