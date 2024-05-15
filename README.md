# E-Commerce API

This repository contains the source code for an E-Commerce API developed using .NET 7. The API is designed following a 3-tier architecture and incorporates modern software design patterns and practices such as the Repository pattern, Unit of Work, and Dependency Injection. Authentication is handled via JWT (JSON Web Tokens).

## Architecture Overview

### 3-Tier Architecture

- **Presentation Layer:** RESTful API endpoints exposed to the frontend/client.
- **Business Logic Layer:** Core processing logic including services and managers.
- **Data Access Layer:** Interaction with the database, implemented using the Repository pattern and Unit of Work for efficient and maintainable data operations.

### Design Patterns

- **Repository Pattern:** Abstracts the data layer, providing a collection-oriented interface for accessing domain entities.
- **Unit of Work:** Maintains a list of transactions and coordinates the writing out of changes and the resolution of concurrency problems.
- **Dependency Injection:** Implemented using .NET Core's built-in IoC container, promoting loose coupling and greater testability.

### Authentication

- **JWT Authentication:** Secure endpoints using JWT, ensuring that only authenticated users can access certain resources.

## Technologies

- **.NET 7**: The latest release of Microsoft's cross-platform developer framework.
- **Entity Framework Core**: ORM for data access.
- **ASP.NET Core Identity**: For managing users, roles, and authentication.
- **Microsoft IdentityModel Tokens**: For handling JWT tokens.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.
