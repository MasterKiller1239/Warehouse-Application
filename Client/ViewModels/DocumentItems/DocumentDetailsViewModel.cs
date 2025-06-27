using Client.Dtos;
using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories;
using Client.Utilities;
using Client.Views.DocumentDetails;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class DocumentDetailsViewModel : INotifyPropertyChanged
{
    #region Fields
    private readonly IApiClient _apiClient;
    private readonly IMessageService _messageService;
    private readonly IAddDocumentItemViewModelFactory _addItemVmFactory;
    private readonly IEditDocumentItemViewModelFactory _editItemVmFactory;
    private readonly Action? _onDocumentUpdated;

    private DocumentDto _document;
    private DocumentItemDto? _selectedItem;
    private ObservableCollection<DocumentItemDto> _items;

    private string? _currentSortColumn;
    private bool _sortAscending = true;
    #endregion

    #region Events
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region Properties
    public string Symbol => _document.Symbol;
    public string Date => _document.Date.ToShortDateString();
    public string ContractorName => _document.Contractor.Name;

    public ObservableCollection<DocumentItemDto> Items
    {
        get => _items;
        private set
        {
            _items = value;
            OnPropertyChanged();
        }
    }

    public DocumentItemDto? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged();
                ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
            }
        }
    }
    #endregion

    #region Commands
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand SortCommand { get; }
    #endregion

    #region Constructor
    public DocumentDetailsViewModel(
        DocumentDto document,
        IApiClient apiClient,
        IMessageService messageService,
        IAddDocumentItemViewModelFactory addItemVmFactory,
        IEditDocumentItemViewModelFactory editItemVmFactory,
        Action? onDocumentUpdated = null)
    {
        _document = document;
        _apiClient = apiClient;
        _messageService = messageService;
        _addItemVmFactory = addItemVmFactory;
        _editItemVmFactory = editItemVmFactory;
        _onDocumentUpdated = onDocumentUpdated;

        Items = new ObservableCollection<DocumentItemDto>(document.Items ?? new List<DocumentItemDto>());

        AddCommand = new RelayCommand(OpenAddItem);
        EditCommand = new RelayCommand(OpenEditItem, () => SelectedItem != null);
        SortCommand = new RelayCommand<string>(SortItems, column => !string.IsNullOrEmpty(column));
    }
    #endregion

    #region Public Methods
    // (Placeholder for any public methods if needed)
    #endregion

    #region Private Methods
    private void OpenAddItem()
    {
        var vm = _addItemVmFactory.Create(_document.Id);
        var view = new AddDocumentItemView(vm);

        vm.RequestClose += success =>
        {
            if (success)
                RefreshDocument();
        };

        view.ShowDialog();
    }

    private void OpenEditItem()
    {
        if (SelectedItem is null)
            return;

        var vm = _editItemVmFactory.Create(SelectedItem);
        var view = new EditDocumentItemView(vm);
        vm.RequestClose += success =>
        {
            if (success)
                RefreshDocument();
        };
        view.ShowDialog();
    }

    private async void RefreshDocument()
    {
        try
        {
            var updated = await _apiClient.GetDocumentAsync(_document.Id);
            _document = updated;
            RefreshItems();
            _onDocumentUpdated?.Invoke();
        }
        catch (Exception ex)
        {
            _messageService.ShowError($"Error refreshing document: {ex.Message}");
        }
    }

    private void RefreshItems()
    {
        Items = new ObservableCollection<DocumentItemDto>(_document.Items);
    }

    private void SortItems(string propertyName)
    {
        if (_currentSortColumn == propertyName)
        {
            _sortAscending = !_sortAscending;
        }
        else
        {
            _sortAscending = true;
            _currentSortColumn = propertyName;
        }

        var sorted = _sortAscending
            ? Items.OrderBy(item => GetPropertyValue(item, propertyName))
            : Items.OrderByDescending(item => GetPropertyValue(item, propertyName));

        Items = new ObservableCollection<DocumentItemDto>(sorted);
    }

    private static object? GetPropertyValue(DocumentItemDto item, string propertyName)
    {
        return typeof(DocumentItemDto)
            .GetProperty(propertyName)?
            .GetValue(item);
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    #endregion
}