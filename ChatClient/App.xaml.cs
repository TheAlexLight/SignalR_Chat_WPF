using ChatClient.Services;
using ChatClient.Services.Interfaces;
using ChatClient.Stores;
using ChatClient.ViewModels;
using EntityFramework.DbContexts;
using EntityFramework.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            //HubConnection connection = new HubConnectionBuilder()
            //    .WithUrl("http://localhost:5000/chat")
            //    .Build();

            //ChatViewModel chatViewModel = ChatViewModel.CreateConnectedViewModel(new SignalRChatService(connection));

            //MainWindow window = new()
            //{
            //    DataContext = chatViewModel
            //};

            IAuthenticationService authentication = new AuthenticationService(new AccountDataService<Account>());
            authentication.Register("qwer@gmail.com", "qwer", "qwer", "qwer");

            NavigationStore navigationStore = new();

            navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);

            MainWindow window = new()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            window.Show();
        }
    }
}
