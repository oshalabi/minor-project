# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Ration/Ration.csproj", "Ration/"]
COPY ["Domain/Domain.csproj", "Domain/"]

RUN dotnet restore ./Ration/Ration.csproj

# Kopieer de volledige broncode
COPY . .

# Build the application
WORKDIR /src/Ration
RUN dotnet publish "Ration.csproj" -c Release -o /app/publish

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the built files from the build image's /out folder
COPY --from=build app/publish .

# Stel de poort in waarop de applicatie luistert
ENV ASPNETCORE_URLS=http://+:5156
# Expose the application's port
EXPOSE 5156

# Set the entry point to run the app
CMD ["dotnet", "Ration.dll"]
