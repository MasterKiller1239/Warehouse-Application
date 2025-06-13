using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IWarehouseContext _context;
        private readonly IMapper _mapper;

        public DocumentService(IWarehouseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocumentDto>> GetAllAsync()
        {
            var documents = await _context.Documents
                .Include(d => d.Items)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DocumentDto>>(documents);
        }

        public async Task<DocumentDto?> GetByIdAsync(int id)
        {
            var document = await _context.Documents
                .Include(d => d.Items)
                .FirstOrDefaultAsync(d => d.Id == id);

            return document == null ? null : _mapper.Map<DocumentDto>(document);
        }

        public async Task<DocumentDto> CreateAsync(DocumentDto dto)
        {
            var entity = _mapper.Map<Document>(dto);
            _context.Documents.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<DocumentDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, DocumentDto dto)
        {
            var entity = await _context.Documents.Include(d => d.Items).FirstOrDefaultAsync(d => d.Id == id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
