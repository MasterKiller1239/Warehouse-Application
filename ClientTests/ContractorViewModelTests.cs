using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Contractors;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Client.Tests.ViewModels.Contractors
{
    public class ContractorsViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly ContractorsViewModel _viewModel;

        public ContractorsViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();

            // Domyślna lista kontrahentów do testów
            _apiClientMock.Setup(x => x.GetContractorsAsync())
                .ReturnsAsync(new List<ContractorDto>
                {
                    new ContractorDto { Id = 1, Name = "Acme" },
                    new ContractorDto { Id = 2, Name = "Globex" }
                });

            _viewModel = new ContractorsViewModel(_apiClientMock.Object);
        }

        [Fact]
        public async Task LoadContractorsAsync_PopulatesContractorsCollection()
        {
            // Wait for async method inside constructor to finish
            await Task.Delay(100);

            Assert.Equal(2, _viewModel.Contractors.Count);
            Assert.Contains(_viewModel.Contractors, c => c.Name == "Acme");
            Assert.Contains(_viewModel.Contractors, c => c.Name == "Globex");
        }

        [Fact]
        public void SelectedContractor_Set_ShouldRaisePropertyChanged_AndUpdateEditCommand()
        {
            var contractor = new ContractorDto { Id = 3, Name = "TestCo" };

            bool propertyChangedCalled = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.SelectedContractor))
                    propertyChangedCalled = true;
            };

            _viewModel.SelectedContractor = contractor;

            Assert.True(propertyChangedCalled);
            Assert.Equal(contractor, _viewModel.SelectedContractor);
            Assert.True(_viewModel.EditCommand.CanExecute(null));
        }

        [Fact]
        public void EditCommand_CanExecute_ReturnsFalse_WhenNoContractorSelected()
        {
            _viewModel.SelectedContractor = null;
            Assert.False(_viewModel.EditCommand.CanExecute(null));
        }

        [Fact]
        public void EditCommand_CanExecute_ReturnsTrue_WhenContractorSelected()
        {
            _viewModel.SelectedContractor = new ContractorDto { Id = 1, Name = "Acme" };
            Assert.True(_viewModel.EditCommand.CanExecute(null));
        }

        [Fact]
        public void AddCommand_IsNotNull()
        {
            Assert.NotNull(_viewModel.AddCommand);
        }

        [Fact]
        public void EditCommand_IsNotNull()
        {
            Assert.NotNull(_viewModel.EditCommand);
        }
    }
}
