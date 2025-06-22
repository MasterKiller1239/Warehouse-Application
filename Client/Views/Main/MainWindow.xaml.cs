using System.Windows;
using Client.ViewModels.Main;

namespace Client.Views.Main
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

    }
}
