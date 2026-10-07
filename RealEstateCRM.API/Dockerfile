FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["RealEstateCRM.API/RealEstateCRM.API.csproj", "RealEstateCRM.API/"]
COPY ["RealEstateCRM.Core/RealEstateCRM.Core.csproj", "RealEstateCRM.Core/"]
COPY ["RealEstateCRM.Infrastructure/RealEstateCRM.Infrastructure.csproj", "RealEstateCRM.Infrastructure/"]
RUN dotnet restore "RealEstateCRM.API/RealEstateCRM.API.csproj"
COPY . .
WORKDIR "/src/RealEstateCRM.API"
RUN dotnet build "RealEstateCRM.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RealEstateCRM.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealEstateCRM.API.dll"]