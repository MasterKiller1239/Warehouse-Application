using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.ViewModels.Contractors;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Client.Services.Factories.Contractors
{
    public class ContractorsViewModelFactory : IContractorsViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;

        public ContractorsViewModelFactory(
            IServiceProvider serviceProvider,
            IApiClient apiClient,
            IMessageService messageService)
        {
            _serviceProvider = serviceProvider;
            _apiClient = apiClient;
            _messageService = messageService;
        }

        public ContractorsViewModel Create()
        {
            var addContractorViewFactory = _serviceProvider.GetRequiredService<IAddContractorViewFactory>();
            var editContractorViewFactory = _serviceProvider.GetRequiredService<IEditContractorViewFactory>();

            return new ContractorsViewModel(
                _apiClient,
                _messageService,
                addContractorViewFactory,
                editContractorViewFactory)
            {
            };
        }
    }
}