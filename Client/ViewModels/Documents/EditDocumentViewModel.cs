using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using Client.ViewModels.Contractors;
using Client.Views.Contractors;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class EditDocumentViewModel : INotifyPropertyChanged
{
    private readonly IApiClient _apiClient;
    public ICommand AddContractorCommand { get; }
    private readonly IMessageService _messageService;
    public EditDocumentViewModel(IApiClient apiClient, DocumentDto document, IMessageService messageService)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        Document = document ?? throw new ArgumentNullException(nameof(document));
        _messageService = messageService;
        Contractors = new ObservableCollection<ContractorDto>();
        LoadContractorsCommand = new RelayCommand(async () => await LoadContractorsAsync());
        UpdateCommand = new RelayCommand(async () => await UpdateDocumentAsync());
        AddContractorCommand = new RelayCommand(async () => await OpenAddContractorDialog());
        _ = LoadContractorsAsync();

    }

    public DocumentDto Document { get; }

    private ContractorDto? _selectedContractor;
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

    public ObservableCollection<ContractorDto> Contractors { get; private set; }

    public ICommand LoadContractorsCommand { get; }
    public ICommand UpdateCommand { get; }
  
    public async Task LoadContractorsAsync()
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

    public async Task UpdateDocumentAsync()
    {
        if (SelectedContractor == null)
        {
            MessageBox.Show("Please select a contractor.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(Document.Symbol))
        {
            MessageBox.Show("Symbol cannot be empty.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Document.Contractor = SelectedContractor;
        Document.Date = Document.Date.ToUniversalTime();
        try
        {
            await _apiClient.UpdateDocumentAsync(Document);
            MessageBox.Show("Document updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private async Task OpenAddContractorDialog()
    {
        var window = new AddContractorView(_apiClient,_messageService);
        if (window.ShowDialog() == true)
        {
            if (window.DataContext is IContractorResultProvider provider && provider.NewContractor is ContractorDto newContractor)
            {

                await LoadContractorsAsync();
                SelectedContractor = Contractors.Where(contractor => contractor.Name == newContractor.Name).FirstOrDefault();
            }
        }
    }

    public event EventHandler? RequestClose;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
