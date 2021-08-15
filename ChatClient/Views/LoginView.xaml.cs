using System.Windows;
using System.Windows.Controls;

namespace ChatClient.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            Window window = Application.Current.MainWindow;
            window.Height = 385;
            window.Width = 385;
        }

        private void loginField_TextOrPasswordChanged(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = pwBoxPassword.Password != null && pwBoxPassword.Password.Length > 0 && txtUsername.Text.Length > 0;
        }
    }
}
