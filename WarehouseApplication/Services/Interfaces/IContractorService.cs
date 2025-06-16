using Microsoft.AspNetCore.Mvc;
using WarehouseApplication.Dtos;

namespace WarehouseApplication.Services.Interfaces
{
    public interface IContractorService
    {
        Task<IEnumerable<ContractorDto>> GetAllAsync();
        Task<ContractorDto?> GetByIdAsync(int id);
        Task<ContractorDto> CreateAsync(ContractorDto dto);
        Task<bool> UpdateAsync(int id, ContractorDto dto);
        Task<ContractorDto?> GetBySymbolAsync(string symbol);
    }

}