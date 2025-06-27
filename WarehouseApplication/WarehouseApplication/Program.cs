using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Extensions;
using WarehouseApplication.Services;
using WarehouseApplication.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Adds controller support (API endpoints)
builder.Services.AddControllers();

// Configures PostgreSQL database context and registers it with DI container
builder.Services.AddDbContext<IWarehouseContext, WarehouseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

// Registers application services with scoped lifetime (one per HTTP request)
builder.Services.AddScoped<IContractorService, ContractorService>();
builder.Services.AddScoped<IDocumentItemService, DocumentItemService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

// Adds AutoMapper with profiles defined in the current assembly (Program)
builder.Services.AddAutoMapper(typeof(Program));
builder.WebHost.UseUrls("http://+:5000");
// Adds API documentation generators
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5000);
    });
    builder.WebHost.UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
      
    });
}
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{

    app.ApplyMigrations();
}

//Enables Swagger middleware to serve OpenAPI specification and UI
app.UseSwagger();
app.UseSwaggerUI();
// Enforces HTTPS redirection
//app.UseHttpsRedirection();

// Enables authorization middleware (can be configured with policies)
app.UseAuthorization();

// Maps controller routes to endpoints (e.g., [Route("api/[controller]")])
app.MapControllers();

// Starts the web application
app.Run();

// Required for integration testing with WebApplicationFactory in tests
public partial class Program { }
