$ErrorActionPreference = "Stop"

$solution = "MangaStoreWeb.sln"
$publishFolder = "deployment/MangaStoreWeb"

Write-Host "Cleaning previous deployment folder..."
if (Test-Path $publishFolder) {
    Remove-Item $publishFolder -Recurse -Force
}

Write-Host "Restoring dependencies..."
dotnet restore $solution

Write-Host "Building project in Release mode..."
dotnet build $solution --configuration Release --no-restore

Write-Host "Running unit tests..."
dotnet test $solution --configuration Release --no-build

Write-Host "Publishing application..."
dotnet publish "MangaStoreWeb.csproj" --configuration Release --output $publishFolder --no-build

Write-Host "Deployment package created in: $publishFolder"