using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Contractors;
using Moq;
using System.Windows.Input;

namespace Client.Tests.ViewModels
{
    public class AddContractorViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly AddContractorViewModel _viewModel;
        private bool _wasClosed = false;
        private bool _eventRaised = false;
        private readonly Mock<IMessageService> _messageServiceMock;
        public AddContractorViewModelTests()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _apiClientMock = new Mock<IApiClient>();
            _viewModel = new AddContractorViewModel(_apiClientMock.Object, _messageServiceMock.Object);
            _viewModel.CloseAction = () => _wasClosed = true;
            _viewModel.RequestClose += (s, e) => _eventRaised = true;
        }

        [Fact]
        public void AddCommand_CannotExecute_WhenFieldsEmpty()
        {
            // Arrange
            _viewModel.Symbol = "";
            _viewModel.Name = "";

            // Act
            var canExecute = _viewModel.AddCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void AddCommand_CanExecute_WhenFieldsFilled()
        {
            // Arrange
            _viewModel.Symbol = "S1";
            _viewModel.Name = "SomeName";

            // Act
            var canExecute = _viewModel.AddCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public async Task AddCommand_AddsContractor_WhenValid()
        {
            // Arrange
            _viewModel.Symbol = "ABC";
            _viewModel.Name = "TestCo";

            ContractorDto? capturedDto = null;
            _apiClientMock
                .Setup(x => x.AddContractorAsync(It.IsAny<ContractorDto>()))
                .Returns(Task.CompletedTask)
                .Callback<ContractorDto>(dto => capturedDto = dto);

            // Act
            await ExecuteCommandAsync(_viewModel.AddCommand);

            // Assert
            Assert.True(_wasClosed);
            Assert.True(_eventRaised);
            Assert.NotNull(_viewModel.NewContractor);
            Assert.Equal("ABC", _viewModel.NewContractor.Symbol);
            Assert.Equal("TestCo", _viewModel.NewContractor.Name);

            Assert.NotNull(capturedDto);
            Assert.Equal("ABC", capturedDto.Symbol);
        }

        [Fact]
        public async Task AddCommand_DoesNotCrash_OnError()
        {
            // Arrange
            _viewModel.Symbol = "XYZ";
            _viewModel.Name = "Bad";

            _apiClientMock
                .Setup(x => x.AddContractorAsync(It.IsAny<ContractorDto>()))
                .ThrowsAsync(new Exception("Simulated failure"));

            // Act
            await ExecuteCommandAsync(_viewModel.AddCommand);

            // Assert
            Assert.False(_wasClosed);
            Assert.False(_eventRaised);
            Assert.Null(_viewModel.NewContractor);
        }

        [Fact]
        public void PropertyChanged_Raised_WhenSymbolOrNameChanged()
        {
            // Arrange
            string? changedProperty = null;
            _viewModel.PropertyChanged += (s, e) => changedProperty = e.PropertyName;

            // Act
            _viewModel.Symbol = "X";

            // Assert
            Assert.Equal("Symbol", changedProperty);

            changedProperty = null;
            _viewModel.Name = "Y";

            Assert.Equal("Name", changedProperty);
        }

        private static async Task ExecuteCommandAsync(ICommand command)
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
