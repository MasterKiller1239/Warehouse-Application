using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data;

namespace WarehouseApplication.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using WarehouseContext dbContext =
            scope.ServiceProvider.GetRequiredService<WarehouseContext>();

        dbContext.Database.Migrate();
 
    }
}