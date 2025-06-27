using Client.Dtos;
using Client.Services.Interfaces;
using System.Windows;

namespace Client.Views.DocumentDetails
{
    public partial class DocumentDetailsView : Window
    {
        public DocumentDetailsView(DocumentDetailsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
