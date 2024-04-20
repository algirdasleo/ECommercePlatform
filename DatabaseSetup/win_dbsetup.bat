@echo off
echo Please enter your PostgreSQL credentials:
set /p PGUSER="Enter your PostgreSQL username: "
set /p PGPASSWORD="Enter your PostgreSQL password: "

echo Creating databases...
psql -U %PGUSER% -W -c "CREATE DATABASE InventoryService;"
psql -U %PGUSER% -W -c "CREATE DATABASE NotificationService;"
psql -U %PGUSER% -W -c "CREATE DATABASE OrderService;"
psql -U %PGUSER% -W -c "CREATE DATABASE PaymentService;"
psql -U %PGUSER% -W -c "CREATE DATABASE ProductService;"
psql -U %PGUSER% -W -c "CREATE DATABASE UserService;"

echo Creating tables in InventoryService...
psql -U %PGUSER% -d InventoryService -W -c "
CREATE TABLE InventoryItems (
    InventoryItemId SERIAL PRIMARY KEY,
    ProductId INT NOT NULL,
    QuantityAvailable INT NOT NULL
);"

echo Creating tables in NotificationService...
psql -U %PGUSER% -d NotificationService -W -c "
CREATE TABLE Notifications (
    NotificationId SERIAL PRIMARY KEY,
    UserId INT NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    CreatedAt TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    SentAt TIMESTAMP WITHOUT TIME ZONE
);"

echo Creating tables in OrderService...
psql -U %PGUSER% -d OrderService -W -c "
CREATE TABLE Orders (
    OrderId SERIAL PRIMARY KEY,
    UserId INT NOT NULL,
    OrderDate TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    TotalAmount NUMERIC NOT NULL,
    OrderStatus INT NOT NULL
);"

echo Creating tables in PaymentService...
psql -U %PGUSER% -d PaymentService -W -c "
CREATE TABLE Payments (
    PaymentId SERIAL PRIMARY KEY,
    OrderId INT NOT NULL,
    Amount NUMERIC NOT NULL,
    PaymentDate TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    PaymentMethod VARCHAR(255) NOT NULL
);"

echo Creating tables in ProductService...
psql -U %PGUSER% -d ProductService -W -c "
CREATE TABLE Products (
    ProductId SERIAL PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL,
    Description TEXT,
    Price NUMERIC,
    CategoryId INT NOT NULL,
    CreatedAt TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP
);"

echo Creating tables in UserService...
psql -U %PGUSER% -d UserService -W -c "
CREATE TABLE Users (
    UserId SERIAL PRIMARY KEY,
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    RegistrationDate TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    UserType INT NOT NULL
);"

echo All databases and tables have been created successfully.
pause
