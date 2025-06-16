﻿using Client.Dtos;
using Client.Services.Interfaces;
using Client.ViewModels.Contractors;
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
        public AddDocumentItemView(int documentId, IApiClient apiClient)
        {
            InitializeComponent();
            var vm = new AddDocumentItemViewModel(documentId, apiClient, this);
            vm.CloseAction = Close;
            DataContext = vm;
        }
    }
}
