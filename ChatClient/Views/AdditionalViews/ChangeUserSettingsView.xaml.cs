using ChatClient.Commands.CustomViewsCommands;
using ChatClient.ViewModels;
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

namespace ChatClient.Views.AdditionalViews
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
                    //object[] values = new object[1];
                    //values[0] = ChangeSettingsType.None;
                    _changeUserSettingsCommand.Execute(ChangeSettingsType.None);
                }

                e.Handled = true;
            }
        }
    }
}
