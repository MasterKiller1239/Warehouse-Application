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
        #region Fields
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly ContractorDto _originalContractor;
        private string _symbol;
        private string _name;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public string Symbol
        {
            get => _symbol;
            set => SetProperty(ref _symbol, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Action CloseAction { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructor
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
        #endregion

        #region Public Methods
        // (Placeholder for any public methods if needed)
        #endregion

        #region Private Methods
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
                    Symbol = Symbol.Trim(),
                    Name = Name.Trim()
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

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region Protected Methods
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}