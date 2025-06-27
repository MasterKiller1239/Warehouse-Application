﻿using Client.Services.Interfaces;
using Client.Services;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Client.Views.Contractors;
using Client.Views.Documents;
using Client.ViewModels.Main;
using Client.Views.Main;
using Client.ViewModels.Contractors;
using Client.Services.Interfaces.IFactories;
using Client.Services.Factories;
using Client.Services.Interfaces.IFactories.Contractors;
using Client.Services.Factories.Contractors;
using Client.ViewModels.Documents;
using Client.Views.DocumentDetails;
using System.Net.Http;
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

            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainWindowViewModel>();

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            services.AddSingleton<IApiClient>(new ApiClient(new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:5001/api/")
            }));
            services.AddSingleton<IMessageService, MessageBoxService>();

            services.AddSingleton<IContractorsViewModelFactory, ContractorsViewModelFactory>();
            services.AddSingleton<IDocumentsViewModelFactory, DocumentsViewModelFactory>();
            services.AddSingleton<IContractorsViewModelFactory, ContractorsViewModelFactory>();
            services.AddSingleton<IAddContractorViewFactory, AddContractorViewFactory>();
            services.AddSingleton<IEditContractorViewFactory, EditContractorViewFactory>();
            services.AddSingleton<IAddDocumentViewModelFactory, AddDocumentViewModelFactory>();
            services.AddSingleton<IEditDocumentViewModelFactory, EditDocumentViewModelFactory>();
            services.AddSingleton<IEditDocumentItemViewModelFactory, EditDocumentItemViewModelFactory>();
            services.AddSingleton<IDocumentDetailsViewModelFactory, DocumentDetailsViewModelFactory>();
            services.AddSingleton<IAddDocumentItemViewModelFactory, AddDocumentItemViewModelFactory>();

            services.AddTransient<MainWindow>();
            services.AddTransient<DocumentsView>();
            services.AddTransient<AddDocumentView>();
            services.AddTransient<EditDocumentView>();
            services.AddTransient<DocumentDetailsView>();
            services.AddTransient<AddDocumentItemView>();
            services.AddTransient<EditDocumentItemView>();
            services.AddTransient<ContractorsView>();
            services.AddTransient<AddContractorView>();
            services.AddTransient<EditContractorView>();
            services.AddTransient<AddContractorViewModel>();
            services.AddTransient<EditContractorViewModel>();
        }
    }

}
