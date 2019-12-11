FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["HomeManagement.API/HomeManagement.API.csproj", "HomeManagement.API/"]
COPY ["HomeManagement.Data/HomeManagement.Data.csproj", "HomeManagement.Data/"]
COPY ["HomeManagement.Data.Contracts/HomeManagement.Data.Contracts.csproj", "HomeManagement.Data.Contracts/"]
COPY ["HomeManagement.Domain/HomeManagement.Domain.csproj", "HomeManagement.Domain/"]
COPY ["HomeManagement.Contracts/HomeManagement.Contracts.csproj", "HomeManagement.Contracts/"]
COPY ["HomeManagement.Localization/HomeManagement.Localization.csproj", "HomeManagement.Localization/"]
COPY ["HomeManagement.FilesStore/HomeManagement.FilesStore.csproj", "HomeManagement.FilesStore/"]
COPY ["HomeManagement.Core/HomeManagement.Core.csproj", "HomeManagement.Core/"]
COPY ["HomeManagement.Business/HomeManagement.Business.csproj", "HomeManagement.Business/"]
COPY ["HomeManagement.Mapper/HomeManagement.Mapper.csproj", "HomeManagement.Mapper/"]
COPY ["HomeManagement.Models/HomeManagement.Models.csproj", "HomeManagement.Models/"]
COPY ["HomeManagement.AI.Vision/HomeManagement.AI.Vision.csproj", "HomeManagement.AI.Vision/"]
COPY ["HomeManagement.Tokens/HomeManagement.Api.Core.csproj", "HomeManagement.Tokens/"]
RUN dotnet restore "HomeManagement.API/HomeManagement.API.csproj"
COPY . .
WORKDIR "/src/HomeManagement.API"
RUN dotnet build "HomeManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeManagement.API.dll"]