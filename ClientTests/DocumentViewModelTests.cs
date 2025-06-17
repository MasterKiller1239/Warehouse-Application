using Client.Dtos;
using Client.Services.Interfaces;
using Moq;
using FluentAssertions;
namespace Client.Tests.ViewModels.Documents
{
    public class DocumentViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly EditDocumentViewModel _viewModel;

        public DocumentViewModelTests()
        {
            var app = new System.Windows.Application();
            _apiClientMock = new Mock<IApiClient>();
            var sampleDocument = new DocumentDto
            {
                Id = 1,
                Symbol = "DOC-001",
                Date = DateTime.UtcNow,
                ContractorId = 1,
                Contractor = new ContractorDto() { Id = 1, Name = "Sample Contractor" },
            };
           
            _viewModel = new EditDocumentViewModel(_apiClientMock.Object, sampleDocument);
        }

        [Fact]
        public async Task LoadContractorsAsync_ShouldPopulateContractorList_AndSelectMatching()
        {
            // Arrange
            var contractors = new List<ContractorDto>
            {
                new ContractorDto { Id = 1, Name = "Test Contractor" },
                new ContractorDto { Id = 2, Name = "Another Contractor" }
            };
            _apiClientMock.Setup(api => api.GetContractorsAsync())
                          .ReturnsAsync(contractors);

            // Act
            await _viewModel.LoadContractorsAsync();

            // Assert
            _viewModel.Contractors.Should().BeEquivalentTo(contractors);
            _viewModel.SelectedContractor?.Id.Should().Be(1);
        }

        [Fact]
        public async Task UpdateDocumentAsync_ShouldCallApiClient_WithUpdatedData()
        {
            // Arrange
            var contractor = new ContractorDto { Id = 2, Name = "Updated Contractor" };
            _viewModel.Document.Symbol = "UPDATED-DOC";
            _viewModel.Document.Date = new DateTime(2024, 2, 1);
            _viewModel.SelectedContractor = contractor;
            var contractors = new List<ContractorDto>
            {
                new ContractorDto { Id = 1, Name = "Test Contractor" },
                new ContractorDto { Id = 2, Name = "Another Contractor" }
            };
            _apiClientMock.Setup(api => api.GetContractorsAsync())
                          .ReturnsAsync(contractors);

            // Act
            await _viewModel.UpdateDocumentAsync();

            // Assert
            _apiClientMock.Verify(api => api.UpdateDocumentAsync(It.Is<DocumentDto>(d =>
                d.Symbol == "UPDATED-DOC" &&
                d.Date == new DateTime(2024, 2, 1) &&
                d.ContractorId == 2 &&
                d.Contractor.Name == contractor.Name
            )), Times.Once);
        }

        //[Fact]
        //public void CanUpdate_ShouldReturnFalse_WhenSymbolIsEmptyOrContractorNotSelected()
        //{
        //    // Arrange
        //    _viewModel.Document.Symbol = " ";
        //    _viewModel.SelectedContractor = null;

        //    // Act
        //    var result = _viewModel.UpdateDocumentAsync();

        //    // Assert
        //    result.Should().NotBe(null);
        //}
    }
}
