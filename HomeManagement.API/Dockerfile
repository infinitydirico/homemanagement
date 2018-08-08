FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY HomeManagement.API/HomeManagement.API.csproj HomeManagement.API/
RUN dotnet restore HomeManagement.API/HomeManagement.API.csproj
COPY . .
WORKDIR /src/HomeManagement.API
RUN dotnet build HomeManagement.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish HomeManagement.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HomeManagement.API.dll"]
