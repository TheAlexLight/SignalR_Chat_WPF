using ChatClient.Services;
using ChatClient.Stores;
using ChatClient.ViewModels;
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

            window.Show();
        }
    }
}
