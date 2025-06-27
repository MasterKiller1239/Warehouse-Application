using Client.Services.Interfaces.IFactories;
using Client.Services.Interfaces;
using Client.ViewModels.DocumentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Dtos;

namespace Client.Services.Factories
{
    public class EditDocumentItemViewModelFactory : IEditDocumentItemViewModelFactory
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;

        public EditDocumentItemViewModelFactory(
            IApiClient apiClient,
            IMessageService messageService)
        {
            _apiClient = apiClient;
            _messageService = messageService;
        }

        public EditDocumentItemViewModel Create(DocumentItemDto item)
        {
            var viewModel = new EditDocumentItemViewModel(
                item,
                _apiClient,
                _messageService)
            {
                CloseAction = () => _messageService.ShowInfo("Operation cancelled", "Info")
            };

            return viewModel;
        }
    }
}
