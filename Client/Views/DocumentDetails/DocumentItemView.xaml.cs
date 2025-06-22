using Client.Dtos;
using System.Windows;
namespace Client.Views.DocumentDetails
{
    public partial class DocumentItemView : Window
    {
        public DocumentItemView(DocumentItemDto item)
        {
            InitializeComponent();
            DataContext = new DocumentItemViewModel(item);
        }
    }

}
