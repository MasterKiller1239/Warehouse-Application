using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.Utilities;
using Client.Views.Contractors;
using Client.Views.Documents;
using System.Windows.Input;

namespace Client.ViewModels.Main
{
    public class MainWindowViewModel
    {
        private readonly IDocumentsViewModelFactory _documentsVmFactory;
        private readonly IContractorsViewModelFactory _contractorsVmFactory;

        public ICommand OpenDocumentsCommand { get; }
        public ICommand OpenContractorsCommand { get; }

        public MainWindowViewModel(
            IDocumentsViewModelFactory documentsVmFactory,
            IContractorsViewModelFactory contractorsVmFactory)
        {
            _documentsVmFactory = documentsVmFactory;
            _contractorsVmFactory = contractorsVmFactory;

            OpenDocumentsCommand = new RelayCommand(OpenDocumentsView);
            OpenContractorsCommand = new RelayCommand(OpenContractorsView);
        }

        private void OpenDocumentsView()
        {
            var vm = _documentsVmFactory.Create();
            if (vm == null) return;
            var view = new DocumentsView(vm);
            view.Show();
        }

        private void OpenContractorsView()
        {
            var vm = _contractorsVmFactory.Create();
            if (vm == null) return;
            var view = new ContractorsView(vm);
            view.Show();
        }
    }
}