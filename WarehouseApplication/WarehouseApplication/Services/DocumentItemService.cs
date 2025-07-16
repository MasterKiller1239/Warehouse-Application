using AutoMapper;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;
using WarehouseApplication.Repositories.Interfaces;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Services
{
    public class DocumentItemService : IDocumentItemService
    {
        private readonly IDocumentItemRepository _repository;
        private readonly IMapper _mapper;

        public DocumentItemService(IDocumentItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocumentItemDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DocumentItemDto>>(items);
        }

        public async Task<DocumentItemDto?> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<DocumentItemDto>(item);
        }

        public async Task<IEnumerable<DocumentItemDto>> GetByDocumentIdAsync(int documentId)
        {
            var items = await _repository.GetByDocumentIdAsync(documentId);
            return _mapper.Map<IEnumerable<DocumentItemDto>>(items);
        }

        public async Task<DocumentItemDto> CreateAsync(DocumentItemDto dto)
        {
            var entity = _mapper.Map<DocumentItem>(dto);
            await _repository.CreateAsync(entity);
            return _mapper.Map<DocumentItemDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, DocumentItemDto dto)
        {
            if (id != dto.Id)
                return false;

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            _mapper.Map(dto, existing);
            return await _repository.UpdateAsync(existing);
        }
    }
}
