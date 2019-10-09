FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.sln .
COPY homemanagement/*.csproj ./homemanagement/
RUN dotnet restore

# copy everything else and build app
COPY homemanagement/. ./homemanagement/
WORKDIR /app/homemanagement
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
ENV ASPNETCORE_URLS http://*:5000 
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
ENTRYPOINT ["dotnet", "HomeManagement.API.dll"]

#FROM microsoft/aspnetcore-build:2.0 AS build
#WORKDIR /src
#COPY ["HomeManagement.API.csproj", ""]
#COPY ["./HomeManagement.Data/HomeManagement.Data.csproj", "./HomeManagement.API/"]
#COPY ["./HomeManagement.Data.Contracts/HomeManagement.Data.Contracts.csproj", "./HomeManagement.API/"]
#COPY ["../HomeManagement.Domain/HomeManagement.Domain.csproj", ""]
#COPY ["../HomeManagement.Contracts/HomeManagement.Contracts.csproj", ""]
#COPY ["../HomeManagement.Localization/HomeManagement.Localization.csproj", ""]
#COPY ["../HomeManagement.FilesStore/HomeManagement.FilesStore.csproj", ""]
#COPY ["../HomeManagement.Core/HomeManagement.Core.csproj", ""]
#COPY ["../HomeManagement.Mapper/HomeManagement.Mapper.csproj", ""]
#COPY ["../HomeManagement.Models/HomeManagement.Models.csproj", ""]
#RUN dotnet restore "HomeManagement.API.csproj"
#COPY . .
#WORKDIR "/src/HomeManagement.API"
#RUN dotnet build "HomeManagement.API.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "HomeManagement.API.csproj" -c Release -o /app
#
#FROM base AS final
#ENV ASPNETCORE_URLS http://*:5000 
#WORKDIR /app
#COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "HomeManagement.API.dll"]