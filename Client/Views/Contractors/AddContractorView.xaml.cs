using Client.Services.Interfaces;
using Client.ViewModels.Contractors;
using System.Windows;

namespace Client.Views.Contractors
{
    public partial class AddContractorView : Window
    {
        public AddContractorView(IApiClient apiClient)
        {
            InitializeComponent();

            var vm = new AddContractorViewModel(apiClient);
            vm.CloseAction = Close;
            DataContext = vm;
        }
    }

}
