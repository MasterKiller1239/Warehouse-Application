using Client.Dtos;
using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.ViewModels.Contractors;
using Client.Views.Contractors;

namespace Client.Services.Factories.Contractors
{
    public class EditContractorViewFactory : IEditContractorViewFactory
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;

        public EditContractorViewFactory(IApiClient apiClient, IMessageService messageService)
        {
            _apiClient = apiClient;
            _messageService = messageService;
        }

        public EditContractorView Create(ContractorDto contractor)
        {
            var viewModel = new EditContractorViewModel(_apiClient, contractor, _messageService);
            var view = new EditContractorView { DataContext = viewModel };
            viewModel.CloseAction = () => view.DialogResult = true;
            return view;
        }
    }
}
