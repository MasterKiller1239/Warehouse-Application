using WarehouseApplication.Models;

namespace WarehouseApplication.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAllAsync();
        Task<Document?> GetByIdAsync(int id);
        Task<Document?> GetBySymbolAsync(string symbol);
        Task CreateAsync(Document entity);
        Task<bool> UpdateAsync(Document entity);
    }
}
