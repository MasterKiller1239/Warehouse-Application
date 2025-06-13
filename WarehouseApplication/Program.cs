using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data;

var builder = WebApplication.CreateBuilder(args);

// Dodanie kontrolerów
builder.Services.AddControllers();

// Po³¹czenie z PostgreSQL
builder.Services.AddDbContext<WarehouseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodanie AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Swagger (dokumentacja API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }