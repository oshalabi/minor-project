#!/bin/bash

# Ensure Docker CLI is installed
if ! command -v docker &>/dev/null; then
  echo "Error: Docker CLI is not installed. Please install Docker and try again."
  exit 1
fi

# Check for Rebuild script
REBUILD_SCRIPT="./RebuildAllDockerImages.sh"
if [[ ! -f "$REBUILD_SCRIPT" ]]; then
  echo "Error: Rebuild script not found at $REBUILD_SCRIPT. Ensure the file exists."
  exit 1
fi

# Check for Push script
PUSH_SCRIPT="./PushAllDockerImages.sh"
if [[ ! -f "$PUSH_SCRIPT" ]]; then
  echo "Error: Push script not found at $PUSH_SCRIPT. Ensure the file exists."
  exit 1
fi

# Run the Rebuild script
echo "Running the Rebuild script..."
bash "$REBUILD_SCRIPT"
if [[ $? -ne 0 ]]; then
  echo "Error: Rebuild script failed. Exiting."
  exit 1
fi

# Run the Push script
echo "Running the Push script..."
bash "$PUSH_SCRIPT"
if [[ $? -ne 0 ]]; then
  echo "Error: Push script failed. Exiting."
  exit 1
fi

echo "Rebuild and Push operations completed successfully!"
