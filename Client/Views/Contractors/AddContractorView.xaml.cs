using Client.Services.Interfaces;
using Client.ViewModels.Contractors;
using System.Windows;

namespace Client.Views.Contractors
{
    public partial class AddContractorView : Window
    {
        public AddContractorView(IApiClient apiClient, IMessageService _messageService)
        {
            InitializeComponent();

            var vm = new AddContractorViewModel(apiClient, _messageService);
            vm.CloseAction = Close;
            DataContext = vm;
        }

        private void ViewModel_RequestClose(object? sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (DataContext is AddContractorViewModel vm)
            {
                vm.RequestClose += (_, _) =>
                {
                    DialogResult = true;
                    Close();
                };
            }
        }
    }

}
