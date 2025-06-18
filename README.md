# 📦 WarehouseApplication – Inventory Management System (.NET 8 + WPF + PostgreSQL)

## 🧾 Description

WarehouseApplication is a full-stack inventory management system consisting of:

- A clean and scalable ASP.NET Core Web API backend
- A modern WPF desktop client for managing contractors, warehouse documents, and items

The project demonstrates clean software architecture, testable layers, and a user-friendly desktop interface connected to a real-time backend.

## 🖥️ Client (WPF Desktop App)

The WPF client provides a desktop UI for managing:
- Contractors
- Documents (e.g. goods received)
- Items inside documents

It uses:
- MVVM architecture for testability and separation of concerns
- REST API integration via injected `IApiClient`
- A testable `IMessageService` abstraction (instead of MessageBox in unit tests)
- Command pattern (via `RelayCommand`) for all interactions
- Support for modal windows (Add/Edit) via a central `IWindowFactory`

## 🛠️ Tech Stack

**Backend:**
- .NET 8 / ASP.NET Core Web API
- PostgreSQL (via Entity Framework Core)
- AutoMapper (for DTO ↔ Entity mapping)
- Swagger / Swashbuckle (for API docs)
- XUnit + Moq + FluentAssertions (unit tests)
- Clean Architecture + SOLID Principles
- Layered design (Controllers, Services, Repositories)

**Frontend (Client):**
- WPF (Windows Presentation Foundation)
- MVVM Pattern
- REST API integration via `IApiClient`
- ViewModel unit tests with mocked services
- `ICommand`, `INotifyPropertyChanged`, event-based dialogs

## 📚 Main Features

### ✅ Backend:
- CRUD for Contractors, Documents, and Document Items
- DTO-based transport layer
- Swagger UI for testing
- Testable and layered architecture

### ✅ Frontend (WPF Client):
- Manage contractors and goods receipts with a desktop interface
- Add/edit documents and items with modal dialogs
- UI testable logic via ViewModels
- Dependency injection support
- MessageBox replaced by `IMessageService` (mockable in tests)
- `IWindowFactory` for decoupled window creation

## 📁 Project Structure

**Backend – WarehouseApplication/**
Controllers/ # API endpoints
Services/ # Business logic
└── Interfaces/ # Service interfaces
Data/ # EF Core DbContext
└── Interfaces/ # IWarehouseContext abstraction
Dtos/ # DTOs
Models/ # Entity classes
Tests/ # XUnit, Moq-based unit tests
**Client – Client/**
ViewModels/ # MVVM ViewModels
Views/ # WPF Windows (Add/Edit)
Services/ # ApiClient and MessageService
Utilities/ # Helpers (e.g., RelayCommand)
Factories/ # IWindowFactory for modal dialogs
Tests/ # Unit tests for ViewModels

## 🔗 Sample API Endpoints

| Endpoint                              | Method | Description                      |
|---------------------------------------|--------|----------------------------------|
| `/api/contractors`                    | GET    | List all contractors            |
| `/api/contractors/{id}`               | GET    | Get contractor by ID            |
| `/api/documents`                      | GET    | List all documents              |
| `/api/documents/{id}`                 | GET    | Get document with items         |
| `/api/documentitems/by-document/{id}` | GET    | List items for a document       |
| `/api/documentitems`                  | POST   | Add an item to a document       |

## � Testing

**Backend:**
- ✅ XUnit for logic testing
- ✅ Moq for mocking services/repositories
- ✅ FluentAssertions for readable assertions
- ✅ Focus on services and controller logic

**Frontend:**
- ✅ ViewModel unit tests with Moq
- ✅ `IMessageService` to avoid MessageBox in tests
- ✅ Commands and `CanExecute` logic covered
