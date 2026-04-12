#!/bin/bash

# Define the version file
VERSION_FILE="./VERSION"

# Define the path to the Angular package.json
PACKAGE_JSON_PATH="./src/WebApp/package.json"

# Validate if Git is installed
if ! command -v git &>/dev/null; then
    echo "Error: Git is not installed or not available in PATH. Please install Git and try again."
    exit 1
fi

# Validate if the script is being run inside a Git repository
if [ ! -d ".git" ]; then
    echo "Error: The .git directory is not found in the current working directory. Please ensure you're in a valid Git repository."
    exit 1
fi

# Ensure jq is installed
if ! command -v jq &>/dev/null; then
    echo "jq is not installed. Attempting to install..."
    if command -v apt &>/dev/null; then
        sudo apt update && sudo apt install -y jq
    elif command -v yum &>/dev/null; then
        sudo yum install -y jq
    elif command -v brew &>/dev/null; then
        brew install jq
    else
        echo "Error: Could not determine package manager to install jq. Please install jq manually and try again."
        exit 1
    fi

    # Verify installation
    if ! command -v jq &>/dev/null; then
        echo "Error: jq installation failed. Please install jq manually and try again."
        exit 1
    fi
    echo "jq successfully installed."
fi

# Determine the current branch
CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD 2>/dev/null)
if [ -z "$CURRENT_BRANCH" ] || [ "$CURRENT_BRANCH" == "HEAD" ]; then
    # Handle detached HEAD state
    CURRENT_BRANCH=$(git describe --all --contains --always 2>/dev/null)
    echo "Running in a detached HEAD state. Using branch: $CURRENT_BRANCH"
fi

# Strip `remotes/origin/` prefix if present
CURRENT_BRANCH=${CURRENT_BRANCH#remotes/origin/}

# Debug the detected branch
echo "Detected branch: $CURRENT_BRANCH"

# Define the valid branch for running the script
VALID_BRANCH="development"

# Ensure the script only runs on the valid branch
if [ "$CURRENT_BRANCH" != "$VALID_BRANCH" ]; then
    echo "The script is configured to run only on the '$VALID_BRANCH' branch. Current branch: $CURRENT_BRANCH. Exiting."
    exit 0
fi

# Read or initialize the VERSION file
if [ -f "$VERSION_FILE" ]; then
    CURRENT_VERSION=$(cat "$VERSION_FILE" | tr -d '[:space:]')
else
    echo "VERSION file not found. Initializing with version 1.0.0."
    echo "1.0.0" > "$VERSION_FILE"
    CURRENT_VERSION="1.0.0"
fi

# Validate version format
if [[ ! "$CURRENT_VERSION" =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
    echo "Error: Invalid version format in VERSION file: $CURRENT_VERSION. Ensure it follows the format MAJOR.MINOR.PATCH (e.g., 1.0.0)."
    exit 1
fi

# Split the version into parts
IFS='.' read -r MAJOR MINOR PATCH <<< "$CURRENT_VERSION"

# Read the latest commit message
COMMIT_MESSAGE=$(git log -1 --pretty=%B)

# Determine the increment type based on the commit message
if [[ "$COMMIT_MESSAGE" =~ ^add ]]; then
    MINOR=$((MINOR + 1))
    PATCH=0  # Reset patch when minor is incremented
elif [[ "$COMMIT_MESSAGE" =~ ^fix ]]; then
    PATCH=$((PATCH + 1))
else
    echo "No recognized keywords in commit message. No version increment applied."
    exit 0
fi

# Construct the new version
NEW_VERSION="$MAJOR.$MINOR.$PATCH"

# Update the VERSION file
echo "$NEW_VERSION" > "$VERSION_FILE"
echo "Updated version to $NEW_VERSION in $VERSION_FILE"

# Update Angular package.json
if [ ! -f "$PACKAGE_JSON_PATH" ]; then
    echo "Error: package.json not found at $PACKAGE_JSON_PATH. Cannot update Angular version."
    exit 1
fi

# Use jq to update the package.json file
if command -v jq &>/dev/null; then
    jq --arg new_version "$NEW_VERSION" '.version = $new_version' "$PACKAGE_JSON_PATH" > "${PACKAGE_JSON_PATH}.tmp" && mv "${PACKAGE_JSON_PATH}.tmp" "$PACKAGE_JSON_PATH"
    echo "Updated Angular app version to $NEW_VERSION in package.json"
else
    echo "Error: jq is required to update package.json. Please install jq and try again."
    exit 1
fi

# Configure Git user identity
git config user.name "Groep 5"
git config user.email "version@team-luna.com"

# Commit and tag the new version
git add "$VERSION_FILE" "$PACKAGE_JSON_PATH"
if git commit -m "chore(version): Bumped version to $NEW_VERSION [ci:skip]"; then
    git tag "v$NEW_VERSION"
    echo "Successfully committed and tagged the new version: v$NEW_VERSION"
else
    echo "Error: Failed to commit the new version."
    exit 1
fi

# Push the changes
if git push origin HEAD:$CURRENT_BRANCH && git push origin --tags; then
    echo "Successfully pushed changes and tags to the remote repository."
else
    echo "Error: Failed to push changes to the remote repository."
    exit 1
fi