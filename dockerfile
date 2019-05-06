FROM microsoft/dotnet:2.1-sdk AS build-env

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY ./King.Service ./King.Service
COPY ./King.Service.Unit.Tests ./King.Service.Unit.Tests
COPY ./King.Service.Demo ./King.Service.Demo
RUN dotnet restore