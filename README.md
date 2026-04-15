# UserAuth
This project is a production-ready User Authentication REST API built with ASP.NET Core 8, following Onion Architecture and clean coding principles.

It provides secure and scalable authentication features including user registration, login, JWT-based authentication, and password management. The application is structured using Repository Pattern, Service Layer, and proper separation of concerns to ensure maintainability and extensibility.

Key Features:
- User Signup & Login
- JWT Authentication & Authorization
- Change Password (secured via JWT)
- Global Exception Handling Middleware
- Standard API Response Wrapper
- Input Validation (Data Annotations)
- Swagger Documentation with JWT support
- SQL Server integration using Entity Framework Core

Architecture:
- Domain Layer (Entities)
- Application Layer (Interfaces, DTOs, Business Logic)
- Infrastructure Layer (Database, Repositories, Services)
- API Layer (Controllers, Middleware)

This project is ideal for learning enterprise-level .NET API design and can be extended for real-world authentication systems.

Tech Stack:
- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger (OpenAPI)
