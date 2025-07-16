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

        public async Task<DocumentDto?> GetBySymbolAsync(string symbol)
        {
            var document = await _documentRepository.GetBySymbolAsync(symbol);
            return document == null ? null : _mapper.Map<DocumentDto>(document);
        }

        public async Task<DocumentDto> CreateAsync(DocumentDto dto)
        {
            var document = _mapper.Map<Document>(dto);

            var contractor = await _contractorRepository.GetByIdAsync(dto.ContractorId);

            if (contractor == null)
            {
                if (dto.Contractor == null)
                    throw new ArgumentException("Contractor not found and no contractor data provided");

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
                document.Contractor = contractor;
                document.ContractorId = (int)contractor.Id;
            }
            else if (contractor == null)
            {
                throw new ArgumentException("Contractor not found and no contractor data provided");
            }

            await _documentRepository.CreateAsync(document);

            return _mapper.Map<DocumentDto>(document);
        }

        public async Task<bool> UpdateAsync(int id, DocumentDto dto)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null) return false;

            _mapper.Map(dto, document);

            return await _documentRepository.UpdateAsync(document);
        }
    }
}
