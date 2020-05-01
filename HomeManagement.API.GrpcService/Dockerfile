FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["HomeManagement.API.GrpcService/HomeManagement.API.GrpcService.csproj", "HomeManagement.API.GrpcService/"]
COPY ["HomeManagement.Data/HomeManagement.Data.csproj", "HomeManagement.Data/"]
COPY ["HomeManagement.Data.Contracts/HomeManagement.Data.Contracts.csproj", "HomeManagement.Data.Contracts/"]
COPY ["HomeManagement.Domain/HomeManagement.Domain.csproj", "HomeManagement.Domain/"]
COPY ["HomeManagement.Contracts/HomeManagement.Contracts.csproj", "HomeManagement.Contracts/"]
COPY ["HomeManagement.Core/HomeManagement.Core.csproj", "HomeManagement.Core/"]
COPY ["HomeManagement.Tokens/HomeManagement.Api.Core.csproj", "HomeManagement.Tokens/"]
RUN dotnet restore "HomeManagement.API.GrpcService/HomeManagement.API.GrpcService.csproj"
COPY . .
WORKDIR "/src/HomeManagement.API.GrpcService"
RUN dotnet build "HomeManagement.API.GrpcService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeManagement.API.GrpcService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeManagement.API.GrpcService.dll"]