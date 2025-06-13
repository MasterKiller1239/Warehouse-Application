using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WarehouseApplication.Dtos;
using Xunit;
namespace WarehouseApplication.Tests.Controllers;
public class DocumentItemsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DocumentItemsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_ShouldCreateItem()
    {
        // Arrange - create contractor and document first
        var contractor = new ContractorDto { Symbol = "I-POST", Name = "Item Contractor" };
        await _client.PostAsJsonAsync("/api/contractors", contractor);
        var contractorId = (await _client.GetFromJsonAsync<List<ContractorDto>>("/api/contractors")).Last().Id;

        var document = new DocumentDto
        {
            Date = DateTime.UtcNow,
            Symbol = "DOC-ITEM",
            ContractorId = contractorId,
        };
        await _client.PostAsJsonAsync("/api/documents", document);
        var documentId = (await _client.GetFromJsonAsync<List<DocumentDto>>("/api/documents")).Last().Id;

        // Act
        var item = new DocumentItemDto
        {
            ProductName = "Created Item",
            Quantity = 10,
            Unit = "kg",
            DocumentId = documentId
        };

        var response = await _client.PostAsJsonAsync("/api/documentitems", item);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Put_ShouldUpdateItem()
    {
        // Arrange
        var contractor = new ContractorDto { Symbol = "I-PUT", Name = "ItemPut Contractor" };
        await _client.PostAsJsonAsync("/api/contractors", contractor);
        var contractorId = (await _client.GetFromJsonAsync<List<ContractorDto>>("/api/contractors")).Last().Id;

        var document = new DocumentDto
        {
            Date = DateTime.UtcNow,
            Symbol = "DOC-PUTITEM",
            ContractorId = contractorId,
        };
        await _client.PostAsJsonAsync("/api/documents", document);
        var documentId = (await _client.GetFromJsonAsync<List<DocumentDto>>("/api/documents")).Last().Id;

        var item = new DocumentItemDto
        {
            ProductName = "To Be Updated",
            Quantity = 2,
            Unit = "pcs",
            DocumentId = documentId
        };
        await _client.PostAsJsonAsync("/api/documentitems", item);
        var documentList = await _client.GetFromJsonAsync<List<DocumentDto>>("/api/documents");
        var itemToUpdate = documentList.Last().Items.Last();
        itemToUpdate.ProductName = "Updated Name";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/documentitems/{itemToUpdate.Id}", itemToUpdate);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}
