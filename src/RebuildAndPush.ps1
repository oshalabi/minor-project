# Ensure Docker CLI is installed
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Error "Error: Docker CLI is not installed. Please install Docker and try again."
    exit 1
}

# Check for Rebuild script
$rebuildScript = "./RebuildAllDockerImages.ps1"
if (-not (Test-Path $rebuildScript)) {
    Write-Error "Error: Rebuild script not found at $rebuildScript. Ensure the file exists."
    exit 1
}

# Check for Push script
$pushScript = "./PushAllDockerImages.ps1"
if (-not (Test-Path $pushScript)) {
    Write-Error "Error: Push script not found at $pushScript. Ensure the file exists."
    exit 1
}

# Run the Rebuild script
Write-Host "Running the Rebuild script..."
& $rebuildScript
if ($LASTEXITCODE -ne 0) {
    Write-Error "Error: Rebuild script failed. Exiting."
    exit $LASTEXITCODE
}

# Run the Push script
Write-Host "Running the Push script..."
& $pushScript
if ($LASTEXITCODE -ne 0) {
    Write-Error "Error: Push script failed. Exiting."
    exit $LASTEXITCODE
}

Write-Host "Rebuild and Push operations completed successfully!"
