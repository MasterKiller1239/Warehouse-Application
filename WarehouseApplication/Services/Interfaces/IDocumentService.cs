using WarehouseApplication.Dtos;

namespace WarehouseApplication.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentDto>> GetAllAsync();
        Task<DocumentDto?> GetByIdAsync(int id);
        Task<DocumentDto> CreateAsync(DocumentDto dto);
        Task<bool> UpdateAsync(int id, DocumentDto dto);
    }


}
