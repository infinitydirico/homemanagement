FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["HomeManagement.Services/HomeManagement.Services.csproj", "HomeManagement.Services/"]
COPY ["HomeManagement.Data/HomeManagement.Data.csproj", "HomeManagement.Data/"]
COPY ["HomeManagement.Data.Contracts/HomeManagement.Data.Contracts.csproj", "HomeManagement.Data.Contracts/"]
COPY ["HomeManagement.Domain/HomeManagement.Domain.csproj", "HomeManagement.Domain/"]
COPY ["HomeManagement.Contracts/HomeManagement.Contracts.csproj", "HomeManagement.Contracts/"]
COPY ["HomeManagement.Core/HomeManagement.Core.csproj", "HomeManagement.Core/"]
COPY ["HomeManagement.Tokens/HomeManagement.Api.Core.csproj", "HomeManagement.Tokens/"]
RUN dotnet restore "HomeManagement.Services/HomeManagement.Services.csproj"
COPY . .
WORKDIR "/src/HomeManagement.Services"
RUN dotnet build "HomeManagement.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeManagement.Services.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeManagement.Services.dll"]