# 📦 WarehouseApplication – Inventory Management System (.NET 8 + WPF + PostgreSQL + Docker + Kubernetes)

## 🧾 Description

WarehouseApplication is a full-stack inventory management system consisting of:

- A clean and scalable ASP.NET Core Web API backend
- A modern WPF desktop client for managing contractors, warehouse documents, and items
- Containerized PostgreSQL database and API with optional Kubernetes deployment

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
- ✅ .NET 8 / ASP.NET Core Web API
- ✅ PostgreSQL (via Entity Framework Core)
- ✅ AutoMapper (for DTO ↔ Entity mapping)
- ✅ Docker (for containerized backend and database setup)
- ✅ Kubernetes (YAML manifests for deployment, PersistentVolumeClaims, ConfigMaps)
- ✅ Swagger / Swashbuckle (for API docs)
- ✅ XUnit + Moq + FluentAssertions (unit tests)
- ✅ Clean Architecture + SOLID Principles
- ✅ Layered design (Controllers, Services, Repositories)

**Frontend (Client):**
- ✅ WPF (Windows Presentation Foundation)
- ✅ MVVM Pattern
- ✅ REST API integration via `IApiClient`
- ✅ ViewModel unit tests with mocked services
- ✅ `ICommand`, `INotifyPropertyChanged`, event-based dialogs

## 📚 Main Features

### ✅ Backend:
- CRUD for Contractors, Documents, and Document Items
- DTO-based transport layer
- Docker support: easily run backend and PostgreSQL with docker-compose
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

**Frontend Windows**

![lbZmWwg70E](https://github.com/user-attachments/assets/3f563539-cc34-49aa-8e93-c2b818783f53)
![TdKeyUVP97](https://github.com/user-attachments/assets/cac3a040-c13e-4762-9d44-0fd60a15c2b7)
![c2AqeM22ci](https://github.com/user-attachments/assets/c8526b85-321d-45be-bdb0-543ad69060c1)
![image](https://github.com/user-attachments/assets/9b651a9a-6f96-4092-b288-2c2f1f2e3076)
![image](https://github.com/user-attachments/assets/11fc5c04-a904-4757-9a21-f04a3eace879)
![image](https://github.com/user-attachments/assets/d9ad8c2f-6ef4-4e8d-a586-54c0762ab944)
![image](https://github.com/user-attachments/assets/9e42ec37-a0d1-4b13-b0e9-9667b069275c)
![image](https://github.com/user-attachments/assets/017dbad0-2ae3-4a9c-a309-156f84628a2a)
![image](https://github.com/user-attachments/assets/3a4451eb-d9ed-4707-a01e-a7ec0fff58d7)
![image](https://github.com/user-attachments/assets/c91d7109-c6d8-4cd0-9704-34824fd31217)


