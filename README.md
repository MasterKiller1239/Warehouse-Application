📦 WarehouseApplication – ASP.NET Core Web API for Warehouse Management

🧾 Description

WarehouseApplication is a web-based inventory management system built with ASP.NET Core Web API and PostgreSQL. It provides a clean and scalable backend architecture for managing warehouse documents, contractors, and individual items within goods receipts.

This project showcases good software engineering practices like Clean Architecture, SOLID principles, AutoMapper, EF Core with interfaces for testability, and Swagger for API documentation.

🛠️ Tech Stack

.NET 8 / ASP.NET Core Web API

PostgreSQL (via Entity Framework Core)

AutoMapper (for mapping between DTOs and Entities)

XUnit + Moq + FluentAssertions (for unit testing)

Swagger / Swashbuckle (for API documentation)

Clean Architecture principles

Dependency Injection

📚 Main Features

CRUD operations for:

Contractors

Documents (e.g. delivery receipts)

Document items (products inside documents)

Structured DTO usage to separate domain and transport layers

Testable architecture – logic extracted into services/interfaces for unit testing

Separation of concerns:

Controllers → thin, responsible for HTTP

Services → business logic

Repositories/DbContext → data access

WarehouseApplication/

│

├── Controllers/              # API endpoints (Documents, Contractors, DocumentItems)

├── Services/                 # Business logic (DocumentService, ContractorService etc.)

│   └── Interfaces/           # Interfaces for services (for DI and testing)

├── Data/                     # EF Core DbContext and interfaces

│   └── Interfaces/           # IWarehouseContext abstraction

├── Dtos/                     # Data Transfer Objects

├── Models/                   # Entity classes (mapped to database)

├── Tests/                    # Unit tests (XUnit, Moq)

│   └── Mocks/                # Mocked DbSets for test isolation

└── Program.cs                # Main entry point, DI setup and middleware


✅ Sample API Endpoints

Endpoint	Method	Description

/api/contractors	GET	List all contractors

/api/contractors/{id}	GET	Get contractor by ID

/api/documents	GET	List all documents with items

/api/documents/{id}	GET	Get document by ID with items

/api/documentitems/by-document/{id}	GET	List items for a specific document

/api/documentitems	POST	Add a document item


🧪 Testing
All controllers are unit-tested using:

XUnit – testing framework

Moq – mocking dependencies

FluentAssertions – for expressive assertions

Test coverage includes:

Get endpoints

Valid/invalid scenarios

Entity-to-DTO mapping verification
