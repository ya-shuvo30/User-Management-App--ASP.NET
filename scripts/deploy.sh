#!/bin/bash

# Azure deployment script for User Management App

echo "🚀 Starting Azure deployment for User Management App..."

# Check if Azure CLI is installed
if ! command -v az &> /dev/null; then
    echo "❌ Azure CLI is not installed. Please install it first."
    exit 1
fi

# Check if user is logged in
if ! az account show &> /dev/null; then
    echo "🔐 Please login to Azure first:"
    az login
fi

# Set variables
RESOURCE_GROUP="rg-usermanagementapp"
LOCATION="eastus"
APP_NAME="usermanagementapp"
SQL_ADMIN_PASSWORD=$(openssl rand -base64 16)

echo "📦 Resource Group: $RESOURCE_GROUP"
echo "🌍 Location: $LOCATION"
echo "🔐 SQL Admin Password: [HIDDEN]"

# Create resource group
echo "📦 Creating resource group..."
az group create --name $RESOURCE_GROUP --location $LOCATION

# Deploy infrastructure
echo "🏗️ Deploying infrastructure..."
az deployment group create \
  --resource-group $RESOURCE_GROUP \
  --template-file infra/main.bicep \
  --parameters environmentName=$APP_NAME \
               location=$LOCATION \
               sqlAdminPassword=$SQL_ADMIN_PASSWORD

# Get the web app name
WEB_APP_NAME=$(az deployment group show \
  --resource-group $RESOURCE_GROUP \
  --name main \
  --query properties.outputs.webAppName.value -o tsv)

echo "🌐 Web App Name: $WEB_APP_NAME"

# Deploy the application
echo "📤 Deploying application..."
dotnet publish -c Release -o ./publish

# Create zip file for deployment
cd publish
zip -r ../app.zip .
cd ..

# Deploy to App Service
az webapp deployment source config-zip \
  --resource-group $RESOURCE_GROUP \
  --name $WEB_APP_NAME \
  --src app.zip

# Run database migrations
echo "🗄️ Running database migrations..."
# You'll need to run migrations manually or set up a deployment script

echo "✅ Deployment completed!"
echo "🌐 Your app is available at: https://$WEB_APP_NAME.azurewebsites.net"
echo "🔑 SQL Admin Password: $SQL_ADMIN_PASSWORD"
echo ""
echo "📝 Next steps:"
echo "1. Run database migrations"
echo "2. Configure custom domain (optional)"
echo "3. Set up SSL certificate (optional)"
echo "4. Configure monitoring and logging"
