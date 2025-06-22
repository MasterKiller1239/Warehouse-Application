using Client.Dtos;
using Client.Services.Interfaces;
using System.Windows;

namespace Client.Views.Documents
{
    public partial class EditDocumentView : Window
    {

        public EditDocumentView()
        {
            InitializeComponent();
        }

        public EditDocumentView(EditDocumentViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Podpięcie zdarzenia zamknięcia z ViewModelu do Window
            viewModel.RequestClose += (result) =>
            {
                this.DialogResult = result;
                this.Close();
            };
        }
    }
}
