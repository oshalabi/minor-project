# Run all containers in the background, and directly afterwards tail the logs files (like"
# running in the foreground. When pressing crtl-c, youre containers will continue to run."
# Updated something? Then run the RebuildAllDockerImages script to run changes."

# Run Rebuild script
./RebuildAllDockerImages.ps1

# Step 2: Start docker compose

docker-compose up -d; docker-compose logs -f
