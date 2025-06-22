using Client.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Client.Utilities;
using Client.Dtos;
using Client.ViewModels.Contractors;
using Client.Views.Contractors;
using Client.Services.Interfaces.IFactories.Contractors;

namespace Client.ViewModels.Documents
{
    public class AddDocumentViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;

        private string _symbol = string.Empty;
        private ContractorDto? _selectedContractor;
        private DateTime _date = DateTime.Now;
        private readonly IMessageService _messageService; 
        private readonly IAddContractorViewFactory _addContractorViewFactory;
        public ObservableCollection<ContractorDto> Contractors { get; } = new ObservableCollection<ContractorDto>();

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
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand AddContractorCommand { get; }
        public Action? CloseAction { get;  set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action<bool> RequestClose;

        public void Close(bool dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public AddDocumentViewModel(IApiClient apiClient, IMessageService messageService, IAddContractorViewFactory addContractorViewFactory)
        {
            _apiClient = apiClient;
            _messageService = messageService; 
            _addContractorViewFactory = addContractorViewFactory;
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            AddContractorCommand = new RelayCommand(async () => await OpenAddContractorDialog());
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());_addContractorViewFactory = addContractorViewFactory;
            LoadContractorsAsync();
        }

        private async Task LoadContractorsAsync()
        {
            try
            {
                var contractors = await _apiClient.GetContractorsAsync();
                Contractors.Clear();
                foreach (var c in contractors)
                    Contractors.Add(c);
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"Error loading contractors: {ex.Message}", "Failure");
            }
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Symbol))
            {
                _messageService.ShowWarning("Please enter a symbol.", "Validation");
                return;
            }

            try
            {
                var existing = await _apiClient.GetDocumentBySymbolAsync(Symbol.Trim());
                if (existing != null)
                {
                    _messageService.ShowWarning($"A document with symbol '{Symbol}' already exists.", "Validation");
                    return;
                }

                if (SelectedContractor == null)
                {
                    _messageService.ShowWarning("Please select a contractor.", "Validation");
                    return;
                }

                var newDocument = new DocumentDto
                {
                    Symbol = Symbol.Trim(),
                    Date = Date.ToUniversalTime(),
                    ContractorId = SelectedContractor.Id,
                    Contractor = SelectedContractor,
                    Items = new List<DocumentItemDto>()
                };

                await _apiClient.AddDocumentAsync(newDocument);
                _messageService.ShowInfo("Document added successfully.", "Success");

                RequestClose?.Invoke(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task OpenAddContractorDialog()
        {
            var addContractorView = _addContractorViewFactory.Create();
            if (addContractorView.ShowDialog() == true)
            {
                await LoadContractorsAsync();
                if (addContractorView.DataContext is AddContractorViewModel vm && vm.NewContractor != null)
                {
                    SelectedContractor = Contractors.FirstOrDefault(c => c.Id == vm.NewContractor.Id);
                }
            }
        }
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
