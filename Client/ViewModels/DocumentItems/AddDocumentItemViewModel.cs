using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class AddDocumentItemViewModel : INotifyPropertyChanged
{
    #region Fields
    private readonly int _documentId;
    private readonly IApiClient _apiClient;
    private readonly IMessageService _messageService;
    private readonly RelayCommand _addCommand;

    private string _productName = string.Empty;
    private string _unit = string.Empty;
    private string _quantityText = string.Empty;
    #endregion

    #region Events
    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<bool> RequestClose;
    #endregion

    #region Properties
    public string ProductName
    {
        get => _productName;
        set
        {
            _productName = value;
            OnPropertyChanged();
            _addCommand.RaiseCanExecuteChanged();
        }
    }

    public string Unit
    {
        get => _unit;
        set
        {
            _unit = value;
            OnPropertyChanged();
            _addCommand.RaiseCanExecuteChanged();
        }
    }

    public string QuantityText
    {
        get => _quantityText;
        set
        {
            _quantityText = value;
            OnPropertyChanged();
            _addCommand.RaiseCanExecuteChanged();
        }
    }

    public Action? CloseAction { get; set; }
    #endregion

    #region Commands
    public ICommand AddCommand => _addCommand;
    public ICommand CancelCommand { get; }
    #endregion

    #region Constructor
    public AddDocumentItemViewModel(
        int documentId,
        IApiClient apiClient,
        IMessageService messageService)
    {
        _documentId = documentId;
        _apiClient = apiClient;
        _messageService = messageService;

        _addCommand = new RelayCommand(async () => await AddItemAsync(), CanAdd);
        CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
    }
    #endregion

    #region Public Methods
    public void Close(bool dialogResult)
    {
        RequestClose?.Invoke(dialogResult);
    }
    #endregion

    #region Private Methods
    private bool CanAdd()
    {
        if (string.IsNullOrWhiteSpace(ProductName))
            return false;

        if (string.IsNullOrWhiteSpace(Unit))
            return false;

        if (!decimal.TryParse(QuantityText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal qty))
            return false;

        if (qty <= 0)
            return false;

        return true;
    }

    private async Task AddItemAsync()
    {
        var item = new DocumentItemDto
        {
            DocumentId = _documentId,
            ProductName = ProductName.Trim(),
            Unit = Unit.Trim(),
            Quantity = decimal.Parse(QuantityText, CultureInfo.InvariantCulture)
        };

        try
        {
            await _apiClient.AddDocumentItemAsync(item);
            _messageService.ShowInfo("Item added successfully.", "Success");
            Close(true);
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"An error occurred while adding the item: {ex.Message}", "Failure");
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    #endregion
}