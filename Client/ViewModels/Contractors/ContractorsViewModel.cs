using Client.Dtos;
using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client.ViewModels.Contractors
{
    public class ContractorsViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageService _messageService;
        private readonly IAddContractorViewFactory _addContractorViewFactory;
        private readonly IEditContractorViewFactory _editContractorViewFactory;
        private List<ContractorDto> _allContractors = new();

        private RelayCommand _editCommand;
        private RelayCommand _sortCommand;
        private RelayCommand _searchCommand;
        public ObservableCollection<ContractorDto> Contractors { get; } = new();
        public ICommand AddCommand { get; }
        public ICommand EditCommand => _editCommand;
        public ICommand SortCommand => _sortCommand;
        public ICommand SearchCommand => _searchCommand;

        private ContractorDto _selectedContractor;
        public ContractorDto SelectedContractor
        {
            get => _selectedContractor;
            set
            {
                _selectedContractor = value;
                OnPropertyChanged();
                _editCommand?.RaiseCanExecuteChanged();
            }
        }

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

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                OnPropertyChanged();
                ApplySorting();
            }
        }

        public ContractorsViewModel(IApiClient apiClient, IMessageService messageService, IAddContractorViewFactory addContractorViewFactory,
        IEditContractorViewFactory editContractorViewFactory)
        {
            _apiClient = apiClient;
            _messageService = messageService;
            _addContractorViewFactory = addContractorViewFactory;
            _editContractorViewFactory = editContractorViewFactory;
            AddCommand = new RelayCommand(OnAdd);
            _editCommand = new RelayCommand(OnEdit, () => SelectedContractor != null);
            _sortCommand = new RelayCommand(ApplySorting);
            _searchCommand = new RelayCommand(ApplySearch);
            LoadContractors();
        }

        private async void LoadContractors()
        {
            Contractors.Clear();
            _allContractors = await _apiClient.GetContractorsAsync();

            ApplySearch();
        }

        private void ApplySearch()
        {
            Contractors.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allContractors
                : _allContractors
                    .Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            foreach (var contractor in filtered)
                Contractors.Add(contractor);

            ApplySorting(); 
        }

        private void ApplySorting()
        {
            var sorted = SelectedSortOption switch
            {
                "NameAsc" => Contractors.OrderBy(c => c.Name).ToList(),
                "NameDesc" => Contractors.OrderByDescending(c => c.Name).ToList(),
                _ => Contractors.ToList()
            };

            Contractors.Clear();
            foreach (var c in sorted)
                Contractors.Add(c);
        }

        private void OnAdd()
        {
            var addView = _addContractorViewFactory.Create();
            addView.ShowDialog();
            LoadContractors();
        }
        private void OnEdit()
        {
            if (SelectedContractor == null) return;

            var editView = _editContractorViewFactory.Create(SelectedContractor);
            editView.ShowDialog();
            LoadContractors();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
