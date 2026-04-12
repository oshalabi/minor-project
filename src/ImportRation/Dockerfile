# Gebruik het officiële .NET 8 SDK-image om de applicatie te bouwen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopieer de projectbestanden en herstel afhankelijkheden
COPY ["ImportRation/ImportRation.csproj", "ImportRation/"]
COPY ["Domain/Domain.csproj", "Domain/"]

# Herstel afhankelijkheden met dotnet restore
RUN dotnet restore "ImportRation/ImportRation.csproj"

# Kopieer de volledige broncode
COPY . .

# Bouw en publiceer de applicatie
WORKDIR /src/ImportRation
RUN dotnet publish "ImportRation.csproj" -c Release -o /app/publish

# Gebruik het officiële .NET 8 ASP.NET runtime image voor productie
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Kopieer de gepubliceerde bestanden vanuit de build-stage
COPY --from=build /app/publish .

# Stel de poort in waarop de applicatie luistert
ENV ASPNETCORE_URLS=http://+:4201
EXPOSE 4201

# Start de applicatie
ENTRYPOINT ["dotnet", "ImportRation.dll"]