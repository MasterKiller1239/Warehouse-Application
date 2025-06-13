using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WarehouseApplication.Dtos;
using Xunit;
namespace WarehouseApplication.Tests.Controllers;
public class DocumentsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DocumentsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ShouldReturnListOfDocuments()
    {
        // Act
        var response = await _client.GetAsync("/api/documents");

        // Assert
        response.EnsureSuccessStatusCode();
        var list = await response.Content.ReadFromJsonAsync<List<DocumentDto>>();
        list.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_ShouldCreateDocument()
    {
        // Arrange - first create a contractor
        var contractor = new ContractorDto { Symbol = "D-POST", Name = "Doc Contractor" };
        await _client.PostAsJsonAsync("/api/contractors", contractor);
        var contractors = await _client.GetFromJsonAsync<List<ContractorDto>>("/api/contractors");
        var contractorId = contractors.Last().Id;

        // Act
        var document = new DocumentDto
        {
            Date = DateTime.UtcNow,
            Symbol = "DOC-001",
            ContractorId = contractorId,
            Items = new List<DocumentItemDto>
            {
                new() { ProductName = "Test Item", Quantity = 5, Unit = "pcs" }
            }
        };

        var response = await _client.PostAsJsonAsync("/api/documents", document);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Put_ShouldUpdateDocument()
    {
        // Arrange - create contractor and document
        var contractor = new ContractorDto { Symbol = "D-PUT", Name = "Put Contractor" };
        await _client.PostAsJsonAsync("/api/contractors", contractor);
        var contractors = await _client.GetFromJsonAsync<List<ContractorDto>>("/api/contractors");
        var contractorId = contractors.Last().Id;

        var document = new DocumentDto
        {
            Date = DateTime.UtcNow,
            Symbol = "PUT-001",
            ContractorId = contractorId,
            Items = new List<DocumentItemDto>()
        };

        await _client.PostAsJsonAsync("/api/documents", document);
        var documents = await _client.GetFromJsonAsync<List<DocumentDto>>("/api/documents");
        var created = documents.Last();
        created.Symbol = "PUT-UPDATED";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/documents/{created.Id}", created);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}
