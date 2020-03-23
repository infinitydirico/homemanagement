$currentDirectory = [System.IO.Directory]::GetCurrentDirectory()
$parentDirectory = [System.IO.Directory]::GetParent($currentDirectory).FullName
Set-Location $parentDirectory
dotnet run -p HomeManagement.Api.Identity\HomeManagement.Api.Identity.csproj