using Client.Dtos;
using Client.Services.Interfaces;
using System.Windows;

namespace Client.Views.DocumentDetails
{
    public partial class DocumentDetailsView : Window
    {
        public DocumentDetailsView(DocumentDto document, IApiClient apiClient)
        {
            InitializeComponent();
            DataContext = new DocumentDetailsViewModel(document, apiClient, this);
        }
    }
}
