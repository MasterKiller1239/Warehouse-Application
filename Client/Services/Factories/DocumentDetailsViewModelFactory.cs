using Client.Dtos;
using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories;
using Client.Views.DocumentDetails;
using System;

namespace Client.Services.Factories
{
    public class DocumentDetailsViewModelFactory : IDocumentDetailsViewModelFactory
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly IAddDocumentItemViewModelFactory _addItemVmFactory;
        private readonly IEditDocumentItemViewModelFactory _editItemVmFactory;

        public DocumentDetailsViewModelFactory(
            IApiClient apiClient,
            IMessageService messageService,
            IAddDocumentItemViewModelFactory addItemVmFactory,
            IEditDocumentItemViewModelFactory editItemVmFactory)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            _addItemVmFactory = addItemVmFactory;
            _editItemVmFactory = editItemVmFactory;
        }

        public DocumentDetailsViewModel Create(DocumentDto document, Action? onDocumentUpdated = null)
        {
            return new DocumentDetailsViewModel(
                document,
                _apiClient,
                _messageService,
                _addItemVmFactory,
                _editItemVmFactory,
                onDocumentUpdated);
        }
    }
}
