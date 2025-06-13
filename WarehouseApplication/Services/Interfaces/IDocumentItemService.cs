using Microsoft.AspNetCore.Mvc;
using WarehouseApplication.Dtos;

namespace WarehouseApplication.Services.Interfaces
{
    public interface IDocumentItemService
    {
        Task<IEnumerable<DocumentItemDto>> GetAllAsync();
        Task<DocumentItemDto?> GetByIdAsync(int id);
        Task<IEnumerable<DocumentItemDto>> GetByDocumentIdAsync(int documentId);
        Task<DocumentItemDto> CreateAsync(DocumentItemDto dto);
        Task<bool> UpdateAsync(int id, DocumentItemDto dto);
    }

}
