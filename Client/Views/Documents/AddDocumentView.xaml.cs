using Client.ViewModels.Documents;
using System.Windows;

namespace Client.Views.Documents
{
    public partial class AddDocumentView : Window
    {
        public AddDocumentView()
        {
            InitializeComponent();

        }
        public AddDocumentView(AddDocumentViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.RequestClose += (result) =>
            {
                DialogResult = result;
                Close();
            };
        }
    }

}
