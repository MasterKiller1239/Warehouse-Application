using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client.ViewModels.Contractors
{
    public class ContractorsViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private RelayCommand _editCommand;
        public ObservableCollection<ContractorDto> Contractors { get; } = new();
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get => _editCommand; }

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

        public ContractorsViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
            AddCommand = new RelayCommand(OnAdd);
            _editCommand = new RelayCommand(OnEdit, () => SelectedContractor != null);
            LoadContractors();
        }

        private async void LoadContractors()
        {
            Contractors.Clear();
            var contractors = await _apiClient.GetContractorsAsync();
            foreach (var contractor in contractors)
                Contractors.Add(contractor);
        }

        private void OnAdd()
        {
            var view = new Views.Contractors.AddContractorView(_apiClient);
            view.ShowDialog();
            LoadContractors();
        }

        private void OnEdit()
        {
            if (SelectedContractor == null) return;
            var view = new Views.Contractors.EditContractorView(_apiClient, SelectedContractor);
            view.ShowDialog();
            LoadContractors();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
