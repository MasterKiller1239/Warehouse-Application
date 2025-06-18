﻿using Client.Services.Interfaces;
using Client.Services;
using Client.Views;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Client.Views.Contractors;
using Client.Views.Documents;
namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient<IApiClient, ApiClient>();
            serviceCollection.AddSingleton<IMessageService, MessageBoxService>();
            serviceCollection.AddTransient<DocumentsView>();
            serviceCollection.AddTransient<ContractorsView>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(e);
        }
    }

}
