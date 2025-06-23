using Client.Dtos;
using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.Utilities;
using Client.ViewModels.Contractors;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

public class EditDocumentViewModel : INotifyPropertyChanged
{
    #region Fields
    private readonly IApiClient _apiClient;
    private readonly IMessageService _messageService;
    private readonly IAddContractorViewFactory _addContractorViewFactory;

    private ContractorDto? _selectedContractor;
    #endregion

    #region Events
    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<bool> RequestClose;
    #endregion

    #region Properties
    public DocumentDto Document { get; }

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

    public ObservableCollection<ContractorDto> Contractors { get; }

    public Action? CloseAction { get; set; }
    #endregion

    #region Commands
    public ICommand LoadContractorsCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand AddContractorCommand { get; }
    #endregion

    #region Constructor
    public EditDocumentViewModel(
        IApiClient apiClient,
        DocumentDto document,
        IMessageService messageService,
        IAddContractorViewFactory addContractorViewFactory)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _addContractorViewFactory = addContractorViewFactory ?? throw new ArgumentNullException(nameof(addContractorViewFactory));

        Document = document ?? throw new ArgumentNullException(nameof(document));
        Contractors = new ObservableCollection<ContractorDto>();

        LoadContractorsCommand = new RelayCommand(async () => await LoadContractorsAsync());
        UpdateCommand = new RelayCommand(async () => await UpdateDocumentAsync());
        AddContractorCommand = new RelayCommand(async () => await OpenAddContractorDialog());
        CancelCommand = new RelayCommand(() => Close(true));
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

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Contractors.Clear();
                foreach (var contractor in contractors)
                {
                    Contractors.Add(contractor);
                }

                SelectedContractor = Contractors.FirstOrDefault(c => c.Id == Document.ContractorId);
            });
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"Error loading contractors: {ex.Message}");
        }
    }

    private async Task UpdateDocumentAsync()
    {
        if (!ValidateDocument())
            return;

        try
        {
            PrepareDocumentForUpdate();
            await _apiClient.UpdateDocumentAsync(Document);

            _messageService.ShowInfo("Document updated successfully.");
            RequestClose?.Invoke(true);
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"Error updating document: {ex.Message}");
        }
    }

    private bool ValidateDocument()
    {
        if (SelectedContractor == null)
        {
            _messageService.ShowWarning("Please select a contractor.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(Document.Symbol))
        {
            _messageService.ShowWarning("Symbol cannot be empty.");
            return false;
        }

        return true;
    }

    private void PrepareDocumentForUpdate()
    {
        Document.Contractor = SelectedContractor;
        Document.Date = Document.Date.ToUniversalTime();
    }

    private async Task OpenAddContractorDialog()
    {
        var window = _addContractorViewFactory.Create();
        if (window.ShowDialog() == true)
        {
            await LoadContractorsAsync();
            if (window.DataContext is IContractorResultProvider provider &&
                provider.NewContractor is ContractorDto newContractor)
            {

                SelectedContractor = Contractors.Where(contractor => contractor.Name == newContractor.Name).FirstOrDefault();
            }
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}