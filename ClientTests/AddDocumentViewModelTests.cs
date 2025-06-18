using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Documents;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Client.Tests.ViewModels
{
    public class AddDocumentViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly AddDocumentViewModel _viewModel;
        private readonly Mock<IMessageService> _messageServiceMock;
        private bool _wasClosed;
        private IMessageService _messageService;
        public AddDocumentViewModelTests()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _apiClientMock = new Mock<IApiClient>();
            _viewModel = new AddDocumentViewModel(_apiClientMock.Object, _messageServiceMock.Object);
            _viewModel.RequestClose += (s, e) => _wasClosed = true;
        }

        [Fact]
        public async Task SaveCommand_ShowsWarning_WhenSymbolIsEmpty()
        {
            // Arrange
            _viewModel.Symbol = "";
            _viewModel.SelectedContractor = new ContractorDto { Id = 1, Name = "Test Contractor" };

            // Act
            await ExecuteCommandAsync(_viewModel.SaveCommand);

            // Assert
            _apiClientMock.Verify(a => a.AddDocumentAsync(It.IsAny<DocumentDto>()), Times.Never);
            Assert.False(_wasClosed);
        }

        [Fact]
        public async Task SaveCommand_ShowsError_WhenDocumentAlreadyExists()
        {
            // Arrange
            _viewModel.Symbol = "DOC123";
            _viewModel.SelectedContractor = new ContractorDto { Id = 2, Name = "Test" };

            _apiClientMock
                .Setup(a => a.GetDocumentBySymbolAsync("DOC123"))
                .ReturnsAsync(new DocumentDto { Symbol = "DOC123" });

            // Act
            await ExecuteCommandAsync(_viewModel.SaveCommand);

            // Assert
            _apiClientMock.Verify(a => a.AddDocumentAsync(It.IsAny<DocumentDto>()), Times.Never);
            Assert.False(_wasClosed);
        }

        [Fact]
        public async Task SaveCommand_ShowsWarning_WhenNoContractorSelected()
        {
            // Arrange
            _viewModel.Symbol = "DOC124";
            _viewModel.SelectedContractor = null;

            _apiClientMock
                .Setup(a => a.GetDocumentBySymbolAsync("DOC124"))
                .ReturnsAsync((DocumentDto?)null);

            // Act
            await ExecuteCommandAsync(_viewModel.SaveCommand);

            // Assert
            _apiClientMock.Verify(a => a.AddDocumentAsync(It.IsAny<DocumentDto>()), Times.Never);
            Assert.False(_wasClosed);
        }

        [Fact]
        public async Task SaveCommand_AddsDocument_WhenValidData()
        {
            // Arrange
            var contractor = new ContractorDto { Id = 3, Name = "ValidCo" };
            _viewModel.Symbol = "NEW123";
            _viewModel.SelectedContractor = contractor;

            _apiClientMock
                .Setup(a => a.GetDocumentBySymbolAsync("NEW123"))
                .ReturnsAsync((DocumentDto?)null);

            // Act
            await ExecuteCommandAsync(_viewModel.SaveCommand);

            // Assert
            _apiClientMock.Verify(a => a.AddDocumentAsync(It.Is<DocumentDto>(d =>
                d.Symbol == "NEW123" &&
                d.ContractorId == contractor.Id &&
                d.Contractor == contractor &&
                d.Items != null)), Times.Once);

            Assert.True(_wasClosed);
        }

        private static async Task ExecuteCommandAsync(System.Windows.Input.ICommand command)
        {
            if (command.CanExecute(null))
            {
                if (command is Client.Utilities.RelayCommand relay)
                    await Task.Run(() => relay.Execute(null));
                else
                    command.Execute(null);
            }
        }
    }
}
