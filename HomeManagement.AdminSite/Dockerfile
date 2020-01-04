FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["HomeManagement.AdminSite/HomeManagement.AdminSite.csproj", "HomeManagement.AdminSite/"]
COPY ["HomeManagement.Business/HomeManagement.Business.csproj", "HomeManagement.Business/"]
COPY ["HomeManagement.Data/HomeManagement.Data.csproj", "HomeManagement.Data/"]
COPY ["HomeManagement.Data.Contracts/HomeManagement.Data.Contracts.csproj", "HomeManagement.Data.Contracts/"]
COPY ["HomeManagement.Domain/HomeManagement.Domain.csproj", "HomeManagement.Domain/"]
COPY ["HomeManagement.Contracts/HomeManagement.Contracts.csproj", "HomeManagement.Contracts/"]
COPY ["HomeManagement.Mapper/HomeManagement.Mapper.csproj", "HomeManagement.Mapper/"]
COPY ["HomeManagement.Core/HomeManagement.Core.csproj", "HomeManagement.Core/"]
COPY ["HomeManagement.Models/HomeManagement.Models.csproj", "HomeManagement.Models/"]
RUN dotnet restore "HomeManagement.AdminSite/HomeManagement.AdminSite.csproj"
COPY . .
WORKDIR "/src/HomeManagement.AdminSite"
RUN dotnet build "HomeManagement.AdminSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeManagement.AdminSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeManagement.AdminSite.dll"]