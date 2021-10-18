using ChatClient.Commands.CustomViewsCommands;
using ChatClient.ViewModels;
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

namespace ChatClient.Views.AdditionalViews
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class UserInfoView : UserControl
    {
        public UserInfoView()
        {
            InitializeComponent();

            _openUserInfoCommand = new OpenUserInfoWIndowCommand((ChatViewModel)((MainViewModel)Application.Current.MainWindow.DataContext).CurrentViewModel);
        }

        private readonly ICommand _openUserInfoCommand;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid grid)
            {
                if (grid != fullCoveredGrid)
                {
                    e.Handled = true;
                }
                else
                {
                    object[] parameters = new object[1];
                    parameters[0] = "false";
                    _openUserInfoCommand.Execute(parameters);
                }
            }
        }
    }
}