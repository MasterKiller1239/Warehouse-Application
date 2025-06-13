using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WarehouseApplication.Dtos;
using Xunit;
namespace WarehouseApplication.Tests.Controllers;
public class ContractorsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContractorsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ShouldReturnOkAndList()
    {
        var response = await _client.GetAsync("/api/contractors");

        response.EnsureSuccessStatusCode();
        var contractors = await response.Content.ReadFromJsonAsync<List<ContractorDto>>();
        contractors.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_ShouldCreateContractor()
    {
        var contractor = new ContractorDto
        {
            Symbol = "ABC",
            Name = "Test Contractor"
        };

        var response = await _client.PostAsJsonAsync("/api/contractors", contractor);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Put_ShouldUpdateContractor()
    {
        // Najpierw dodaj nowego kontrahenta
        var contractor = new ContractorDto { Symbol = "XYZ", Name = "ToUpdate" };
        var postResponse = await _client.PostAsJsonAsync("/api/contractors", contractor);
        postResponse.EnsureSuccessStatusCode();

        // Pobierz listê kontrahentów i znajdŸ ID
        var getResponse = await _client.GetAsync("/api/contractors");
        var list = await getResponse.Content.ReadFromJsonAsync<List<ContractorDto>>();
        var created = list.Last();

        // Edytuj
        created.Name = "Updated Name";
        var putResponse = await _client.PutAsJsonAsync($"/api/contractors/{created.Id}", created);

        putResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}
