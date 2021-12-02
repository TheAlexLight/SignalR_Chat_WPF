using ChatClient.Factories.ViewModelFactories;
using ChatClient.Interfaces;
using ChatClient.Interfaces.BaseConfiguration;
using ChatClient.Interfaces.Factories;
using ChatClient.MVVM.ViewModels.BaseViewModels;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using ChatClient.Services;
using ChatClient.Services.BaseConfiguration;
using ChatClient.Services.ConcreteConfiguration;
using ChatClient.Services.Stores;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using SharedItems.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            IWindowConfigurationService service = serviceProvider.GetRequiredService<IWindowConfigurationService>();
            Window window = serviceProvider.GetRequiredService<Window>();

            window = service.SetWindowStartupData(
                     window: window,
                     top: 80,
                     left: 425,
                     width: 385,
                     height: 385);

            window.Show();
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<HubConnectionBuilder>();
            services.AddSingleton<WindowConfigurationService>();
            services.AddSingleton<ChatBaseConfiguration>();

            services.AddSingleton<ISignalRChatService, SignalRChatService>();
            services.AddSingleton<IWindowConfigurationService, WindowConfigurationService>();
            services.AddSingleton<INavigator, NavigationStore>();

            services.AddSingleton<IViewModelAbstractFactory, ViewModelFactory>();
            services.AddSingleton<IViewModelConcreteFactory<LoginViewModel>, LoginViewModelFactory>();
            services.AddSingleton<IViewModelConcreteFactory<RegistrationViewModel>, RegistrationViewModelFactory>();
            services.AddSingleton<IViewModelConcreteFactory<ChatViewModel>, ChatViewModelFactory>();
                      
            services.AddScoped<MainViewModel>();
            services.AddScoped<ChatViewModelBase, LoginViewModel>();

            services.AddScoped<Window>(s=> new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}
