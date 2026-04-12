# Define the version file
$versionFile = "./VERSION"

# Define the path to the Angular package.json
$packageJsonPath = "./src/WebApp/package.json"

# Validate if Git is installed
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Error "Git is not installed or not available in PATH. Please install Git and try again."
    exit 1
}

# Validate if the script is being run inside a Git repository
if (-not (Test-Path ".git")) {
    Write-Error "The .git directory is not found in the current working directory. Please ensure you're in a valid Git repository."
    exit 1
}

# Determine the current branch
$currentBranch = git rev-parse --abbrev-ref HEAD 2>$null
if (-not $currentBranch -or $currentBranch -eq "HEAD") {
    # Handle detached HEAD state
    $currentBranch = git describe --all --contains --always 2>$null
    Write-Host "Running in a detached HEAD state. Using branch: $currentBranch"
}

# Strip `remotes/origin/` prefix if present
if ($currentBranch -like "remotes/origin/*") {
    $currentBranch = $currentBranch -replace "remotes/origin/", ""
}

# Debug the detected branch
Write-Host "Detected branch: $currentBranch"

# Define the valid branch for running the script
$validBranch = "development"

# Ensure the script only runs on the valid branch
if ($currentBranch -ne $validBranch) {
    Write-Host "The script is configured to run only on the '$validBranch' branch. Current branch: $currentBranch. Exiting."
    exit 0
}

# Read or initialize the VERSION file
if (Test-Path $versionFile) {
    $currentVersion = Get-Content $versionFile -Raw
    $currentVersion = $currentVersion.Trim()
} else {
    Write-Host "VERSION file not found. Initializing with version 1.0.0."
    Write-Output "1.0.0" > $versionFile
    $currentVersion = "1.0.0"
}

# Validate version format
if (-not ($currentVersion -match '^\d+\.\d+\.\d+$')) {
    Write-Error "Invalid version format in VERSION file: $currentVersion. Ensure it follows the format MAJOR.MINOR.PATCH (e.g., 1.0.0)."
    exit 1
}

# Split the version into parts
$versionParts = $currentVersion -split '\.'
$major = [int]$versionParts[0]
$minor = [int]$versionParts[1]
$patch = [int]$versionParts[2]

# Read the latest commit message
$commitMessage = git log -1 --pretty=%B

# Debug: Output the commit message for visibility
Write-Host "Commit Message: $commitMessage"

# Check if the commit is a merge commit
if ($commitMessage -match "^Merged PR") {
    # Extract PR title and check for keywords
    $prTitle = ($commitMessage -split ':', 2)[1].Trim()
    Write-Host "Detected PR title: $prTitle"

    if ($prTitle -match "\badd\b") {
        $minor++
        $patch = 0  # Reset patch when minor is incremented
        Write-Host "Incrementing minor version. New values: Minor=$minor, Patch=$patch"
    } elseif ($prTitle -match "\bfix\b") {
        $patch++
        Write-Host "Incrementing patch version. New value: Patch=$patch"
    } else {
        Write-Host "No recognized keywords in PR title. No version increment applied."
        exit 0
    }
} else {
    # Check for version increment keywords in the commit message
    if ($commitMessage -match "\badd\b") {
        $minor++
        $patch = 0  # Reset patch when minor is incremented
        Write-Host "Incrementing minor version. New values: Minor=$minor, Patch=$patch"
    } elseif ($commitMessage -match "\bfix\b") {
        $patch++
        Write-Host "Incrementing patch version. New value: Patch=$patch"
    } else {
        Write-Host "No recognized keywords in commit message. No version increment applied."
        exit 0
    }
}

# Construct the new version
$newVersion = "$major.$minor.$patch"

# Update the VERSION file
try {
    Set-Content -Path $versionFile -Value $newVersion
    Write-Host "Updated version to $newVersion in $versionFile"
} catch {
    Write-Error "Failed to update VERSION file: $_"
    exit 1
}

# Update Angular package.json
if (-not (Test-Path $packageJsonPath)) {
    Write-Error "package.json not found at $packageJsonPath. Cannot update Angular version."
    exit 1
}

try {
    $packageJson = Get-Content $packageJsonPath -Raw | ConvertFrom-Json
    $packageJson.version = $newVersion

    # Convert to formatted JSON with proper indentation
    $formattedJson = $packageJson | ConvertTo-Json -Depth 10 | Out-String
    Set-Content -Path $packageJsonPath -Value $formattedJson -Encoding UTF8

    Write-Host "Updated Angular app version to $newVersion in package.json"
} catch {
    Write-Error "Failed to update package.json: $_"
    exit 1
}

# Configure Git user identity
git config user.name "Groep 5"
git config user.email "version@team-luna.com"

# Commit and tag the new version
try {
    git add $versionFile
    git add $packageJsonPath
    git commit -m "chore(version): Bumped version to $newVersion [auto-version] [ci:skip]"
    git tag "v$newVersion"
    Write-Host "Successfully committed and tagged the new version: v$newVersion"
} catch {
    Write-Error "Failed to commit or tag the new version: $_"
    exit 1
}

# Push the changes
try {
    git push origin HEAD:$currentBranch
    git push origin --tags
    Write-Host "Successfully pushed changes and tags to the remote repository."
} catch {
    Write-Error "Failed to push changes to the remote repository: $_"
    exit 1
}