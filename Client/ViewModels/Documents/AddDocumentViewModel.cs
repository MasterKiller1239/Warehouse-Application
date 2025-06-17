using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Client.Utilities;
using Client.Dtos;

namespace Client.ViewModels.Documents
{
    public class AddDocumentViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;

        private string _symbol = string.Empty;
        private ContractorDto? _selectedContractor;
        private DateTime _date = DateTime.Now;

        public ObservableCollection<ContractorDto> Contractors { get; } = new ObservableCollection<ContractorDto>();

        public string Symbol
        {
            get => _symbol;
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? RequestClose;

        public AddDocumentViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            LoadContractorsAsync();
        }

        private async void LoadContractorsAsync()
        {
            try
            {
                var contractors = await _apiClient.GetContractorsAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Contractors.Clear();
                    foreach (var c in contractors)
                        Contractors.Add(c);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading contractors: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Symbol))
            {
                MessageBox.Show("Please enter a symbol.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var existing = await _apiClient.GetDocumentBySymbolAsync(Symbol.Trim());
                if (existing != null)
                {
                    MessageBox.Show($"A document with symbol '{Symbol}' already exists.", "Duplicate Document", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (SelectedContractor == null)
                {
                    MessageBox.Show("Please select a contractor.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newDocument = new DocumentDto
                {
                    Symbol = Symbol.Trim(),
                    Date = Date.ToUniversalTime(),
                    ContractorId = SelectedContractor.Id,
                    Contractor = SelectedContractor,
                    Items = new List<DocumentItemDto>()
                };

                await _apiClient.AddDocumentAsync(newDocument);

                MessageBox.Show("Document added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving document: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
