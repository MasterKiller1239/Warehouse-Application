using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client.ViewModels.Contractors
{
    public class EditContractorViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly ContractorDto _originalContractor;

        private string _symbol;
        public string Symbol
        {
            get => _symbol;
            set => SetProperty(ref _symbol, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseAction { get; set; }

        public EditContractorViewModel(
            IApiClient apiClient, 
            ContractorDto contractor,
            IMessageService messageService)
        {
            _apiClient = apiClient;
            _originalContractor = contractor;
            _messageService = messageService;

            _symbol = contractor.Symbol;
            _name = contractor.Name;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Symbol) || string.IsNullOrWhiteSpace(Name))
            {
                _messageService.ShowWarning("Symbol and name are required", "Validation Error");
                return;
            }

            try
            {
                var updatedContractor = new ContractorDto
                {
                    Id = _originalContractor.Id,
                    Symbol = Symbol,
                    Name = Name
                };

                await _apiClient.UpdateContractorAsync(updatedContractor);
                _messageService.ShowInfo("Contractor updated successfully");
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"Error saving contractor: {ex.Message}");
                CloseAction?.Invoke();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}