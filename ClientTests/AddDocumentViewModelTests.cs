using Client.Dtos;
using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.Utilities;
using Client.ViewModels.Contractors;
using Client.ViewModels.Documents;
using Client.Views.Contractors;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xunit;

namespace Client.Tests.ViewModels
{
    public class AddDocumentViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly Mock<IAddContractorViewFactory> _addContractorViewFactoryMock;
        private readonly AddDocumentViewModel _viewModel;
        private bool _wasClosed;

        public AddDocumentViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _messageServiceMock = new Mock<IMessageService>();
            _addContractorViewFactoryMock = new Mock<IAddContractorViewFactory>();

            _viewModel = new AddDocumentViewModel(
                _apiClientMock.Object,
                _messageServiceMock.Object,
                _addContractorViewFactoryMock.Object);

            _viewModel.RequestClose += (success) => _wasClosed = success;
        }

        [Fact]
        public async Task SaveCommand_WhenSymbolEmpty_ShowsWarningAndDoesNotClose()
        {
            // Arrange
            _viewModel.Symbol = "";
            _viewModel.SelectedContractor = new ContractorDto { Id = 1 };

            // Act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // Assert
            _messageServiceMock.Verify(x => x.ShowWarning(
                "Please enter a symbol.",
                It.IsAny<string>()),
                Times.Once);

            Assert.False(_wasClosed);
            _apiClientMock.Verify(x => x.AddDocumentAsync(It.IsAny<DocumentDto>()), Times.Never);
        }

        [Fact]
        public async Task SaveCommand_WhenDocumentExists_ShowsWarningAndDoesNotClose()
        {
            // Arrange
            _viewModel.Symbol = "EXIST123";
            _viewModel.SelectedContractor = new ContractorDto { Id = 2 };

            _apiClientMock.Setup(x => x.GetDocumentBySymbolAsync("EXIST123"))
                .ReturnsAsync(new DocumentDto());

            // Act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // Assert
            _messageServiceMock.Verify(x => x.ShowWarning(
                "A document with symbol 'EXIST123' already exists.",
                It.IsAny<string>()),
                Times.Once);

            Assert.False(_wasClosed);
            _apiClientMock.Verify(x => x.AddDocumentAsync(It.IsAny<DocumentDto>()), Times.Never);
        }

        [Fact]
        public async Task SaveCommand_WhenNoContractor_ShowsWarningAndDoesNotClose()
        {
            // Arrange
            _viewModel.Symbol = "VALID123";
            _viewModel.SelectedContractor = null;

            _apiClientMock.Setup(x => x.GetDocumentBySymbolAsync("VALID123"))
                .ReturnsAsync((DocumentDto?)null);

            // Act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // Assert
            _messageServiceMock.Verify(x => x.ShowWarning(
                "Please select a contractor.",
                It.IsAny<string>()),
                Times.Once);

            Assert.False(_wasClosed);
            _apiClientMock.Verify(x => x.AddDocumentAsync(It.IsAny<DocumentDto>()), Times.Never);
        }

        [Fact]
        public async Task SaveCommand_WithValidData_AddsDocumentAndCloses()
        {
            // Arrange
            var contractor = new ContractorDto { Id = 3, Name = "Test" };
            _viewModel.Symbol = "NEW123";
            _viewModel.SelectedContractor = contractor;
            _viewModel.Date = DateTime.Now;

            _apiClientMock.Setup(x => x.GetDocumentBySymbolAsync("NEW123"))
                .ReturnsAsync((DocumentDto?)null);

            _apiClientMock
    .Setup(x => x.AddDocumentAsync(It.IsAny<DocumentDto>()));

            // Act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // Assert
            _apiClientMock.Verify(x => x.AddDocumentAsync(It.Is<DocumentDto>(d =>
                d.Symbol == "NEW123" &&
                d.ContractorId == 3 &&
                d.Contractor == contractor &&
                d.Items != null &&
                d.Date == _viewModel.Date.ToUniversalTime())),
                Times.Once);

            _messageServiceMock.Verify(x => x.ShowInfo(
                "Document added successfully.",
                "Success"),
                Times.Once);

            Assert.True(_wasClosed);
        }

        [Fact]
        public async Task SaveCommand_WhenApiFails_ShowsErrorAndDoesNotClose()
        {
            // Arrange
            _viewModel.Symbol = "FAIL123";
            _viewModel.SelectedContractor = new ContractorDto { Id = 4 };

            _apiClientMock.Setup(x => x.GetDocumentBySymbolAsync("FAIL123"))
                .ReturnsAsync((DocumentDto?)null);

            _apiClientMock.Setup(x => x.AddDocumentAsync(It.IsAny<DocumentDto>()))
                .ThrowsAsync(new Exception("API Error"));

            // Act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // Assert
            _messageServiceMock.Verify(x => x.ShowError(
                "An error occurred while saving the document: API Error",
                "Error"),
                Times.Once);

            Assert.False(_wasClosed);
        }
    }

    // Extension method for async command execution
    public static class CommandExtensions
    {
        public static async Task ExecuteAsync(this ICommand command, object parameter)
        {
            if (command is RelayCommand relayCommand)
            {
                await Task.Run(() => relayCommand.Execute(parameter));
            }
            else
            {
                command.Execute(parameter);
            }
        }
    }
}