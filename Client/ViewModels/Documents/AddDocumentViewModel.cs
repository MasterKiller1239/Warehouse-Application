using Client.Dtos;
using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.Utilities;
using Client.ViewModels.Contractors;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client.ViewModels.Documents
{
    public class AddDocumentViewModel : INotifyPropertyChanged
    {
        #region Fields
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly IAddContractorViewFactory _addContractorViewFactory;

        private string _symbol = string.Empty;
        private ContractorDto? _selectedContractor;
        private DateTime _date = DateTime.Now;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action<bool> RequestClose;
        #endregion

        #region Properties
        public ObservableCollection<ContractorDto> Contractors { get; }

        public string Symbol
        {
            get => _symbol;
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged();
                }
            }
        }

        public ContractorDto? SelectedContractor
        {
            get => _selectedContractor;
            set
            {
                if (_selectedContractor != value)
                {
                    _selectedContractor = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public Action? CloseAction { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddContractorCommand { get; }
        #endregion

        #region Constructor
        public AddDocumentViewModel(
            IApiClient apiClient,
            IMessageService messageService,
            IAddContractorViewFactory addContractorViewFactory)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            _addContractorViewFactory = addContractorViewFactory;

            Contractors = new ObservableCollection<ContractorDto>();

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => Close(true));
            AddContractorCommand = new RelayCommand(async () => await OpenAddContractorDialog());

            _ = LoadContractorsAsync();
        }
        #endregion

        #region Public Methods
        public void Close(bool dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
        #endregion

        #region Private Methods
        private async Task LoadContractorsAsync()
        {
            try
            {
                var contractors = await _apiClient.GetContractorsAsync();
                Contractors.Clear();
                foreach (var contractor in contractors)
                {
                    Contractors.Add(contractor);
                }
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"Error loading contractors: {ex.Message}", "Failure");
            }
        }

        private async Task SaveAsync()
        {
            if (!ValidateInput())
                return;

            try
            {
                var newDocument = CreateNewDocument();
                await _apiClient.AddDocumentAsync(newDocument);

                _messageService.ShowInfo("Document added successfully.", "Success");
                RequestClose?.Invoke(true);
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"Error saving document: {ex.Message}", "Error");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Symbol))
            {
                _messageService.ShowWarning("Please enter a symbol.", "Validation");
                return false;
            }

            if (SelectedContractor == null)
            {
                _messageService.ShowWarning("Please select a contractor.", "Validation");
                return false;
            }

            return true;
        }

        private DocumentDto CreateNewDocument()
        {
            return new DocumentDto
            {
                Symbol = Symbol.Trim(),
                Date = Date.ToUniversalTime(),
                ContractorId = SelectedContractor!.Id,
                Contractor = SelectedContractor,
                Items = new List<DocumentItemDto>()
            };
        }

        public async Task OpenAddContractorDialog()
        {
            var addContractorView = _addContractorViewFactory.Create();
            if (addContractorView.ShowDialog() == true)
            {
                await LoadContractorsAsync();

                if (addContractorView.DataContext is AddContractorViewModel vm &&
                    vm.NewContractor != null)
                {
                    SelectedContractor = Contractors.Where(contractor => contractor.Name == vm.NewContractor.Name).FirstOrDefault();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}