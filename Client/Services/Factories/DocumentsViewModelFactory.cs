using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories;
using Client.ViewModels.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    // Client/Services/Factories/DocumentsViewModelFactory.cs
    public class DocumentsViewModelFactory : IDocumentsViewModelFactory
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly IAddDocumentViewModelFactory _addDocumentVmFactory;
        private readonly IEditDocumentViewModelFactory _editDocumentVmFactory;
        private readonly IDocumentDetailsViewModelFactory _documentDetailsVmFactory;

        public DocumentsViewModelFactory(
            IApiClient apiClient,
            IMessageService messageService,
            IAddDocumentViewModelFactory addDocumentVmFactory,
            IEditDocumentViewModelFactory editDocumentVmFactory,
            IDocumentDetailsViewModelFactory documentDetailsVmFactory)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            _addDocumentVmFactory = addDocumentVmFactory;
            _editDocumentVmFactory = editDocumentVmFactory;
            _documentDetailsVmFactory = documentDetailsVmFactory;
        }

        public DocumentsViewModel Create()
        {
            return new DocumentsViewModel(
                _apiClient,
                _messageService,
                _addDocumentVmFactory,
                _editDocumentVmFactory,
                _documentDetailsVmFactory);
        }
    }
}
