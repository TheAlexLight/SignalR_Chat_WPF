using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using SharedItems.Models;
using System;
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
            HubConnection connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/chat")
                    .WithAutomaticReconnect()
                    .Build();

            NavigationStore navigationStore = new();
            SignalRChatService chatService = new(connection);
            ChatViewModelBase viewModel = new LoginViewModel(navigationStore, chatService);

            navigationStore.CurrentViewModel = viewModel;

            MainWindow window = new()
            {
                DataContext = new MainViewModel(navigationStore),
                Top = 80,
                Left = 425,
                Width = 385,
                Height = 385
            };

            window.Show();
        }
    }
}
