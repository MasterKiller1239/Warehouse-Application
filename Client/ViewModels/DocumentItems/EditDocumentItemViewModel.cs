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
        private readonly IMessageService _messageService;
        public event PropertyChangedEventHandler? PropertyChanged;

        public EditDocumentItemViewModel(DocumentItemDto item, IApiClient apiClient, IMessageService messageService)
        {
            _item = item;
            _apiClient = apiClient;
            _messageService = messageService;
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
                _messageService.ShowWarning("Product name is required.", "Validation");
                return;
            }

            if (string.IsNullOrWhiteSpace(Unit))
            {
                _messageService.ShowWarning("Unit is required.", "Validation");
                return;
            }

            if (!decimal.TryParse(QuantityText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal quantity) || quantity <= 0)
            {
                _messageService.ShowWarning("Please enter a valid positive number for quantity.", "Validation");
                return;
            }

            _item.ProductName = ProductName.Trim();
            _item.Unit = Unit.Trim();
            _item.Quantity = quantity;

            try
            {
                await _apiClient.UpdateDocumentItemAsync(_item);
                _messageService.ShowInfo("Item updated successfully.", "Success");
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"An error occurred while updating the item: {ex.Message}", "Failure");
            }
        }

        public event EventHandler? RequestClose;

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
