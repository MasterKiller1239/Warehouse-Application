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
                .Include(d => d.Contractor)
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
            var contractor = await _context.Contractors
                    .FirstOrDefaultAsync(c => c.Id == dto.ContractorId);

            if (contractor == null && dto.Contractor != null)
            {
                contractor = new Contractor
                {
                    Id = dto.ContractorId,
                    Name = dto.Contractor.Name,
                    Symbol = dto.Contractor.Symbol
                };
                _context.Contractors.Add(contractor);
            }
            else if (contractor != null && dto.Contractor != null)
            {
                entity.ContractorId = (int)contractor.Id;
                entity.Contractor = contractor;
            }
            else if (contractor == null)
            {
                throw new ArgumentException("Contractor not found and no contractor data provided");
            }
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
        public async Task<DocumentDto?> GetBySymbolAsync(string symbol)
        {
            var entity = await _context.Documents
                .Include(d => d.Contractor)
                .Include(d => d.Items)
                .FirstOrDefaultAsync(d => d.Symbol == symbol);
            if (entity == null)
                return null;
            return new DocumentDto
            {
                Id = entity.Id,
                Symbol = entity.Symbol,
                Date = entity.Date,
                ContractorId = entity.ContractorId,
                Contractor = new ContractorDto() { Name = entity.Contractor.Name, Symbol = entity.Contractor.Symbol },
                Items = entity.Items?.Select(i => new DocumentItemDto
                {
                    Id = i.Id,
                    DocumentId = i.DocumentId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Unit = i.Unit
                }).ToList() ?? new List<DocumentItemDto>()
            };
        }

    }

}
