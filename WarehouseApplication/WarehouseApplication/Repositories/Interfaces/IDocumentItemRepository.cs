using WarehouseApplication.Models;

namespace WarehouseApplication.Repositories.Interfaces
{
    public interface IDocumentItemRepository
    {
        Task<IEnumerable<DocumentItem>> GetAllAsync();
        Task<DocumentItem?> GetByIdAsync(int id);
        Task<IEnumerable<DocumentItem>> GetByDocumentIdAsync(int documentId);
        Task CreateAsync(DocumentItem entity);
        Task<bool> UpdateAsync(DocumentItem entity);
    }
}
