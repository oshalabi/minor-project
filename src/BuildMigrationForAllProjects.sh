#!/bin/bash

# Parse arguments
name=""

while [[ $# -gt 0 ]]; do
    key="$1"
    case $key in
        --name)
            name="$2"
            shift # past argument
            shift # past value
            ;;
        *)
            echo -e "\e[31mUnknown argument: $1\e[0m"
            exit 1
            ;;
    esac
done

# Check if name is provided
if [ -z "$name" ]; then
    echo -e "\e[31mError: Migration name is required.\e[0m"
    echo "Usage: ./BuildMigrationForAllProjects.sh --name <migration_name>"
    exit 1
fi

# Get the current directory as the solution directory
SolutionDir=$(pwd)

# Define project directories
ProjectDirs=(
    "$SolutionDir/BasalRation"
    "$SolutionDir/EnergyFood"
    "$SolutionDir/Ration"
)

# Iterate over the project directories
for ProjectDir in "${ProjectDirs[@]}"; do
    # Check if the directory exists
    if [ ! -d "$ProjectDir" ]; then
        echo -e "\e[31mProject directory does not exist: $ProjectDir\e[0m"
        continue
    fi

    # Navigate to the project directory
    cd "$ProjectDir" || continue

    # Run the migration command
    echo -e "\e[32mCreating migration: $name in $ProjectDir\e[0m"
    if dotnet ef migrations add "$name"; then
        echo -e "\e[32mMigration '$name' created successfully in $ProjectDir.\e[0m"
    else
        echo -e "\e[31mError creating migration in $ProjectDir.\e[0m"
    fi
done

# Navigate back to the solution directory
cd "$SolutionDir" || exit

echo -e "\e[32mDone!\e[0m"