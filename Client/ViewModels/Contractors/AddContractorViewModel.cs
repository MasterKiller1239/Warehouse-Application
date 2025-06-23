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
        #region Fields
        private readonly IMessageService _messageService;
        private readonly IApiClient _apiClient;
        private string _symbol;
        private string _name;
        private readonly RelayCommand _addCommand;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? RequestClose;
        #endregion

        #region Properties
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

        public ContractorDto? NewContractor { get; private set; }
        public Action? CloseAction { get; set; }
        #endregion

        #region Commands
        public ICommand AddCommand => _addCommand;
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructor
        public AddContractorViewModel(IApiClient apiClient, IMessageService messageService)
        {
            _apiClient = apiClient;
            _messageService = messageService;

            _addCommand = new RelayCommand(async () => await AddContractor(), CanAdd);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
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

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}