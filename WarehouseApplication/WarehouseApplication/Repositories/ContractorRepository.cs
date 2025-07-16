using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Models;
using WarehouseApplication.Repositories.Interfaces;

namespace WarehouseApplication.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly IWarehouseContext _context;

        public ContractorRepository(IWarehouseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contractor>> GetAllAsync()
            => await _context.Contractors.ToListAsync();

        public async Task<Contractor?> GetByIdAsync(int id)
            => await _context.Contractors.FindAsync(id);

        public async Task<Contractor?> GetBySymbolAsync(string symbol)
            => await _context.Contractors.FirstOrDefaultAsync(c => c.Symbol == symbol);

        public Task AddAsync(Contractor contractor)
        {
            _context.Contractors.Add(contractor);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Contractor contractor)
        {
            _context.Contractors.Update(contractor);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
