# Ensure Docker CLI is installed
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Error "Error: Docker CLI is not installed. Please install Docker and try again."
    exit 1
}

# Read version number from the VERSION file
if (-not (Test-Path "./VERSION")) {
    Write-Error "Error: VERSION file not found. Ensure the VERSION file exists in the project root."
    exit 1
}

$VERSION = Get-Content "../VERSION" -Raw
$VERSION = $VERSION.Trim()

# Validate that the VERSION is not empty
if (-not $VERSION) {
    Write-Error "Error: VERSION is empty. Ensure the VERSION file contains a valid version."
    exit 1
}

# Define the Azure Container Registry (ACR) name
$ACR_NAME = "minorhangroep5.azurecr.io"

# Define services and their associated image names
$SERVICES = @{
    "webapp" = "webapp:1.0"
    "importration" = "importration:1.0"
    "basalrationapi" = "basalrationapi:1.0"
    "rationapi" = "rationapi:1.0"
    "energyfoodapi" = "energyfoodapi:1.0"
    "normapi" = "normapi:1.0"
}

# Loop through the services and tag/push each image
foreach ($SERVICE in $SERVICES.Keys) {
    $IMAGE_NAME = $SERVICES[$SERVICE]

    # Tag the image with the new version
    Write-Host "Tagging $IMAGE_NAME as ${ACR_NAME}/${SERVICE}:${VERSION}"
    docker tag $IMAGE_NAME "${ACR_NAME}/${SERVICE}:${VERSION}"

    # Push the tagged image to the Azure Container Registry
    Write-Host "Pushing ${ACR_NAME}/${SERVICE}:${VERSION} to ACR"
    docker push "${ACR_NAME}/${SERVICE}:${VERSION}"
}

Write-Host "All Docker images tagged and pushed successfully with version: $VERSION"
