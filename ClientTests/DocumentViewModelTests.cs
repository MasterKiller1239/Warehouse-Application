using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Documents;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Client.Tests.ViewModels
{
    public class DocumentsViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly DocumentsViewModel _viewModel;
        private readonly List<DocumentDto> _mockDocuments;
        private readonly Mock<IMessageService> _messageServiceMock;
        public DocumentsViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _messageServiceMock = new Mock<IMessageService>();
            _mockDocuments = new List<DocumentDto>
            {
                new DocumentDto { Id = 1, Symbol = "ABC123" },
                new DocumentDto { Id = 2, Symbol = "XYZ456" },
                new DocumentDto { Id = 3, Symbol = "abc789" }
            };

            _apiClientMock.Setup(a => a.GetDocumentsAsync()).ReturnsAsync(_mockDocuments);

            // Tworzymy ViewModel z zamockowanym klientem
            _viewModel = new DocumentsViewModel(_apiClientMock.Object, _messageServiceMock.Object);
        }

        [Fact]
        public async Task LoadDocumentsCommand_LoadsDocumentsIntoCollection()
        {
            // Act
            await Task.Delay(100); 

            // Assert
            Assert.Equal(3, _viewModel.Documents.Count);
        }

        [Fact]
        public async Task SymbolFilter_FiltersDocumentsCorrectly()
        {
            // Arrange
            await Task.Delay(100);
            _viewModel.SearchText = "abc";

            // Act
            _viewModel.SearchCommand.Execute(null);

            // Assert
            Assert.Equal(2, _viewModel.Documents.Count); // "ABC123" i "abc789"
        }

        [Fact]
        public void SelectedDocument_PropertyChanged_RaisesCommandCanExecuteChanged()
        {
            // Arrange
            var doc = new DocumentDto { Id = 99, Symbol = "TEST" };
            bool canExecuteBefore = _viewModel.OpenDetailsCommand.CanExecute(null);

            // Act
            _viewModel.SelectedDocument = doc;
            bool canExecuteAfter = _viewModel.OpenDetailsCommand.CanExecute(null);

            // Assert
            Assert.False(canExecuteBefore);
            Assert.True(canExecuteAfter);
        }

        [Fact]
        public void SortDocuments_ChangesOrderCorrectly()
        {
            // Arrange
            _viewModel.Documents.Clear();
            _viewModel.Documents.Add(new DocumentDto { Symbol = "Z" });
            _viewModel.Documents.Add(new DocumentDto { Symbol = "A" });

            // Act
            _viewModel.SortDocuments("Symbol");

            // Assert
            var sortedSymbols = _viewModel.Documents.Select(d => d.Symbol).ToList();
            Assert.Equal(new[] { "Z", "A" }, sortedSymbols);
        }
    }
}
