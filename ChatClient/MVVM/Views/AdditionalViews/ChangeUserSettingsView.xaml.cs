using ChatClient.Commands.CustomViewsCommands;
using ChatClient.MVVM.ViewModels.ChatMainViewModels;
using SharedItems.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient.MVVM.Views.AdditionalViews
{
    /// <summary>
    /// Interaction logic for ChangeUserSettingsView.xaml
    /// </summary>
    public partial class ChangeUserSettingsView : UserControl
    {
        public ChangeUserSettingsView()
        {
            InitializeComponent();
            _changeUserSettingsCommand = new ChangeUserSettingsCommand((ChatViewModel)((MainViewModel)Application.Current.MainWindow.DataContext).CurrentViewModel);
        }

        private readonly ICommand _changeUserSettingsCommand;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid grid)
            {
                if (grid == mainGrid)
                {
                    _changeUserSettingsCommand.Execute(ChangeSettingsType.None);
                }

                e.Handled = true;
            }
        }

        private void UsernameTextControl_TextOrPasswordChanged(object sender, RoutedEventArgs e)
        {
            submitUsername.IsEnabled = !Validation.GetHasError(username)
                && username.Text != string.Empty
                && currentUsernamePassword.Password != null
                && currentUsernamePassword.Password != string.Empty;
        }

        private void EmailTextControl_TextOrPasswordChanged(object sender, RoutedEventArgs e)
        {
            submitEmail.IsEnabled = !Validation.GetHasError(email)
                && email.Text != string.Empty
                && currentEmailPassword.Password != null
                && currentEmailPassword.Password != string.Empty;
        }

        private void PasswordTextControl_TextOrPasswordChanged(object sender, RoutedEventArgs e)
        {
            submitPassword.IsEnabled = !Validation.GetHasError(newPassword)
                && !Validation.GetHasError(newPasswordConfirm)
                && currentPassword.Password != null
                && currentPassword.Password != string.Empty;
        }
    }
}
