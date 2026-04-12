#!/bin/bash

# Ensure Docker CLI is installed
if ! command -v docker &>/dev/null; then
  echo "Error: Docker CLI is not installed. Please install Docker and try again."
  exit 1
fi

# Read version number from the VERSION file
VERSION=$(cat ./VERSION | xargs) # Trim leading and trailing whitespace

# Validate that the VERSION is not empty
if [[ -z "$VERSION" ]]; then
  echo "Error: VERSION is empty. Ensure the VERSION file contains a valid version."
  exit 1
fi

# Define the Azure Container Registry (ACR) name
ACR_NAME="minorhangroep5.azurecr.io"

# Define services and their associated image names
declare -A SERVICES=(
  ["webapp"]="webapp:1.0"
  ["importration"]="importration:1.0"
  ["basalrationapi"]="basalrationapi:1.0"
  ["rationapi"] = "rationapi:1.0"
  ["energyfoodapi"] = "energyfoodapi:1.0"
  ["normapi"] = "normapi:1.0"
)

# Loop through the services and tag/push each image
for SERVICE in "${!SERVICES[@]}"; do
  IMAGE_NAME=${SERVICES[$SERVICE]}

  # Tag the image with the new version
  echo "Tagging $IMAGE_NAME as $ACR_NAME/$SERVICE:$VERSION"
  docker tag "$IMAGE_NAME" "$ACR_NAME/$SERVICE:$VERSION"

  # Push the tagged image to the Azure Container Registry
  echo "Pushing $ACR_NAME/$SERVICE:$VERSION to ACR"
  docker push "$ACR_NAME/$SERVICE:$VERSION"
done

echo "All Docker images tagged and pushed successfully with version: $VERSION"
