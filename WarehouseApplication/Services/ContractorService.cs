using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Services
{
    public class ContractorService : IContractorService
    {
        private readonly IWarehouseContext _context;
        private readonly IMapper _mapper;

        public ContractorService(IWarehouseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContractorDto>> GetAllAsync()
        {
            var entities = await _context.Contractors.ToListAsync();
            return _mapper.Map<IEnumerable<ContractorDto>>(entities);
        }

        public async Task<ContractorDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Contractors.FindAsync(id);
            if (entity == null) return null;
            return _mapper.Map<ContractorDto>(entity);
        }

        public async Task<ContractorDto> CreateAsync(ContractorDto dto)
        {
            var entity = _mapper.Map<Contractor>(dto);
            _context.Contractors.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ContractorDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ContractorDto dto)
        {
            if (id != dto.Id) return false;

            var entity = await _context.Contractors.FindAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
