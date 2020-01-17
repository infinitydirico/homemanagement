#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["HomeManagement.API.Storage/HomeManagement.API.Storage.csproj", "HomeManagement.API.Storage/"]
COPY ["HomeManagement.Tokens/HomeManagement.Api.Core.csproj", "HomeManagement.Tokens/"]
COPY ["HomeManagement.Core/HomeManagement.Core.csproj", "HomeManagement.Core/"]
COPY ["HomeManagement.Contracts/HomeManagement.Contracts.csproj", "HomeManagement.Contracts/"]
COPY ["HomeManagement.Models/HomeManagement.Models.csproj", "HomeManagement.Models/"]
RUN dotnet restore "HomeManagement.API.Storage/HomeManagement.API.Storage.csproj"
COPY . .
WORKDIR "/src/HomeManagement.API.Storage"
RUN dotnet build "HomeManagement.API.Storage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeManagement.API.Storage.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeManagement.API.Storage.dll"]