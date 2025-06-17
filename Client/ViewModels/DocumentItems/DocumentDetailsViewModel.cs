using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using Client.Views.DocumentDetails;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class DocumentDetailsViewModel : INotifyPropertyChanged
{
    private readonly IApiClient _apiClient;

    public DocumentDetailsViewModel(DocumentDto document, IApiClient apiClient, Window window)
    {
        _apiClient = apiClient;

        _document = document;
        Items = new ObservableCollection<DocumentItemDto>(document.Items ?? new());

        AddCommand = new RelayCommand(() => OpenAddItem(), () => true);
        EditCommand = new RelayCommand(() => OpenEditItem(), () => SelectedItem != null);
        SortCommand = new RelayCommand<string>(column => SortItems(column), column => !string.IsNullOrEmpty(column));
    }

    private DocumentDto _document;

    public string Symbol => _document.Symbol;
    public string Date => _document.Date.ToShortDateString();
    public string ContractorName => _document.Contractor.Name;
    public ICommand SortCommand { get; }

    private string? _currentSortColumn;
    private bool _sortAscending = true;
    private ObservableCollection<DocumentItemDto> _items;
    public ObservableCollection<DocumentItemDto> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged();
        }
    }


    private DocumentItemDto? _selectedItem;
    public DocumentItemDto? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
            ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
        }
    }

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }

    private async void OpenAddItem()
    {
        var addView = new AddDocumentItemView(_document.Id, _apiClient);
        addView.ShowDialog();

        var updated = await _apiClient.GetDocumentAsync(_document.Id);
        _document = updated;
        RefreshItems();
    }

    private async void OpenEditItem()
    {
        if (SelectedItem is null) return;

        var editView = new EditDocumentItemView(SelectedItem, _apiClient);
        editView.ShowDialog();

        var updated = await _apiClient.GetDocumentAsync(_document.Id);
        _document = updated;
        RefreshItems();
    }

    private void RefreshItems()
    {
        Items.Clear();
        foreach (var item in _document.Items)
            Items.Add(item);
    }
    private void SortItems(string propertyName)
    {
        if (_currentSortColumn == propertyName)
            _sortAscending = !_sortAscending;
        else
        {
            _sortAscending = true;
            _currentSortColumn = propertyName;
        }

        var sorted = _sortAscending
            ? Items.OrderBy(item => GetProperty(item, propertyName))
            : Items.OrderByDescending(item => GetProperty(item, propertyName));

        Items = new ObservableCollection<DocumentItemDto>(sorted);
        OnPropertyChanged(nameof(Items));
    }

    private object? GetProperty(DocumentItemDto item, string propertyName)
    {
        return typeof(DocumentItemDto).GetProperty(propertyName)?.GetValue(item);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
