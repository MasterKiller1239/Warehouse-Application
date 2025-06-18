using Client.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Client.Utilities;
using Client.Dtos;
using Client.ViewModels.Contractors;
using Client.Views.Contractors;

namespace Client.ViewModels.Documents
{
    public class AddDocumentViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;

        private string _symbol = string.Empty;
        private ContractorDto? _selectedContractor;
        private DateTime _date = DateTime.Now;
        private readonly IMessageService _messageService;
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

        public ICommand SaveCommand { get; }
        public ICommand AddContractorCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? RequestClose;

        public AddDocumentViewModel(IApiClient apiClient, IMessageService messageService)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            AddContractorCommand = new RelayCommand(async () => await OpenAddContractorDialog());

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

                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task OpenAddContractorDialog()
        {
            var window = new AddContractorView(_apiClient, _messageService);
            if (window.ShowDialog() == true)
            {
                if (window.DataContext is IContractorResultProvider provider && provider.NewContractor is ContractorDto newContractor)
                {

                    await LoadContractorsAsync();
                    SelectedContractor = Contractors.Where(contractor => contractor.Name == newContractor.Name).FirstOrDefault();
                }
            }
        }
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
