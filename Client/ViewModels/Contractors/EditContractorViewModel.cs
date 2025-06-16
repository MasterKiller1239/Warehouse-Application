using Client.Services.Interfaces;
using Client.Utilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Client.Dtos;

namespace Client.ViewModels.Contractors
{
    public class EditContractorViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;

        public string Symbol { get; set; }
        public string Name { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly int _contractorId;

        public Action? CloseAction { get; set; }
        public EditContractorViewModel(IApiClient apiClient, ContractorDto contractor, Window window)
        {
            _apiClient = apiClient;
            _contractorId = contractor.Id;

            Symbol = contractor.Symbol;
            Name = contractor.Name;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Symbol) || string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("All fields are required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updated = new ContractorDto
            {
                Id = _contractorId,
                Symbol = Symbol,
                Name = Name
            };

            try
            {
                await _apiClient.UpdateContractorAsync(updated);
                MessageBox.Show("Contractor updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
