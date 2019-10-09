FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["HomeManagement.API.csproj", ""]
COPY ["./HomeManagement.Data/HomeManagement.Data.csproj", "./HomeManagement.API/"]
COPY ["./HomeManagement.Data.Contracts/HomeManagement.Data.Contracts.csproj", "./HomeManagement.API/"]
COPY ["../HomeManagement.Domain/HomeManagement.Domain.csproj", ""]
COPY ["../HomeManagement.Contracts/HomeManagement.Contracts.csproj", ""]
COPY ["../HomeManagement.Localization/HomeManagement.Localization.csproj", ""]
COPY ["../HomeManagement.FilesStore/HomeManagement.FilesStore.csproj", ""]
COPY ["../HomeManagement.Core/HomeManagement.Core.csproj", ""]
COPY ["../HomeManagement.Mapper/HomeManagement.Mapper.csproj", ""]
COPY ["../HomeManagement.Models/HomeManagement.Models.csproj", ""]
RUN dotnet restore "HomeManagement.API.csproj"
COPY . .
WORKDIR "/src/HomeManagement.API"
RUN dotnet build "HomeManagement.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HomeManagement.API.csproj" -c Release -o /app

FROM base AS final
ENV ASPNETCORE_URLS http://*:5000 
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HomeManagement.API.dll"]