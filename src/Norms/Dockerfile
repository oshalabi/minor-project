# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files for both BasalRationAPI and Domain
COPY ./Norms/Norms.csproj ./Norms/
COPY ./Domain/Domain.csproj ./Domain/

# Restore dependencies for BasalRationAPI, which includes Domain as a project reference
RUN dotnet restore ./Norms/Norms.csproj

# Copy all source files for both BasalRationAPI and Domain
COPY ./Norms ./Norms
COPY ./Domain ./Domain

# Build the application
WORKDIR /app/Norms
RUN dotnet publish -c Release -o /out

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the built files from the build image's /out folder
COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:4850
# Expose the application's port
EXPOSE 4850

# Set the entry point to run the app
CMD ["dotnet", "Norms.dll"]