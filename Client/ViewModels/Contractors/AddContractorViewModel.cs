using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModels.Contractors
{
    public class AddContractorViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;

        public AddContractorViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
            _addCommand = new RelayCommand(async () => await AddContractor(), CanAdd);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }

        private string _symbol;
        public string Symbol
        {
            get => _symbol;
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged();
                    _addCommand.RaiseCanExecuteChanged(); 
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                    _addCommand.RaiseCanExecuteChanged(); 
                }
            }
        }

        private RelayCommand _addCommand;
        public ICommand AddCommand { get => _addCommand; }
        public ICommand CancelCommand { get; }

        public Action? CloseAction { get; set; }

        private bool CanAdd() => !string.IsNullOrWhiteSpace(Symbol) && !string.IsNullOrWhiteSpace(Name);
        private async Task AddContractor()
        {
            try
            {
                var contractor = new ContractorDto { Symbol = Symbol.Trim(), Name = Name.Trim() };
                await _apiClient.AddContractorAsync(contractor);
                MessageBox.Show("Contractor added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add contractor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
