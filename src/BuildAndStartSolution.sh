#!/bin/bash

# Run all containers in the background and tail the logs files like running in the foreground.
# Pressing ctrl-c will stop the logs but leave containers running.
# Updated something? Run the RebuildAllDockerImages script to apply changes.

# Run Rebuild script
./RebuildAllDockerImages.sh

# Step 2: Start docker compose

docker-compose up -d; docker-compose logs -f

