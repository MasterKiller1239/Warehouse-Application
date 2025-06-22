using Client.Services.Interfaces.IFactories;
using Client.Services.Interfaces;
using Client.ViewModels.Documents;
using Client.Services.Interfaces.IFactories.Contractors;

namespace Client.Services.Factories
{
    public class AddDocumentViewModelFactory : IAddDocumentViewModelFactory
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly IAddContractorViewFactory _addContractorViewFactory;

        public AddDocumentViewModelFactory(
            IApiClient apiClient,
            IMessageService messageService,
            IAddContractorViewFactory addContractorViewFactory)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            _addContractorViewFactory = addContractorViewFactory;
        }

        public AddDocumentViewModel Create() => new(_apiClient, _messageService, _addContractorViewFactory);
    }
}
