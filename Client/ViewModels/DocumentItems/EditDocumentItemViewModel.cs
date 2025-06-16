using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Client.Dtos;
using Client.Utilities;

namespace Client.ViewModels.DocumentItems
{
    public class EditDocumentItemViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly DocumentItemDto _item;
        private string _productName;
        private string _unit;
        private string _quantityText;

        public event PropertyChangedEventHandler? PropertyChanged;

        public EditDocumentItemViewModel(DocumentItemDto item, IApiClient apiClient)
        {
            _item = item;
            _apiClient = apiClient;

            ProductName = _item.ProductName;
            Unit = _item.Unit;
            QuantityText = _item.Quantity.ToString(CultureInfo.InvariantCulture);
            SaveCommand = new RelayCommand(async () => await SaveAsync());
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged();
                }
            }
        }

        public string QuantityText
        {
            get => _quantityText;
            set
            {
                if (_quantityText != value)
                {
                    _quantityText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                MessageBox.Show("Product name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Unit))
            {
                MessageBox.Show("Unit is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(QuantityText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid positive number for quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _item.ProductName = ProductName.Trim();
            _item.Unit = Unit.Trim();
            _item.Quantity = quantity;

            try
            {
                await _apiClient.UpdateDocumentItemAsync(_item);
                MessageBox.Show("Item updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Zamknięcie okna po udanym zapisie, ale to już zrobi View — więc wywołaj event
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event EventHandler? RequestClose;

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
