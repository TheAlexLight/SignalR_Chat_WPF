using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.ViewModels;
using EntityFramework.DbContexts;
using EntityFramework.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
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

            NavigationStore navigationStore = new();

            navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);

            MainWindow window = new()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            using (AuthorizationDbContext db = new AuthorizationDbContext())
            {
                db.Database.Migrate();

                db.Add(new User
                {
                    Username = "123"
                });
                db.Add(new User
                {
                    Username = "321"
                });

                db.SaveChanges();

                foreach (var item in db.Users)
                {
                    MessageBox.Show(item.Username);
                }
            }

            window.Show();
        }
    }
}
