using Client.Dtos;
using Client.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Client.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5000/api/"); 
        }

        public async Task<List<DocumentDto>> GetDocumentsAsync()
        {
            var response = await _httpClient.GetAsync("documents");
            if (!response.IsSuccessStatusCode) return new List<DocumentDto>();
            return await response.Content.ReadFromJsonAsync<List<DocumentDto>>() ?? new List<DocumentDto>();
        }

        public async Task<List<ContractorDto>> GetContractorsAsync()
        {
            var response = await _httpClient.GetAsync("contractors");
            if (!response.IsSuccessStatusCode) return new List<ContractorDto>();
            return await response.Content.ReadFromJsonAsync<List<ContractorDto>>() ?? new List<ContractorDto>();
        }

        public async Task<bool> PostDocumentAsync(DocumentDto document)
        {
            var response = await _httpClient.PostAsJsonAsync("documents", document);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDocumentAsync(DocumentDto document)
        {
            if (document.Id <= 0)
                throw new ArgumentException("Document ID must be greater than zero for update.");

            var response = await _httpClient.PutAsJsonAsync($"documents/{document.Id}", document);
            return response.IsSuccessStatusCode;
        }

        public async Task AddContractorAsync(ContractorDto contractor)
        {
            var contractorExists = await GetContractorBySymbolAsync(contractor.Symbol);
            if (contractorExists != null)
            {
                throw new InvalidOperationException($"Contractor with symbol '{contractor.Symbol}' already exists.");
            }
            var response = await _httpClient.PostAsJsonAsync($"contractors", contractor);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateContractorAsync(ContractorDto contractor)
        {
            var response = await _httpClient.PutAsJsonAsync($"contractors/{contractor.Id}", contractor);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddDocumentAsync(DocumentDto document)
        {
            var response = await _httpClient.PostAsJsonAsync($"documents", document);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddDocumentItemAsync(DocumentItemDto item)
        {
            var response = await _httpClient.PostAsJsonAsync($"documentitems", item);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateDocumentItemAsync(DocumentItemDto item)
        {
            var response = await _httpClient.PutAsJsonAsync($"documentitems/{item.Id}", item);
            response.EnsureSuccessStatusCode();
        }
        public async Task<ContractorDto?> GetContractorBySymbolAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"contractors/by-symbol/{symbol}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ContractorDto>();
        }

        public async Task<DocumentDto?> GetDocumentBySymbolAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"documents/by-symbol/{symbol}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return await response.Content.ReadFromJsonAsync<DocumentDto>();
        }
        public async Task<DocumentDto> GetDocumentAsync(int documentId)
        {
            var response = await _httpClient.GetAsync($"documents/{documentId}");
            response.EnsureSuccessStatusCode();

            var document = await response.Content.ReadFromJsonAsync<DocumentDto>();
            return document!;
        }
    }
}
