# User Management App

An ASP.NET Core MVC application for managing users with registration, login, block/unblock/delete, and admin dashboard.

## Features

- User registration and login
- Admin dashboard for bulk block, unblock, delete
- Server-side search, filter, sort
- Bootstrap 5 styling

## Getting Started

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

## Admin actions

- Select users and click Block/Unblock/Delete to update status.
- Search, filter, and sort users with the controls above the table.

## Tech Stack

- ASP.NET Core MVC
- Entity Framework Core
- Bootstrap 5

## License

MIT
