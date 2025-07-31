# User Management App

An ASP.NET Core MVC application for managing users with registration, login, block/unblock/delete, and admin dashboard.

## Features

- User registration and login
- Admin dashboard for bulk block, unblock, delete
- Server-side search, filter, sort
- Bootstrap 5 styling
- Azure-ready deployment configuration

## Getting Started

### Local Development

1. **Clone the repository**
2. **Configure the database**  
   Update `appsettings.json` with your connection string.
3. **Run migrations**
    ```
    dotnet ef database update
    ```
4. **Run the application**
    ```
    dotnet run
    ```
5. **Access**
    - Register: `/Account/Register`
    - Login: `/Account/Login`
    - Dashboard: `/User/Index`

### Azure Deployment

#### Prerequisites
- Azure CLI installed and configured
- Azure subscription with appropriate permissions
- .NET 8.0 SDK

#### Deploy to Azure

1. **Login to Azure**
   ```bash
   az login
   ```

2. **Run deployment script**
   
   **Windows:**
   ```cmd
   scripts\deploy.bat
   ```
   
   **Linux/Mac:**
   ```bash
   chmod +x scripts/deploy.sh
   ./scripts/deploy.sh
   ```

3. **Manual deployment using Azure CLI**
   ```bash
   # Create resource group
   az group create --name rg-usermanagementapp --location eastus
   
   # Deploy infrastructure
   az deployment group create \
     --resource-group rg-usermanagementapp \
     --template-file infra/main.bicep \
     --parameters environmentName=usermanagementapp \
                  location=eastus \
                  sqlAdminPassword=YourSecurePassword123!
   
   # Build and publish
   dotnet publish -c Release
   
   # Deploy to App Service (get app name from deployment output)
   az webapp deployment source config-zip \
     --resource-group rg-usermanagementapp \
     --name [your-app-name] \
     --src publish.zip
   ```

#### Post-Deployment Steps

1. **Run database migrations** in Azure Cloud Shell or local CLI:
   ```bash
   dotnet ef database update --connection "your-azure-sql-connection-string"
   ```

2. **Configure application settings** in Azure Portal:
   - Set `AZURE_SQL_CONNECTION_STRING`
   - Set `ASPNETCORE_ENVIRONMENT` to `Production`

3. **Test the deployment** by accessing your Azure App Service URL

## Admin actions

- Select users and click Block/Unblock/Delete to update status.
- Search, filter, and sort users with the controls above the table.

## Architecture

- **Frontend**: ASP.NET Core MVC with Razor Pages
- **Backend**: Entity Framework Core with SQL Server
- **Authentication**: Cookie-based authentication
- **UI Framework**: Bootstrap 5.3.3
- **Cloud Platform**: Microsoft Azure
  - App Service for web hosting
  - Azure SQL Database for data storage
  - Managed Identity for secure connections

## Tech Stack

- ASP.NET Core 9.0 MVC
- Entity Framework Core 9.0
- Bootstrap 5.3.3
- Azure App Service
- Azure SQL Database
- Bicep (Infrastructure as Code)

## Project Structure

```
├── Controllers/          # MVC Controllers
├── Data/                # Entity Framework DbContext
├── Models/              # Data models
├── Views/               # Razor views
├── wwwroot/             # Static files
├── infra/               # Azure infrastructure (Bicep templates)
├── scripts/             # Deployment scripts
├── appsettings.json     # Local configuration
└── appsettings.Production.json  # Production configuration
```

## License

MIT
