﻿using System.Windows;
using Client.Services.Interfaces;
using Client.ViewModels.Contractors;

namespace Client.Views.Contractors
{
    public partial class ContractorsView : Window
    {
        public ContractorsView(IApiClient apiClient, IMessageService messageService)
        {
            InitializeComponent();
            DataContext = new ContractorsViewModel(apiClient, messageService);
        }
    }
}