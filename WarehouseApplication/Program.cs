using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data;
using WarehouseApplication.Services.Interfaces;
using WarehouseApplication.Services;
using WarehouseApplication.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Adds controller support (API endpoints)
builder.Services.AddControllers();

// Configures PostgreSQL database context and registers it with DI container
builder.Services.AddDbContext<IWarehouseContext, WarehouseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registers application services with scoped lifetime (one per HTTP request)
builder.Services.AddScoped<IContractorService, ContractorService>();
builder.Services.AddScoped<IDocumentItemService, DocumentItemService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

// Adds AutoMapper with profiles defined in the current assembly (Program)
builder.Services.AddAutoMapper(typeof(Program));

// Adds API documentation generators
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enables Swagger middleware to serve OpenAPI specification and UI
app.UseSwagger();
app.UseSwaggerUI();

// Enforces HTTPS redirection
app.UseHttpsRedirection();

// Enables authorization middleware (can be configured with policies)
app.UseAuthorization();

// Maps controller routes to endpoints (e.g., [Route("api/[controller]")])
app.MapControllers();

// Starts the web application
app.Run();

// Required for integration testing with WebApplicationFactory in tests
public partial class Program { }
