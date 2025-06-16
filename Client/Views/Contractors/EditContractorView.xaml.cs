using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Contractors;
using System.Windows;

namespace Client.Views.Contractors
{
    public partial class EditContractorView : Window
    {
        public EditContractorView(IApiClient apiClient, ContractorDto contractor)
        {
            InitializeComponent();
            var vm = new EditContractorViewModel(apiClient, contractor, this);
            vm.CloseAction = Close;
            DataContext = vm;
        }
    }
}
