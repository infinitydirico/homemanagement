FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["HomeManagement.API/HomeManagement.API.csproj", "HomeManagement.API/"]
COPY ["HomeManagement.Data/HomeManagement.Data.csproj", "HomeManagement.Data/"]
COPY ["HomeManagement.Data.Contracts/HomeManagement.Data.Contracts.csproj", "HomeManagement.Data.Contracts/"]
COPY ["HomeManagement.Domain/HomeManagement.Domain.csproj", "HomeManagement.Domain/"]
COPY ["HomeManagement.Contracts/HomeManagement.Contracts.csproj", "HomeManagement.Contracts/"]
COPY ["HomeManagement.Localization/HomeManagement.Localization.csproj", "HomeManagement.Localization/"]
COPY ["HomeManagement.FilesStore/HomeManagement.FilesStore.csproj", "HomeManagement.FilesStore/"]
COPY ["HomeManagement.Core/HomeManagement.Core.csproj", "HomeManagement.Core/"]
COPY ["HomeManagement.Mapper/HomeManagement.Mapper.csproj", "HomeManagement.Mapper/"]
COPY ["HomeManagement.Models/HomeManagement.Models.csproj", "HomeManagement.Models/"]
RUN dotnet restore "HomeManagement.API/HomeManagement.API.csproj"
COPY . .
WORKDIR "/src/HomeManagement.API"
RUN dotnet build "HomeManagement.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HomeManagement.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HomeManagement.API.dll"]