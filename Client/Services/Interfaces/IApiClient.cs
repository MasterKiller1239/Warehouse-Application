using Client.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IApiClient
    {
        Task<List<DocumentDto>> GetDocumentsAsync();
        Task<List<ContractorDto>> GetContractorsAsync();
        Task<bool> PostDocumentAsync(DocumentDto document);
        Task<bool> UpdateDocumentAsync(DocumentDto document);
        Task AddContractorAsync(ContractorDto contractor);
        Task UpdateContractorAsync(ContractorDto contractor);
        Task AddDocumentAsync(DocumentDto document);
        Task AddDocumentItemAsync(DocumentItemDto item);
        Task UpdateDocumentItemAsync(DocumentItemDto item);
        Task<ContractorDto?> GetContractorBySymbolAsync(string symbol);

        Task<DocumentDto?> GetDocumentBySymbolAsync(string symbol);
        Task<DocumentDto> GetDocumentAsync(int documentId);
    }
}
