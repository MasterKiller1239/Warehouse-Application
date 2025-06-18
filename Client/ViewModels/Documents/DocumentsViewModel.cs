using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using Client.Views.DocumentDetails;
using Client.Views.Documents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Client.ViewModels.Documents
{
    public class DocumentsViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private DocumentDto? _selectedDocument;
        private RelayCommand _searchCommand;

        public ObservableCollection<DocumentDto> Documents { get; } = new ObservableCollection<DocumentDto>();

        public DocumentDto? SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                if (_selectedDocument != value)
                {
                    _selectedDocument = value;
                    OnPropertyChanged();
                    _openDetailsCommand.RaiseCanExecuteChanged();
                    _editDetailsCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand LoadDocumentsCommand { get; }
        public ICommand OpenDetailsCommand => _openDetailsCommand;

        public RelayCommand _openDetailsCommand;
        public ICommand AddDocumentCommand { get; }
        public ICommand EditDocumentCommand => _editDetailsCommand;
        public RelayCommand _editDetailsCommand;
        public ICommand SearchCommand => _searchCommand;
        public event PropertyChangedEventHandler? PropertyChanged;

        private ListSortDirection _lastSortDirection = ListSortDirection.Ascending;
        private string? _lastSortColumn = null;

        public RelayCommand<string> SortCommand { get; }
        private string _symbolFilter = string.Empty;
        public string SymbolFilter
        {
            get => _symbolFilter;
            set
            {
                if (_symbolFilter != value)
                {
                    _symbolFilter = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public DocumentsViewModel(IApiClient apiClient, IMessageService messageService)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            LoadDocumentsCommand = new RelayCommand(async () => await LoadDocumentsAsync());
            _openDetailsCommand = new RelayCommand(OpenDetails, () => SelectedDocument != null);
            AddDocumentCommand = new RelayCommand(AddDocument);
            _editDetailsCommand = new RelayCommand(EditDocument, () => SelectedDocument != null);
            SortCommand = new RelayCommand<string>(SortDocuments);
            _searchCommand = new RelayCommand(ApplyFilter);
            // Load documents initially
            _ = LoadDocumentsAsync();
        }
        private List<DocumentDto> _allDocuments = new(); 

       private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        private async Task LoadDocumentsAsync()
        {
            var docs = await _apiClient.GetDocumentsAsync();
            Documents.Clear();
            foreach (var d in docs)
                Documents.Add(d);
            _allDocuments = docs.ToList();
            //await  Application.Current.Dispatcher.InvokeAsync(() =>
            //{
               
            //});
        }
        private void ApplyFilter()
        {
            Documents.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allDocuments
                : _allDocuments
                    .Where(c => c.Symbol.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            foreach (var contractor in filtered)
                Documents.Add(contractor);

           SortDocuments("Symbol");
        }
        private void OpenDetails()
        {
            if (SelectedDocument != null)
            {
                var detailsView = new DocumentDetailsView(SelectedDocument, _apiClient, _messageService);
                detailsView.Show();
            }
        }

        private void AddDocument()
        {
            var addView = new AddDocumentView(_apiClient, _messageService);
            addView.Closed += async (s, e) => await LoadDocumentsAsync();
            addView.ShowDialog();
        }

        private void EditDocument()
        {
            if (SelectedDocument != null)
            {
                var editView = new EditDocumentView(_apiClient, SelectedDocument,_messageService);
                editView.Closed += async (s, e) => await LoadDocumentsAsync();
                editView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a document to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void SortDocuments(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return;

            var collectionView = CollectionViewSource.GetDefaultView(Documents);

            if (_lastSortColumn == columnName)
            {
                _lastSortDirection = _lastSortDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }
            else
            {
                _lastSortColumn = columnName;
                _lastSortDirection = ListSortDirection.Ascending;
            }

            collectionView.SortDescriptions.Clear();
            collectionView.SortDescriptions.Add(new SortDescription(columnName, _lastSortDirection));
            collectionView.Refresh();
        }
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
