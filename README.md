# Banking System API

A RESTful Banking API built with **ASP.NET Core** to demonstrate my backend development skills and understanding of modern .NET development practices.

This project focuses on building a clean, secure, and maintainable Web API rather than creating a production-ready banking system.

## Technologies

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- AutoMapper
- Swagger / OpenAPI

## What I Practiced

- RESTful API design
- Clean Architecture principles
- Dependency Injection
- Authentication & Authorization using JWT
- ASP.NET Core Identity
- Entity Framework Core with Code First
- DTOs and AutoMapper
- Global Exception Handling
- Repository Pattern
- API documentation with Swagger

## Additional Feature

This project also includes a **Minimal API** endpoint that retrieves a list of bank branches.

The endpoint uses **In-Memory Cache** to cache the response for **1 minute**, reducing unnecessary processing and demonstrating basic caching techniques in ASP.NET Core.

## Running the Project

Clone the repository:

```bash
git clone https://github.com/fariborzvrm/BankingSystem.git
```

Navigate to the project folder:

```bash
cd BankingSystem
```

Update the connection string in `appsettings.json`, then run:

```bash
dotnet ef database update
dotnet run
```

Swagger will be available at:

```
https://localhost:<port>/swagger
```

## About Me

I'm a Junior .NET Backend Developer with a strong interest in building clean, scalable APIs and continuously improving my software engineering skills.

I'm currently looking for opportunities where I can contribute, learn from experienced developers, and grow as a backend engineer.

---

Feel free to explore the code or reach out if you have any feedback.