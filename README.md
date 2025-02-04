# User Management Project

A **.NET-based** system for managing users, roles, books, and categories. The solution follows a **Clean Architecture** approach with **CQRS (Command Query Responsibility Segregation)** using **MediatR** for business logic, **FluentValidation** for validation, and **EF Core with PostgreSQL** for persistence. The project is structured into multiple layers to ensure maintainability and separation of concerns.


## Table of Contents

- [Project Overview](#project-overview)
- [Prerequisites](#prerequisites)
- [Installation and Setup](#installation-and-setup)
- [Project Layout](#project-layout)
- [Business Rules and Constraints](#business-rules-and-constraints)
- [Testing](#testing)

## Project Overview
User Management Project is a **modular** solution designed to handle user, role, book, and category management efficiently. It enforces strict business rules and constraints to maintain consistency in data handling.

- **Follows Clean Architecture:** Keeps business logic separate from infrastructure concerns.
- **Implements CQRS:** Uses **Commands** for write operations and **Queries** for read operations.
- **Leverages MediatR:** Decouples request handling logic.
- **Uses FluentValidation:** Ensures input validation at the application layer.
- **Built on PostgreSQL & EF Core:** Uses relational database storage with **Entity Framework Core**.


## Prerequisites

Before running the project, make sure you have the following installed:

- **.NET 8+**
- **PostgreSQL**
- **Docker** 

## Installation and Setup
### Solution
1. Clone the repository:   
    ```bash
    git clone https://github.com/mouoent/usermanagementproject.git
    cd UserManagementProject
    ```
2. Restore NuGet packages:
    ```bash
    dotnet restore
    ```

### Using Docker for PostgreSQL
By default, the DefaultConnection connection string in **appsettings.json** is set to the PostgreSQL database instance that will be set up in the below Docker container. If you wish to use your own database; change the DefaultConnectionString accordingly. 

```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "Host=localhost;Port=5432;Database=UserManagementDb;Username=postgres;Password=yourpassword"
        }
    }
```

**Note:** The database schema is optimized for PostgreSQL (e.g., all table names are in lowercase).

1. Download and install [Docker](https://docs.docker.com/desktop/setup/install/windows-install/)
2. Navigate to project root:
    ```bash
    cd UserManagementProject
    ```
3. Run Docker containers
    ```bash
    docker-compose up --build
    ```
4. Check if PostgreSQL is running:
    ```bash
    docker ps
    ```

## Database Migrations
1. Apply the database migration:
    ```bash
    dotnet ef database update --project UserManagementProject.Infrastructure --startup-project UserManagementProject.API

    ```
    
2. Create new migration file (Optional):
    ```bash
    dotnet ef migrations add <MigrationName> --project UserManagementProject.Infrastructure

    ```

    A migration file has already been created for the current set of Entities, if you make any changes to them, you will have to create a new migration.

## Run project
2. Build project:
    ```bash
    dotnet build
    ```
2. Run project:
    ```bash
    dotnet run --project ./UserManagementProject/UserManagementProject.API.csproj
    ```    
3. Visit the API's Swagger documentation:
   [http://localhost:5173/swagger/index.html](http://localhost:5173/swagger/index.html)


## Project Layout
The solution is split into multiple projects for maintainability:

- **API**: Handles HTTP requests via controllers and configurations (appsettings.json for connection strings, API versioning, and other settings).
- **Application**: Holds all application logic, including CQRS features, command/query handlers, DTOs, validation, and dependency injection.
- **Domain**: Defines core domain logic such as entity models and enums.
- **Infrastructure**: Handles persistence, database configurations (EF Core with PostgreSQL), repositories, middleware, and logging.
- **Tests**: Unit tests ensuring business logic correctness.

        UserManagementProject
        ├── API
        │   ├── Controllers           
        │   └── Program.cs
        │   └── appsettings.json
        ├── Application
        │   ├── Features
        │   │   ├── Queries, QueryHandlers
        │   │   ├── Commands, CommandHandlers
        │   │   ├── DTOs
        │   │   └── Validators          
        │   ├── Interfaces
        │   └── DependencyInjection
        ├── Domain
        │   ├── Entities
        │   └── Enums
        ├── Infrastructure
        │   ├── Middlewares
        │   ├── Migrations
        │   ├── Persistence
        │   ├── Repositories
        │   ├── Services
        │   └── DependencyInjection
        └── Tests
            └── UnitTests             

## Features
This service supports the following operations:

- ✅ **Create User**
- ✅ **Get All Books**
- ✅ **Update User Role**
- ✅ **Delete Category**
- *and  more*

## Business Rules and Constraints
- #### Users:
    - Must contain at least one `Role`.
    - Can contain multiple `Roles`.    
- #### Roles:
    - `Roles` are predefined and cannot be dynamically created or modified.
    - Multiple `Users` can have the same `Role`.
- #### Books:
    - Cannot exist without a `Category`.
- #### Categories:
    - A `Category` cannot be deleted if it is assigned to any existing `Book`. All related `Books` must be reassigned or removed before deletion.
    - Multiple `Books` can have the same `Category`.

### Testing
The solution includes unit tests for:
- Command and Query Handlers (CQRS)
- Validation Rules (FluentValidation)
- Repository and Service Layer
 ensuring core business logic and validations are covered. Tests are located in the `UserManagementProject/UserManagementProject.Tests/UnitTests` directory.

```bash
# Run all tests
dotnet test
```