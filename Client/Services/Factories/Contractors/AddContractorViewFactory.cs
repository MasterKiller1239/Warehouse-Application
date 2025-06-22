using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.ViewModels.Contractors;
using Client.Views.Contractors;

namespace Client.Services.Factories.Contractors
{
    public class AddContractorViewFactory : IAddContractorViewFactory
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;

        public AddContractorViewFactory(IApiClient apiClient, IMessageService messageService)
        {
            _apiClient = apiClient;
            _messageService = messageService;
        }

        public AddContractorView Create()
        {
            var vm = new AddContractorViewModel(_apiClient, _messageService);
            var view = new AddContractorView()
            {
                DataContext = vm
            };
            vm.CloseAction = view.Close;
            return view;
        }
    }
}
