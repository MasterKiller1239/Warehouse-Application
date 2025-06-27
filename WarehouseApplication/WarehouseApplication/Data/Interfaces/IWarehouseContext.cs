using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Models;

namespace WarehouseApplication.Data.Interfaces
{
    public interface IWarehouseContext
    {
        DbSet<Document> Documents { get; }
        DbSet<DocumentItem> DocumentItems { get; }
        DbSet<Contractor> Contractors { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
