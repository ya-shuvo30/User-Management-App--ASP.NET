@echo off
REM Azure deployment script for User Management App (Windows)

echo 🚀 Starting Azure deployment for User Management App...

REM Check if Azure CLI is installed
az --version >nul 2>&1
if errorlevel 1 (
    echo ❌ Azure CLI is not installed. Please install it first.
    exit /b 1
)

REM Check if user is logged in
az account show >nul 2>&1
if errorlevel 1 (
    echo 🔐 Please login to Azure first:
    az login
)

REM Set variables
set RESOURCE_GROUP=rg-usermanagementapp
set LOCATION=eastus
set APP_NAME=usermanagementapp
set SQL_ADMIN_PASSWORD=%RANDOM%%RANDOM%

echo 📦 Resource Group: %RESOURCE_GROUP%
echo 🌍 Location: %LOCATION%
echo 🔐 SQL Admin Password: [HIDDEN]

REM Create resource group
echo 📦 Creating resource group...
az group create --name %RESOURCE_GROUP% --location %LOCATION%

REM Deploy infrastructure
echo 🏗️ Deploying infrastructure...
az deployment group create ^
  --resource-group %RESOURCE_GROUP% ^
  --template-file infra/main.bicep ^
  --parameters environmentName=%APP_NAME% ^
               location=%LOCATION% ^
               sqlAdminPassword=%SQL_ADMIN_PASSWORD%

echo ✅ Infrastructure deployment completed!
echo 🔑 SQL Admin Password: %SQL_ADMIN_PASSWORD%
echo ""
echo 📝 Next steps:
echo 1. Build and publish your application: dotnet publish -c Release
echo 2. Deploy to App Service using Visual Studio or Azure CLI
echo 3. Run database migrations
echo 4. Test your application

pause
