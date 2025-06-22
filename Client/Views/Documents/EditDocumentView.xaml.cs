using Client.Dtos;
using Client.Services.Interfaces;
using System.Windows;

namespace Client.Views.Documents
{
    public partial class EditDocumentView : Window
    {
        private readonly EditDocumentViewModel _viewModel;

        public EditDocumentView(IApiClient apiClient, DocumentDto document, IMessageService messageService)
        {
            InitializeComponent();
            _viewModel = new EditDocumentViewModel(apiClient, document, messageService);
            _viewModel.CloseAction = Close;
            _viewModel.RequestClose += (s, e) => this.Close();
            DataContext = _viewModel;
        }
    }
}
