#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

# Install dotnet-ef tool
echo "Installing dotnet-ef tool..."
(cd ../ && dotnet tool install --global dotnet-ef --version 8.0.11)

# Update database
echo "Updating database..."
(cd ../src/ShelfLayoutManager.Web && dotnet ef database update)

echo "Script completed successfully!"