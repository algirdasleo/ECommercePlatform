# Ecommerce Platform

- This project is an ecommerce platform built as a microservices architecture using **.NET 6**. It utilizes various microservices to manage different aspects of the ecommerce process, such as inventory, orders, payments.
    
## Architecture

The platform is divided into the following microservices:

- `Inventory`: Manages product inventory.
- `Notification`: Manages notifications and handles sending notifications to users with SignalR.
- `Order`: Manages order processing.
- `Payment`: Manages payments.
- `Product`: Manages product information.
- `User`: Manages user information.

Additionally, a `SharedLibrary` is used across services to share common code.

## Prerequisites

To run this project, you will need:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Microsoft Tye](https://github.com/dotnet/tye) (`dotnet tool install -g Microsoft.Tye --version "0.11.0-alpha.22111.1"`)
- [PostgreSQL](https://www.postgresql.org/download/) (for database services)
- Proper setup of the PostgreSQL command line tools (`psql`), accessible from your command line.

## Setup
1. **Clone the repository:**
   ```bash
   git clone https://github.com/algirdasleo/ecommerceplatform.git
   cd ecommerceplatform
   ```

2. **Database Setup:**
    - Navigate to the `DatabaseSetup` folder:
   ```bash
   cd DatabaseSetup
   ```

    - Follow the instructions in the `HowToUse.md` to set up databases using the scripts:
    - `mac_dbsetup.sh` for macOS users
    - `win_dbsetup.bat` for Windows users

3. **Configure the connection strings in appsettings.json for each microservice.**
    - For example, for the Notification service:
      

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost; Database=NotificationService; Username=your_username; Password=your_password"
      }
    }
    ```

    - Replace `your_username` and `your_password` with the actual username and password for your PostgreSQL instance.

4. **Build the solution:**

    ```
    dotnet build
    ```

5. **Run the services using Tye:**

    ```
    tye run
    ```

**Tye will start all microservices**, and the Tye dashboard will be available at **http://localhost:8000**, where you can see the status of all services.
