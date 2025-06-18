using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.DocumentItems;
using System.Windows;

namespace Client.Views.DocumentDetails
{
    public partial class EditDocumentItemView : Window
    {
        public EditDocumentItemView(DocumentItemDto item, IApiClient apiClient, IMessageService messageService)
        {
            InitializeComponent();

            var vm = new EditDocumentItemViewModel(item, apiClient, messageService);
            vm.RequestClose += (s, e) => this.Close();

            DataContext = vm;
        }
    }
}
