using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Models;
using WarehouseApplication.Repositories.Interfaces;

namespace WarehouseApplication.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IWarehouseContext _context;

        public DocumentRepository(IWarehouseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Documents
                .Include(d => d.Items)
                .Include(d => d.Contractor)
                .ToListAsync();
        }

        public async Task<Document?> GetByIdAsync(int id)
        {
            return await _context.Documents
                .Include(d => d.Items)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Document?> GetBySymbolAsync(string symbol)
        {
            return await _context.Documents
                .Include(d => d.Items)
                .Include(d => d.Contractor)
                .FirstOrDefaultAsync(d => d.Symbol == symbol);
        }

        public async Task CreateAsync(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Document document)
        {
            _context.Documents.Update(document);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
