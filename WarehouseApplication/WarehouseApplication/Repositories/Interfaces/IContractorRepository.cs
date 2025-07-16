using WarehouseApplication.Models;

namespace WarehouseApplication.Repositories.Interfaces
{
    public interface IContractorRepository
    {
        Task<IEnumerable<Contractor>> GetAllAsync();
        Task<Contractor?> GetByIdAsync(int id);
        Task<Contractor?> GetBySymbolAsync(string symbol);
        Task AddAsync(Contractor contractor);
        Task UpdateAsync(Contractor contractor);
        Task SaveChangesAsync();
    }
}
