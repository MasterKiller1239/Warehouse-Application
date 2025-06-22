using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Views.Documents
{
    public partial class AddDocumentView : Window
    {
        public AddDocumentView(IApiClient apiClient, IMessageService messageService)
        {
            InitializeComponent();

            var vm = new AddDocumentViewModel(apiClient, messageService);
            vm.RequestClose += (s, e) => Close();
            vm.CloseAction = Close;
            DataContext = vm;
        }
    }

}
