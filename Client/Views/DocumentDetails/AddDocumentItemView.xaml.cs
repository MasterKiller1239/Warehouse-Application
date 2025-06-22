using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Contractors;
using Client.ViewModels.Documents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Views.DocumentDetails
{
    public partial class AddDocumentItemView : Window
    {
        public AddDocumentItemView(AddDocumentItemViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.RequestClose += (result) =>
            {
                this.DialogResult = result;
                this.Close();
            };
        }
    }
}
