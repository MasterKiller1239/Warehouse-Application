using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Models;
using WarehouseApplication.Repositories.Interfaces;

namespace WarehouseApplication.Repositories
{
    public class DocumentItemRepository : IDocumentItemRepository
    {
        private readonly IWarehouseContext _context;

        public DocumentItemRepository(IWarehouseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentItem>> GetAllAsync()
        {
            return await _context.DocumentItems.ToListAsync();
        }

        public async Task<DocumentItem?> GetByIdAsync(int id)
        {
            return await _context.DocumentItems.FindAsync(id);
        }

        public async Task<IEnumerable<DocumentItem>> GetByDocumentIdAsync(int documentId)
        {
            return await _context.DocumentItems
                                 .Where(di => di.DocumentId == documentId)
                                 .ToListAsync();
        }

        public async Task CreateAsync(DocumentItem entity)
        {
            _context.DocumentItems.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(DocumentItem entity)
        {
            _context.DocumentItems.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
