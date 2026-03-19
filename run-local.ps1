# Start API and Web projects in separate PowerShell windows
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

$apiProj = Join-Path $scriptDir "InventoryManagement.API\InventoryManagement.API.csproj"
$webProj = Join-Path $scriptDir "InventoryManagement.Web\InventoryManagement.Web.csproj"

Write-Host "Starting InventoryManagement.API..."
Start-Process -FilePath "powershell" -ArgumentList "-NoExit","-Command","dotnet run --project `"$apiProj`" --urls \"https://localhost:5041;http://localhost:5040\"" -WorkingDirectory $scriptDir

Start-Sleep -Milliseconds 500

Write-Host "Starting InventoryManagement.Web..."
Start-Process -FilePath "powershell" -ArgumentList "-NoExit","-Command","dotnet run --project `"$webProj`" --urls \"https://localhost:5042;http://localhost:5043\"" -WorkingDirectory $scriptDir

Write-Host "Both projects started (in new PowerShell windows)."