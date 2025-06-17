using Client.Dtos;
using Client.Services.Interfaces;
using Moq;

namespace Client.Tests.ViewModels.Documents
{
    public class EditDocumentViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly DocumentDto _testDocument;
        private readonly EditDocumentViewModel _viewModel;

        public EditDocumentViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _testDocument = new DocumentDto
            {
                Id = 2,
                Symbol = "DOC123",
                Date = DateTime.UtcNow,
                ContractorId = 2,
                Contractor = new ContractorDto() { Id = 1, Name = "Sample Contractor" },
            };

            _viewModel = new EditDocumentViewModel(_apiClientMock.Object, _testDocument);
        }

        [Fact]
        public void Constructor_InitializesPropertiesFromDocument()
        {
            Assert.Equal("DOC123", _viewModel.Document.Symbol);
            Assert.Equal(_testDocument.Date, _viewModel.Document.Date);
        }

        [Fact]
        public async Task LoadContractorsAsync_PopulatesContractors_AndSetsSelected()
        {
            // Arrange
            var contractors = new List<ContractorDto>
            {
                new ContractorDto { Id = 1, Name = "Other Contractor" },
                new ContractorDto { Id = _testDocument.ContractorId, Name = "Test Contractor" }
            };

            _apiClientMock.Setup(x => x.GetContractorsAsync()).ReturnsAsync(contractors);
            // Act
            await _viewModel.LoadContractorsAsync();

            // Assert
            Assert.Equal(2, _viewModel.Contractors.Count);
            Assert.NotNull(_viewModel.SelectedContractor);
            Assert.Equal(_testDocument.ContractorId, _viewModel.SelectedContractor.Id);
        }

        [Fact]
        public async Task UpdateDocumentAsync_CallsUpdate_WithCorrectValues()
        {
            // Arrange
            var newContractor = new ContractorDto
            {
                Id = 1,
                Name = "Updated Contractor"
            };

            _viewModel.Document.Symbol = "UPDATED123";
            _viewModel.Document.Date = DateTime.UtcNow.AddDays(1);
            _viewModel.SelectedContractor = newContractor;

            // Act
            await _viewModel.UpdateDocumentAsync();

            // Assert
            _apiClientMock.Verify(x => x.UpdateDocumentAsync(It.Is<DocumentDto>(d =>
                d.Symbol == "UPDATED123" &&
                d.Date == _viewModel.Document.Date &&
                d.ContractorId == newContractor.Id &&
                d.Contractor == newContractor
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateDocumentAsync_DoesNotCallApi_IfContractorNotSelected()
        {
            // Arrange
            _viewModel.SelectedContractor = null;

            // Act
            await _viewModel.UpdateDocumentAsync();

            // Assert
            _apiClientMock.Verify(x => x.UpdateDocumentAsync(It.IsAny<DocumentDto>()), Times.Never);
        }
    }
}
