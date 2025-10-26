# ELKOOD ToDo API 

A professional To-Do management RESTful API built with ASP.NET Core, using Clean Architecture and JWT Authentication. Developed as part of the ELKOOD back-end developer recruitment process.

## Features

- **JWT Authentication** with role-based authorization (Owner/Guest)
- **Complete CRUD Operations** for ToDo items
- **Advanced Filtering & Searching** by category, priority, status
- **Pagination** for efficient data retrieval
- **AutoMapper** for object mapping
- **FluentValidation** for request validation
- **Entity Framework Core** with In-Memory Database
- **Docker Support** with docker-compose
- **Unit & Integration Tests** (100% passing)
- **Swagger Documentation**
- **Global Error Handling & Logging**
- **Clean Architecture** (Onion Architecture)

## Architecture

- **Clean Architecture** with 4-layer separation
- **Repository Pattern** with Dependency Injection
- **Role-Based Access Control** (RBAC)
- **JWT Bearer Authentication**

## 🛠️ Technologies

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQL Server (Docker) / In-Memory Database
- JWT Authentication
- AutoMapper
- FluentValidation
- xUnit, Moq, FluentAssertions
- Docker & Docker Compose

## Quick Start

### Using .NET CLI (Development)

```bash
# Clone the repository
git clone https://github.com/engosamahmoud/ELKOOD-ToDo-API.git
cd ELKOOD-ToDo-API

# Run the application
cd ELKOOD.ToDo.API
dotnet run
