using Client.Services.Interfaces.IFactories;
using Client.Services.Interfaces;
using Client.Dtos;
using Client.Services.Interfaces.IFactories.Contractors;

namespace Client.Services.Factories
{
    public class EditDocumentViewModelFactory : IEditDocumentViewModelFactory
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly IAddContractorViewFactory _addContractorViewFactory;

        public EditDocumentViewModelFactory(
            IApiClient apiClient,
            IMessageService messageService,
            IAddContractorViewFactory addContractorViewFactory)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            _addContractorViewFactory = addContractorViewFactory;
        }

        public EditDocumentViewModel Create(DocumentDto document) =>
            new(_apiClient, document, _messageService, _addContractorViewFactory);
    }
}
