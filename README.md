#  Hotel Management System (Mobile App & API)

A cross-platform mobile application and backend API built for daily hotel operations.

> **Status:** MVP is functional. The project is under development.

## Screenshots


## Tech Stack

**Backend (REST API):**
* C# .NET 
* Entity Framework Core (SQL Server)
* Architecture: Clean Architecture, CQRS (MediatR)
* Validation: FluentValidation

**Frontend (Mobile):**
* React Native
* React Native Paper (Material Design)
* Custom Hooks for automated form error mapping

**Infrastructure:**
* Docker & Docker Compose

## How to run locally

The entire backend infrastructure (API & Database) is fully containerized.

**Start the backend environment:**
   ```bash
   docker-compose up --build -d
   ```
** TO DO Features**

* Implement JWT Authentication & Role-Based Access Control
* Unit and Integration Testing 
* CI/CD Pipeline (GitLab)
