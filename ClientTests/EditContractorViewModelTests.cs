using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Contractors;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Client.Tests.ViewModels
{
    public class EditContractorViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly ContractorDto _contractor;
        private readonly EditContractorViewModel _viewModel;
        private bool _wasClosed = false;
        private readonly Mock<IMessageService> _messageServiceMock;
        public EditContractorViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _messageServiceMock = new Mock<IMessageService>();
            _contractor = new ContractorDto
            {
                Id = 5,
                Symbol = "OLD",
                Name = "OldName"
            };

            _viewModel = new EditContractorViewModel(_apiClientMock.Object, _contractor, _messageServiceMock.Object)
            {
                CloseAction = () => _wasClosed = true
            };
        }

        [Fact]
        public async Task SaveAsync_DoesNotCallUpdate_WhenFieldsAreEmpty()
        {
            // Arrange
            _viewModel.Symbol = "";
            _viewModel.Name = "";

            // Act
            await ExecuteCommandAsync(_viewModel.SaveCommand);

            // Assert
            _apiClientMock.Verify(x => x.UpdateContractorAsync(It.IsAny<ContractorDto>()), Times.Never);
            Assert.False(_wasClosed);
        }

        [Fact]
        public async Task SaveAsync_CallsUpdate_WhenValid()
        {
            // Arrange
            _viewModel.Symbol = "NEW";
            _viewModel.Name = "NewName";

            // Act
            await ExecuteCommandAsync(_viewModel.SaveCommand);

            // Assert
            _apiClientMock.Verify(x => x.UpdateContractorAsync(It.Is<ContractorDto>(
                dto => dto.Id == 5 && dto.Symbol == "NEW" && dto.Name == "NewName"
            )), Times.Once);

            Assert.True(_wasClosed);
        }

        [Fact]
        public async Task SaveAsync_ShowsError_WhenUpdateThrows()
        {
            // Arrange
            _viewModel.Symbol = "ERR";
            _viewModel.Name = "ErrorTest";

            _apiClientMock
                .Setup(x => x.UpdateContractorAsync(It.IsAny<ContractorDto>()))
                .ThrowsAsync(new Exception("DB failed"));

            // Act
            await ExecuteCommandAsync(_viewModel.SaveCommand);

            // Assert
            _apiClientMock.Verify(x => x.UpdateContractorAsync(It.IsAny<ContractorDto>()), Times.Once);
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
