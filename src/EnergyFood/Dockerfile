# Gebruik het officiële .NET 8 SDK-image om de applicatie te bouwen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EnergyFood/EnergyFood.csproj", "EnergyFood/"]
COPY ["Domain/Domain.csproj", "Domain/"]

RUN dotnet restore "EnergyFood/EnergyFood.csproj"

COPY . .

WORKDIR /src/EnergyFood
RUN dotnet publish "EnergyFood.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:4300
EXPOSE 4300

ENTRYPOINT ["dotnet", "EnergyFood.dll"]