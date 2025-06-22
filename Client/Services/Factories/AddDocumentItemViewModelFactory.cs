using Client.Services.Interfaces.IFactories;
using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Factories
{
    public class AddDocumentItemViewModelFactory : IAddDocumentItemViewModelFactory
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;

        public AddDocumentItemViewModelFactory(
            IApiClient apiClient,
            IMessageService messageService)
        {
            _apiClient = apiClient;
            _messageService = messageService;
        }

        public AddDocumentItemViewModel Create(int documentId)
        {
            return new AddDocumentItemViewModel(
                documentId,
                _apiClient,
                _messageService);
        }
    }
}
