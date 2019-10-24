FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["HomeManagement.AdminSite/HomeManagement.AdminSite.csproj", "HomeManagement.AdminSite/"]
COPY ["HomeManagement.Core/HomeManagement.Core.csproj", "HomeManagement.Core/"]
COPY ["HomeManagement.Contracts/HomeManagement.Contracts.csproj", "HomeManagement.Contracts/"]
COPY ["HomeManagement.Models/HomeManagement.Models.csproj", "HomeManagement.Models/"]
RUN dotnet restore "HomeManagement.AdminSite/HomeManagement.AdminSite.csproj"
COPY . .
WORKDIR "/src/HomeManagement.AdminSite"
RUN dotnet build "HomeManagement.AdminSite.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HomeManagement.AdminSite.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HomeManagement.AdminSite.dll"]