using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class EditDocumentViewModel : INotifyPropertyChanged
{
    private readonly IApiClient _apiClient;

    public EditDocumentViewModel(IApiClient apiClient, DocumentDto document)
    {
        _apiClient = apiClient;
        Document = document;

        Contractors = new ObservableCollection<ContractorDto>();
        LoadContractorsCommand = new RelayCommand(async () => await LoadContractorsAsync());
        UpdateCommand = new RelayCommand(async () => await UpdateDocumentAsync());

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

    public ObservableCollection<ContractorDto> Contractors { get; }

    public ICommand LoadContractorsCommand { get; }
    public ICommand UpdateCommand { get; }

    private async Task LoadContractorsAsync()
    {
        var contractors = await _apiClient.GetContractorsAsync();

        Application.Current.Dispatcher.Invoke(() =>
        {
            Contractors.Clear();
            foreach (var c in contractors)
                Contractors.Add(c);

            SelectedContractor = Contractors.FirstOrDefault(c => c.Id == Document.ContractorId);
        });
    }

    private async Task UpdateDocumentAsync()
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

        Document.ContractorId = SelectedContractor.Id;
        Document.ContractorName = SelectedContractor.Name;

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

    public event EventHandler? RequestClose;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
