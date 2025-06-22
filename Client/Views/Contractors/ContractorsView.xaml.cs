using System.Windows;
using Client.Services.Interfaces;
using Client.ViewModels.Contractors;

namespace Client.Views.Contractors
{
    public partial class ContractorsView : Window
    {
        public ContractorsView(ContractorsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}