using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Documents;
using Moq;
using System.Windows.Data;
using Client.Services.Interfaces.IFactories;

namespace Client.Tests.ViewModels
{
    public class DocumentsViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly Mock<IAddDocumentViewModelFactory> _addDocVmFactoryMock;
        private readonly Mock<IEditDocumentViewModelFactory> _editDocVmFactoryMock;
        private readonly Mock<IDocumentDetailsViewModelFactory> _docDetailsVmFactoryMock;
        private readonly DocumentsViewModel _viewModel;
        private readonly List<DocumentDto> _mockDocuments;

        public DocumentsViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _messageServiceMock = new Mock<IMessageService>();
            _addDocVmFactoryMock = new Mock<IAddDocumentViewModelFactory>();
            _editDocVmFactoryMock = new Mock<IEditDocumentViewModelFactory>();
            _docDetailsVmFactoryMock = new Mock<IDocumentDetailsViewModelFactory>();

            _mockDocuments = new List<DocumentDto>
            {
                new DocumentDto { Id = 1, Symbol = "ABC123", ContractorId = 1 },
                new DocumentDto { Id = 2, Symbol = "XYZ456", ContractorId = 2 },
                new DocumentDto { Id = 3, Symbol = "abc789", ContractorId = 1 }
            };

            _apiClientMock.Setup(a => a.GetDocumentsAsync()).ReturnsAsync(_mockDocuments);
            _apiClientMock.Setup(a => a.GetContractorsAsync()).ReturnsAsync(new List<ContractorDto>());

            _viewModel = new DocumentsViewModel(
                _apiClientMock.Object,
                _messageServiceMock.Object,
                _addDocVmFactoryMock.Object,
                _editDocVmFactoryMock.Object,
                _docDetailsVmFactoryMock.Object);
        }

        [Fact]
        public async Task LoadDocumentsCommand_ShouldLoadDocumentsIntoCollection()
        {
            // Act
            await _viewModel.LoadDocumentsAsync();

            // Assert
            Assert.Equal(3, _viewModel.Documents.Count);
            _apiClientMock.Verify(x => x.GetDocumentsAsync(), Times.Once);
        }

        [Fact]
        public async Task ApplyFilter_ShouldFilterDocumentsCaseInsensitive()
        {
            // Arrange
            await _viewModel.LoadDocumentsAsync();
            _viewModel.SearchText = "abc";

            // Act
            _viewModel.ApplyFilter();

            // Assert
            Assert.Equal(2, _viewModel.Documents.Count);
            Assert.Contains(_viewModel.Documents, d => d.Symbol == "ABC123");
            Assert.Contains(_viewModel.Documents, d => d.Symbol == "abc789");
        }

        [Fact]
        public void SelectedDocument_WhenChanged_ShouldUpdateCommandStates()
        {
            // Arrange
            var canExecuteChangedRaised = false;
            _viewModel.OpenDetailsCommand.CanExecuteChanged += (s, e) => canExecuteChangedRaised = true;

            // Act
            _viewModel.SelectedDocument = _mockDocuments.First();

            // Assert
            Assert.True(canExecuteChangedRaised);
            Assert.True(_viewModel.OpenDetailsCommand.CanExecute(null));
            Assert.True(_viewModel.EditDocumentCommand.CanExecute(null));
        }

        [Fact]
        public void SortDocuments_ShouldSortAscendingFirstThenDescending()
        {
            // Arrange
            var collectionView = CollectionViewSource.GetDefaultView(_viewModel.Documents);
            _viewModel.Documents.Clear();
            _viewModel.Documents.Add(new DocumentDto { Symbol = "B" });
            _viewModel.Documents.Add(new DocumentDto { Symbol = "A" });
            _viewModel.Documents.Add(new DocumentDto { Symbol = "C" });

            // Act - First sort (ascending)
            _viewModel.SortDocuments("Symbol");
            var firstSort = _viewModel.Documents.Select(d => d.Symbol).ToList();

            // Act - Second sort (descending)
            _viewModel.SortDocuments("Symbol");
            var secondSort = _viewModel.Documents.Select(d => d.Symbol).ToList();

            // Assert
            Assert.Equal(new[] { "A", "B", "C" }, firstSort);
            Assert.Equal(new[] { "C", "B", "A" }, secondSort);
        }



        [Fact]
        public void EditDocumentCommand_WhenNoDocumentSelected_ShouldShowWarning()
        {
            // Arrange
            _viewModel.SelectedDocument = null;

            // Act
            _viewModel.EditDocumentCommand.Execute(null);

            // Assert
            _messageServiceMock.Verify(
                x => x.ShowWarning(
                    "Please select a document to edit",
                    It.IsAny<string>()), 
                Times.Once);

            _editDocVmFactoryMock.Verify(
                x => x.Create(It.IsAny<DocumentDto>()),
                Times.Never);
        }
    }
}