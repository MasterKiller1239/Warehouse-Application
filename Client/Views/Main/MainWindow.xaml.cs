using Client.Views.Contractors;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Client.Views.Documents;

namespace Client.Views.Main
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenDocumentsView(object sender, RoutedEventArgs e)
        {
            var documentsView = App.ServiceProvider.GetService<DocumentsView>();
            documentsView.Owner = this;
            documentsView.Show();
        }

        private void OpenContractorsView(object sender, RoutedEventArgs e)
        {
            var contractorsView = App.ServiceProvider.GetService<ContractorsView>();
            contractorsView.Owner = this;
            contractorsView.Show();
        }
    }
}
