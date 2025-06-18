using Client.Services.Interfaces;
using Client.Utilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Client.Dtos;

namespace Client.ViewModels.Contractors
{
    public class EditContractorViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;

        public string Symbol { get; set; }
        public string Name { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly int _contractorId;

        public Action? CloseAction { get; set; }
        public EditContractorViewModel(IApiClient apiClient, ContractorDto contractor, Window window, IMessageService messageService)
        {
            _apiClient = apiClient;
            _contractorId = contractor.Id;
            _messageService = messageService;
            Symbol = contractor.Symbol;
            Name = contractor.Name;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Symbol) || string.IsNullOrWhiteSpace(Name))
            {
                _messageService.ShowWarning("All fields are required.", "Validation");
                return;
            }

            var updated = new ContractorDto
            {
                Id = _contractorId,
                Symbol = Symbol,
                Name = Name
            };

            try
            {
                await _apiClient.UpdateContractorAsync(updated);
                _messageService.ShowInfo("Contractor updated.", "Success");
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"Error: {ex.Message}", "Failure");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
