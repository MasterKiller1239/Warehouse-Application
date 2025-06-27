using Client.Services.Interfaces;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.Utilities;
using Client.Views.Contractors;
using Client.Views.Documents;
using System.Windows.Input;

namespace Client.ViewModels.Main
{
    public class MainWindowViewModel
    {
        #region Fields
        private readonly IDocumentsViewModelFactory _documentsVmFactory;
        private readonly IContractorsViewModelFactory _contractorsVmFactory;
        #endregion

        #region Commands
        public ICommand OpenDocumentsCommand { get; }
        public ICommand OpenContractorsCommand { get; }
        #endregion

        #region Constructor
        public MainWindowViewModel(
            IDocumentsViewModelFactory documentsVmFactory,
            IContractorsViewModelFactory contractorsVmFactory)
        {
            _documentsVmFactory = documentsVmFactory ?? throw new ArgumentNullException(nameof(documentsVmFactory));
            _contractorsVmFactory = contractorsVmFactory ?? throw new ArgumentNullException(nameof(contractorsVmFactory));

            OpenDocumentsCommand = new RelayCommand(OpenDocumentsView);
            OpenContractorsCommand = new RelayCommand(OpenContractorsView);
        }
        #endregion

        #region Private Methods
        private void OpenDocumentsView()
        {
            var viewModel = _documentsVmFactory.Create();
            if (viewModel == null) return;

            var view = new DocumentsView(viewModel);
            view.Show();
        }

        private void OpenContractorsView()
        {
            var viewModel = _contractorsVmFactory.Create();
            if (viewModel == null) return;

            var view = new ContractorsView(viewModel);
            view.Show();
        }
        #endregion
    }
}