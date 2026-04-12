param (
    [string]$name
)

$SolutionDir = Get-Location

$ProjectDirs = @(
    "$SolutionDir\BasalRation",
    "$SolutionDir\EnergyFood",
    "$SolutionDir\Ration",
    "$SolutionDir\Norms",
    "$SolutionDir\ImportRation"
)

foreach ($ProjectDir in $ProjectDirs) {
    # Check if the directory exists
    if (!(Test-Path -Path $ProjectDir)) {
        Write-Host "Project directory does not exist: $ProjectDir" -ForegroundColor Red
        continue
    }

    # Navigate to the project directory
    Set-Location -Path $ProjectDir

    # Run the migration command
    Write-Host "Creating migration: $Name in $ProjectDir" -ForegroundColor Green
    try {
        dotnet ef migrations add $Name
        Write-Host "Migration '$Name' created successfully in $ProjectDir." -ForegroundColor Green
    } catch {
        $errorMessage = $_.Exception.Message
        Write-Host ("Error creating migration in " + $ProjectDir + ": " + $errorMessage) -ForegroundColor Red
    }
}

Set-Location -Path $SolutionDir
Write-Host "Done!"
