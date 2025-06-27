using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.DocumentItems;
using System.Windows;

namespace Client.Views.DocumentDetails
{
    public partial class EditDocumentItemView : Window
    {
        public EditDocumentItemView(EditDocumentItemViewModel vm)
        {
            InitializeComponent();

            vm.RequestClose += (result) =>
            {
                this.DialogResult = result;
                this.Close();
            };
            vm.CloseAction = Close;
            DataContext = vm;
        }
    }
}
