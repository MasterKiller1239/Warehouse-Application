using Client.Dtos;
using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories;
using Client.Utilities;
using Client.Views.DocumentDetails;
using Client.Views.Documents;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Client.ViewModels.Documents
{
    public class DocumentsViewModel : INotifyPropertyChanged
    {
        #region Fields
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly IAddDocumentViewModelFactory _addDocumentVmFactory;
        private readonly IEditDocumentViewModelFactory _editDocumentVmFactory;
        private readonly IDocumentDetailsViewModelFactory _documentDetailsVmFactory;

        private DocumentDto? _selectedDocument;
        private List<DocumentDto> _allDocuments = new();
        private string _searchText;
        private ListSortDirection _lastSortDirection = ListSortDirection.Ascending;
        private string? _lastSortColumn = null;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action<bool> RequestClose;
        #endregion

        #region Properties
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

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand LoadDocumentsCommand { get; }
        public ICommand OpenDetailsCommand => _openDetailsCommand;
        private readonly RelayCommand _openDetailsCommand;

        public ICommand AddDocumentCommand { get; }

        public ICommand EditDocumentCommand => _editDetailsCommand;
        private readonly RelayCommand _editDetailsCommand;

        public ICommand SearchCommand => _searchCommand;
        private readonly RelayCommand _searchCommand;

        public RelayCommand<string> SortCommand { get; }
        #endregion

        #region Constructor
        public DocumentsViewModel(
            IApiClient apiClient,
            IMessageService messageService,
            IAddDocumentViewModelFactory addDocumentVmFactory,
            IEditDocumentViewModelFactory editDocumentVmFactory,
            IDocumentDetailsViewModelFactory documentDetailsVmFactory)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            _addDocumentVmFactory = addDocumentVmFactory;
            _editDocumentVmFactory = editDocumentVmFactory;
            _documentDetailsVmFactory = documentDetailsVmFactory;

            LoadDocumentsCommand = new RelayCommand(async () => await LoadDocumentsAsync());
            _openDetailsCommand = new RelayCommand(OpenDetails, () => SelectedDocument != null);
            AddDocumentCommand = new RelayCommand(async () => await AddDocument());
            _editDetailsCommand = new RelayCommand(async () => await EditDocument(), () => SelectedDocument != null);
            SortCommand = new RelayCommand<string>(SortDocuments);
            _searchCommand = new RelayCommand(ApplyFilter);

            // Load documents initially
            _ = LoadDocumentsAsync();
        }
        #endregion

        #region Public Methods
        public async Task LoadDocumentsAsync()
        {
            var docs = await _apiClient.GetDocumentsAsync();
            Documents.Clear();
            foreach (var d in docs)
                Documents.Add(d);
            _allDocuments = docs.ToList();
        }

        public void ApplyFilter()
        {
            Documents.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allDocuments
                : _allDocuments
                    .Where(c => c.Symbol.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            foreach (var contractor in filtered)
                Documents.Add(contractor);
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
        #endregion

        #region Private Methods
        private void OpenDetails()
        {
            if (SelectedDocument != null)
            {
                var detailsView = _documentDetailsVmFactory.Create(SelectedDocument, async () => await LoadDocumentsAsync());
                var view = new DocumentDetailsView(detailsView);
                view.Show();
            }
        }

        private async Task AddDocument()
        {
            var viewModel = _addDocumentVmFactory.Create();
            var view = new AddDocumentView(viewModel);

            view.ShowDialog();

            if (view.DialogResult == true)
            {
                await LoadDocumentsAsync();
                ApplyFilter();
            }
        }

        private async Task EditDocument()
        {
            if (SelectedDocument != null)
            {
                var viewModel = _editDocumentVmFactory.Create(SelectedDocument);
                var editView = new EditDocumentView(viewModel);

                viewModel.RequestClose += (success) =>
                {
                    if (success) _ = LoadDocumentsAsync();
                    ApplyFilter();
                };

                editView.ShowDialog();
            }
            else
            {
                _messageService.ShowWarning("Please select a document to edit", "Warning");
            }
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}