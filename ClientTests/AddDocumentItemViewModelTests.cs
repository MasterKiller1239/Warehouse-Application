using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using Client.ViewModels.DocumentItems;
using Moq;
using System.Windows.Input;

namespace Client.Tests.ViewModels
{
    public class AddDocumentItemViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly AddDocumentItemViewModel _viewModel;
        private bool _wasClosed = false;
        private readonly Mock<IMessageService> _messageServiceMock;
        public AddDocumentItemViewModelTests()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _apiClientMock = new Mock<IApiClient>();
            _viewModel = new AddDocumentItemViewModel(42, _apiClientMock.Object, _messageServiceMock.Object); 
            _viewModel.CloseAction = () => _wasClosed = true;
        }

        [Theory]
        [InlineData("", "kg", "10")]
        [InlineData("Apple", "", "10")]
        [InlineData("Apple", "kg", "")]
        [InlineData("Apple", "kg", "-5")]
        [InlineData("Apple", "kg", "abc")]
        public void CanAdd_ReturnsFalse_WhenInvalidInput(string name, string unit, string quantity)
        {
            // Arrange
            _viewModel.ProductName = name;
            _viewModel.Unit = unit;
            _viewModel.QuantityText = quantity;

            // Act
            var result = _viewModel.AddCommand.CanExecute(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanAdd_ReturnsTrue_WhenValidInput()
        {
            // Arrange
            _viewModel.ProductName = "Apple";
            _viewModel.Unit = "kg";
            _viewModel.QuantityText = "12.5";

            // Act
            var result = _viewModel.AddCommand.CanExecute(null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddCommand_AddsItemAndClosesWindow()
        {
            // Arrange
            _viewModel.ProductName = " Banana ";
            _viewModel.Unit = " szt ";
            _viewModel.QuantityText = "5";

            // Act
            await ExecuteCommandAsync(_viewModel.AddCommand);

            // Assert
            _apiClientMock.Verify(api => api.AddDocumentItemAsync(It.Is<DocumentItemDto>(dto =>
                dto.DocumentId == 42 &&
                dto.ProductName == "Banana" &&
                dto.Unit == "szt" &&
                dto.Quantity == 5m
            )), Times.Once);

            Assert.True(_wasClosed);
        }

        [Fact]
        public async Task AddCommand_ShowsErrorMessage_OnException()
        {
            // Arrange
            _viewModel.ProductName = "Test";
            _viewModel.Unit = "kg";
            _viewModel.QuantityText = "1.5";

            _apiClientMock
                .Setup(a => a.AddDocumentItemAsync(It.IsAny<DocumentItemDto>()))
                .ThrowsAsync(new Exception("API error"));

            // Act
            await ExecuteCommandAsync(_viewModel.AddCommand);

            // Assert
            _apiClientMock.Verify(a => a.AddDocumentItemAsync(It.IsAny<DocumentItemDto>()), Times.Once);
            Assert.False(_wasClosed); // okno się nie zamyka przy błędzie
        }

        private static async Task ExecuteCommandAsync(ICommand command)
        {
            if (command.CanExecute(null))
            {
                if (command is RelayCommand relayCommand)
                    await Task.Run(() => relayCommand.Execute(null));
                else
                    command.Execute(null);
            }
        }
    }
}
