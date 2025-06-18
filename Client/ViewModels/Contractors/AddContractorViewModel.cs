using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModels.Contractors
{
    public class AddContractorViewModel : INotifyPropertyChanged, IContractorResultProvider
    {
        private readonly IMessageService _messageService;
        private readonly IApiClient _apiClient;
        public event EventHandler? RequestClose;
        public AddContractorViewModel(IApiClient apiClient, IMessageService messageService)
        {
            _apiClient = apiClient;
            _addCommand = new RelayCommand(async () => await AddContractor(), CanAdd);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
            _messageService = messageService;
        }

        private string _symbol;
        public string Symbol
        {
            get => _symbol;
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged();
                    _addCommand.RaiseCanExecuteChanged(); 
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                    _addCommand.RaiseCanExecuteChanged(); 
                }
            }
        }

        private RelayCommand _addCommand;
        public ICommand AddCommand { get => _addCommand; }
        public ICommand CancelCommand { get; }

        public Action? CloseAction { get; set; }
        public ContractorDto? NewContractor { get; private set; }
        private bool CanAdd() => !string.IsNullOrWhiteSpace(Symbol) && !string.IsNullOrWhiteSpace(Name);
        private async Task AddContractor()
        {
            try
            {
                var contractor = new ContractorDto { Symbol = Symbol.Trim(), Name = Name.Trim() };
                await _apiClient.AddContractorAsync(contractor);
                NewContractor = contractor;
                _messageService.ShowInfo("Contractor added successfully.", "Success");
                RequestClose?.Invoke(this, EventArgs.Empty);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"Failed to add contractor: {ex.Message}", "Error");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
