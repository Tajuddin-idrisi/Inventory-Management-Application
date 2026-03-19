@echo off
setlocal

set API_PROJECT=InventoryManagement.API\InventoryManagement.API.csproj
set WEB_PROJECT=InventoryManagement.Web\InventoryManagement.Web.csproj

start "API" powershell -NoExit -Command "dotnet run --project '%API_PROJECT%' --urls 'https://localhost:5041;http://localhost:5040'"
start "Web" powershell -NoExit -Command "dotnet run --project '%WEB_PROJECT%' --urls 'https://localhost:5042;http://localhost:5043'"

echo Started API and Web projects.
pause
