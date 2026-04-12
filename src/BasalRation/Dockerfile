# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files for both BasalRationAPI and Domain
COPY ["BasalRation/BasalRation.csproj", "BasalRation/"]
COPY ["Domain/Domain.csproj", "Domain/"]

# Restore dependencies for BasalRationAPI, which includes Domain as a project reference
RUN dotnet restore ./BasalRation/BasalRation.csproj

# Kopieer de volledige broncode
COPY . .

# Build the application
WORKDIR /src/BasalRation
RUN dotnet publish "BasalRation.csproj" -c Release -o /app/publish

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the built files from the build image's /out folder
COPY --from=build app/publish .

# Stel de poort in waarop de applicatie luistert
ENV ASPNETCORE_URLS=http://+:4900
# Expose the application's port
EXPOSE 4900

# Set the entry point to run the app
CMD ["dotnet", "BasalRation.dll"]
