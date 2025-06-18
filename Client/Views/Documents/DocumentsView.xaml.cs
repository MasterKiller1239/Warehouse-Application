using Client.Dtos;
using Client.Services;
using Client.Services.Interfaces;
using Client.ViewModels.Documents;
using Client.Views.DocumentDetails;
using Client.Views.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Views.Documents
{
    public partial class DocumentsView : Window
    {
        public DocumentsView(IApiClient apiClient, IMessageService messageService)
        {
            InitializeComponent();
            DataContext = new DocumentsViewModel(apiClient, messageService);
        }

        private void GridViewColumnHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && DataContext is DocumentsViewModel vm)
            {
                string? property = textBlock.Tag as string;
                if (!string.IsNullOrEmpty(property))
                {
                    vm.SortCommand.Execute(property);
                }
            }
        }

    }
}
