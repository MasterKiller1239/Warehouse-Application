using Client.Dtos;
using Client.Services;
using Client.Services.Interfaces;
using Client.ViewModels.Documents;
using Client.Views.DocumentDetails;
using Client.Views.Documents;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views.Documents
{
    public partial class DocumentsView : Window
    {
        public DocumentsView(IApiClient apiClient)
        {
            InitializeComponent();
            DataContext = new DocumentsViewModel(apiClient);
        }

        private void GridViewColumnHeader_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is DocumentsViewModel vm)
            {
                if (sender is TextBlock tb)
                {
                    string column = tb.Text; // tekst w nagłówku np. "Symbol", "Date" itp.

                    // Mapuj nagłówek na nazwę właściwości w DocumentDto
                    string propertyName = column switch
                    {
                        "Symbol" => nameof(DocumentDto.Symbol),
                        "Date" => nameof(DocumentDto.Date),
                        "Contractor" => nameof(DocumentDto.ContractorName),
                        _ => ""
                    };

                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        vm.SortCommand.Execute(propertyName);
                    }
                }
            }
        }
    }
}
