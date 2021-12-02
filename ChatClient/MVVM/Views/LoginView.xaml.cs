using System.Windows;
using System.Windows.Controls;

namespace ChatClient.MVVM.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LoginField_TextOrPasswordChanged(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = pwBoxPassword.Password != null && pwBoxPassword.Password.Length > 0 && txtUsername.Text.Length > 0;
        }
    }
}
