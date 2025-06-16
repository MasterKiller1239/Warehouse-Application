using Client.Services.Interfaces;
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

            // Rejestracja ApiClient i innych serwisów
            serviceCollection.AddHttpClient<IApiClient, ApiClient>();

            // Rejestracja widoków - jeśli chcesz też wstrzykiwać do nich zależności
            serviceCollection.AddTransient<DocumentsView>();
            serviceCollection.AddTransient<ContractorsView>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(e);
        }
    }

}
