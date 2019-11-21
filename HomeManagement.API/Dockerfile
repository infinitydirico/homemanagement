FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

#COPY *.csproj ./
#COPY ./HomeManagement.*.csproj ./
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
#WORKDIR "/src/HomeManagement.API"
RUN dotnet build "HomeManagement.API/HomeManagement.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HomeManagement.API/HomeManagement.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HomeManagement.API.dll"]