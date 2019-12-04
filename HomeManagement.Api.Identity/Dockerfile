FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["HomeManagement.Api.Identity/HomeManagement.Api.Identity.csproj", "HomeManagement.Api.Identity/"]
COPY ["HomeManagement.Data/HomeManagement.Data.csproj", "HomeManagement.Data/"]
COPY ["HomeManagement.Data.Contracts/HomeManagement.Data.Contracts.csproj", "HomeManagement.Data.Contracts/"]
COPY ["HomeManagement.Domain/HomeManagement.Domain.csproj", "HomeManagement.Domain/"]
COPY ["HomeManagement.Contracts/HomeManagement.Contracts.csproj", "HomeManagement.Contracts/"]
COPY ["HomeManagement.Core/HomeManagement.Core.csproj", "HomeManagement.Core/"]
COPY ["HomeManagement.Tokens/HomeManagement.Api.Core.csproj", "HomeManagement.Tokens/"]
COPY ["HomeManagement.Models/HomeManagement.Models.csproj", "HomeManagement.Models/"]
RUN dotnet restore "HomeManagement.Api.Identity/HomeManagement.Api.Identity.csproj"
COPY . .
WORKDIR "/src/HomeManagement.Api.Identity"
RUN dotnet build "HomeManagement.Api.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeManagement.Api.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeManagement.Api.Identity.dll"]