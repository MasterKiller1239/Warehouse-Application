using AutoMapper;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;
using WarehouseApplication.Repositories.Interfaces;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Services
{
    public class ContractorService : IContractorService
    {
        private readonly IContractorRepository _repository;
        private readonly IMapper _mapper;

        public ContractorService(IContractorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContractorDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ContractorDto>>(entities);
        }

        public async Task<ContractorDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ContractorDto>(entity);
        }

        public async Task<ContractorDto?> GetBySymbolAsync(string symbol)
        {
            var entity = await _repository.GetBySymbolAsync(symbol);
            return entity == null ? null : _mapper.Map<ContractorDto>(entity);
        }

        public async Task<ContractorDto> CreateAsync(ContractorDto dto)
        {
            var entity = _mapper.Map<Contractor>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<ContractorDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ContractorDto dto)
        {
            if (id != dto.Id)
                return false;

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return false;

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
