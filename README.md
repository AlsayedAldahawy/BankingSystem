
# Banking System 

A simple banking system application built with ASP.NET Core and Entity Framework Core. This project demonstrates the use of inheritance, database operations, and persistent ID generation.
This solution was developed to fulfill an entry-level task required by Paysky as part of their job application process.## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or another compatible database

### Setup

1. **Clone the repository**:
    ```bash
    git clone https://github.com/alsayedaldahawy/bankingsystem.git
    cd bankingsystem
    ```

2. **Configure the database connection string**:
   - Open `appsettings.json` and update the connection string to point to your database:
    ```json
    {
        "ConnectionStrings": {
            "MyConnection": "YourConnectionStringHere"
        }
    }
    ```

3. **Restore dependencies**:
    ```bash
    dotnet restore
    ```

4. **Apply database migrations**:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

5. **Run the application**:
    ```bash
    dotnet run
    ```

### Usage

- Navigate to `https://localhost:7095/api/accounts` to access the Accounts API.
- Navigate to `https://localhost:7095/api/transactions` to access the Transactions API.

### Example API Requests

#### Deposit
```bash
curl -X POST https://localhost:5001/api/accounts/deposit \
    -H "Content-Type: application/json" \
    -d '{   
        "accountId": 123,
        "amount": 100.0
    }'
