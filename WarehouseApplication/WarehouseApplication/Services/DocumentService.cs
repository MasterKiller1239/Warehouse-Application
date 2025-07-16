using AutoMapper;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;
using WarehouseApplication.Repositories.Interfaces;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IContractorRepository _contractorRepository;
        private readonly IMapper _mapper;

        public DocumentService(
            IDocumentRepository documentRepository,
            IContractorRepository contractorRepository,
            IMapper mapper)
        {
            _documentRepository = documentRepository;
            _contractorRepository = contractorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocumentDto>> GetAllAsync()
        {
            var documents = await _documentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DocumentDto>>(documents);
        }

        public async Task<DocumentDto?> GetByIdAsync(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            return document == null ? null : _mapper.Map<DocumentDto>(document);
        }

        public async Task<DocumentDto> CreateAsync(DocumentDto dto)
        {
            var entity = _mapper.Map<Document>(dto);

            var contractor = await _contractorRepository.GetByIdAsync(dto.ContractorId);

            if (contractor == null && dto.Contractor != null)
            {
                contractor = new Contractor
                {
                    Id = dto.ContractorId,
                    Name = dto.Contractor.Name,
                    Symbol = dto.Contractor.Symbol
                };
                await _contractorRepository.AddAsync(contractor);
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

            await _documentRepository.CreateAsync(entity);
            return _mapper.Map<DocumentDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, DocumentDto dto)
        {
            var entity = await _documentRepository.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);

            return await _documentRepository.UpdateAsync(entity);
        }

        public async Task<DocumentDto?> GetBySymbolAsync(string symbol)
        {
            var entity = await _documentRepository.GetBySymbolAsync(symbol);
            if (entity == null) return null;

            return _mapper.Map<DocumentDto>(entity);
        }
    }
}
