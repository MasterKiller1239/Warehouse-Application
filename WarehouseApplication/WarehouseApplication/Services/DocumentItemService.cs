using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Services
{
    public class DocumentItemService : IDocumentItemService
    {
        private readonly IWarehouseContext _context;
        private readonly IMapper _mapper;

        public DocumentItemService(IWarehouseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocumentItemDto>> GetAllAsync()
        {
            var items = await _context.DocumentItems.ToListAsync();
            return _mapper.Map<IEnumerable<DocumentItemDto>>(items);
        }

        public async Task<DocumentItemDto?> GetByIdAsync(int id)
        {
            var item = await _context.DocumentItems.FindAsync(id);
            return item is null ? null : _mapper.Map<DocumentItemDto>(item);
        }

        public async Task<IEnumerable<DocumentItemDto>> GetByDocumentIdAsync(int documentId)
        {
            var items = await _context.DocumentItems
                .Where(i => i.DocumentId == documentId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DocumentItemDto>>(items);
        }

        public async Task<DocumentItemDto> CreateAsync(DocumentItemDto dto)
        {
            var entity = _mapper.Map<DocumentItem>(dto);
            _context.DocumentItems.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<DocumentItemDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, DocumentItemDto dto)
        {
            if (id != dto.Id) return false;

            var entity = await _context.DocumentItems.FindAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
