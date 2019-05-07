FROM microsoft/dotnet:2.2-sdk AS build-env

WORKDIR /app

# Copy and build
COPY . ./

#Restore Packages
RUN dotnet restore

# Build Projects
RUN dotnet build -c release

#Create Output Container Image
FROM microsoft/dotnet:runtime
WORKDIR /app

RUN ls

COPY --from=build-env /app/King.Service.Demo/bin/release/netcoreapp2.2/ .

# Temp Entry
ENTRYPOINT [ "dotnet",  "King.Service.Demo.dll"]