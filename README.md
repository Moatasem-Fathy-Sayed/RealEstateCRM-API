# 🏢 RealEstate CRM Enterprise API (.NET 9)

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![C# 13](https://img.shields.io/badge/C%23-13.0-239120?style=for-the-badge&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![EF Core 9](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://docs.microsoft.com/en-us/ef/core/)
[![Docker Ready](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker)](https://www.docker.com/)
[![Tests Passed](https://img.shields.io/badge/xUnit-Passed-4C1D95?style=for-the-badge&logo=xunit)](https://xunit.net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

A production-ready, enterprise-grade **Real Estate Customer Relationship Management (CRM) Backend System** built with **.NET 9** and **C# 13**. Designed following **Clean Architecture**, **Domain-Driven Design (DDD)** principles, and modern software design patterns to ensure high scalability, security, maintainability, and testability.

---

## 📐 System Architecture Diagram

The solution adheres strictly to **Clean Architecture** principles, enforcing clear separation of concerns across distinct layers:

                  ┌────────────────────────────────────────┐
                  │           RealEstateCRM.API            │
                  │  (Controllers, Middleware, Swagger)    │
                  └───────────────────┬────────────────────┘
                                      │
                                      ▼
                  ┌────────────────────────────────────────┐
                  │     RealEstateCRM.Infrastructure       │
                  │ (EF Core, Repositories, BackgroundSvc) │
                  └───────────────────┬────────────────────┘
                                      │
                                      ▼
                  ┌────────────────────────────────────────┐
                  │           RealEstateCRM.Core           │
                  │    (Entities, Interfaces, Enums, DTOs) │
                  └────────────────────────────────────────┘

---

## 📁 Project Directory Structure

```text
RealEstateCRM/
├── src/
│   ├── RealEstateCRM.Core/             # Domain Model, Entities, Interfaces, Specifications
│   ├── RealEstateCRM.Infrastructure/   # EF Core DbContext, Repositories, Migrations, Background Tasks
│   └── RealEstateCRM.API/              # Controllers, Extensions, Rate Limiting, Health Checks
├── tests/
│   └── RealEstateCRM.Tests/            # Unit & Integration Tests (xUnit, Moq, FluentAssertions)
├── Dockerfile                          # Multi-stage production container build definition
├── docker-compose.yml                  # Orchestration file for API & SQL Server 2022
└── RealEstateCRM.sln                   # Solution File
✨ Enterprise Features & Capabilities
🔐 1. Security & Authentication
JWT Bearer Token Authentication: Identity-based secure token generation and validation.

Role-Based Access Control (RBAC): Granular permission enforcement across API endpoints.

Rate Limiting: Protects endpoints against DoS abuse using Fixed Window Rate Limiters (429 Too Many Requests).

🏢 2. Core CRM Business Modules
Properties Management: Soft-delete enabled listing operations using EF Core Global Query Filters.

Leads & Interactions: Prospect tracking, lifecycle status, and sales communication logging.

Deals & Commission Rules: Deal settlement processing with automated financial commission calculations.

Appointments & Reminders: Scheduled viewing appointments integrated with background tasks.

⏱️ 3. Infrastructure & Resilience
Hosted Background Services: Automated period execution (AppointmentReminderService) using BackgroundService.

Audit Trail & Logging: Automatic entity state change capture (Creation, Updates, Soft Deletes).

Database Health Checks: Native monitoring endpoints (/health) verifying SQL Server connectivity.

Standardized Error Handling: RFC 7807 ProblemDetails exception handling middleware.

Performance Caching: Memory caching strategy (IMemoryCache) for optimized query reads.

Data Export: CSV reporting export engine powered by CsvHelper.

🛠️ Tech Stack & Dependencies
Category	Technology / Library
Framework	.NET 9.0 / C# 13
Architecture	Clean Architecture, DDD, Repository & Unit of Work
Database	SQL Server 2022, Entity Framework Core 9.0
Testing	xUnit, Moq, FluentAssertions, EF Core InMemory
Security	ASP.NET Core JWT Bearer, Rate Limiting Middleware
DevOps & Containers	Docker, Docker-Compose
API Documentation	Swagger / OpenAPI
🚀 Quick Start Guide
Option A: Run via Docker Compose (Recommended)
Clone the repository:

Bash
git clone [https://github.com/YourUsername/RealEstateCRM-API.git](https://github.com/YourUsername/RealEstateCRM-API.git)
cd RealEstateCRM-API
Spin up the API and SQL Server containers:

Bash
docker-compose up -d --build
Open Swagger UI in your browser:
http://localhost:5000/swagger

Option B: Run via .NET CLI
Update the SQL Server Connection String in RealEstateCRM.API/appsettings.json.

Apply EF Core Migrations:

Bash
dotnet ef database update --project RealEstateCRM.Infrastructure --startup-project RealEstateCRM.API
Run the application:

Bash
dotnet run --project RealEstateCRM.API
Run Unit Tests:

Bash
dotnet test RealEstateCRM.Tests
📝 License
Distributed under the MIT License. See LICENSE for more information.