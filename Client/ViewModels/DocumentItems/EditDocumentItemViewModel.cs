using Client.Dtos;
using Client.Services.Interfaces;
using Client.Utilities;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client.ViewModels.DocumentItems
{
    public class EditDocumentItemViewModel : INotifyPropertyChanged
    {
        #region Fields
        private readonly IApiClient _apiClient;
        private readonly DocumentItemDto _item;
        private readonly IMessageService _messageService;

        private string _productName;
        private string _unit;
        private string _quantityText;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;

        public event Action<bool> RequestClose;
        #endregion

        #region Properties
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

        public Action? CloseAction { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructor
        public EditDocumentItemViewModel(
            DocumentItemDto item,
            IApiClient apiClient,
            IMessageService messageService)
        {
            _item = item;
            _apiClient = apiClient;
            _messageService = messageService;

            ProductName = _item.ProductName;
            Unit = _item.Unit;
            QuantityText = _item.Quantity.ToString(CultureInfo.InvariantCulture);

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }
        #endregion
        #region Public Methods
        public void Close(bool dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
        #endregion
        #region Private Methods
        private async Task SaveAsync()
        {
            if (!ValidateInput())
                return;

            UpdateItemFromInput();

            try
            {
                await _apiClient.UpdateDocumentItemAsync(_item);
                _messageService.ShowInfo("Item updated successfully.", "Success");
                RequestClose?.Invoke(true);
            }
            catch (Exception ex)
            {
                _messageService.ShowError($"An error occurred while updating the item: {ex.Message}", "Failure");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                _messageService.ShowWarning("Product name is required.", "Validation");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Unit))
            {
                _messageService.ShowWarning("Unit is required.", "Validation");
                return false;
            }

            if (!decimal.TryParse(QuantityText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal quantity) || quantity <= 0)
            {
                _messageService.ShowWarning("Please enter a valid positive number for quantity.", "Validation");
                return false;
            }

            return true;
        }

        private void UpdateItemFromInput()
        {
            _item.ProductName = ProductName.Trim();
            _item.Unit = Unit.Trim();
            _item.Quantity = decimal.Parse(QuantityText, CultureInfo.InvariantCulture);
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}