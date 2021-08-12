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
    /// Interaction logic for BindablePasswordBoxConfirm.xaml
    /// </summary>
    public partial class BindablePasswordBoxConfirm : UserControl
    {
        private bool _isPasswordConfirmChanging;

        public string PasswordConfirm
        {
            get => (string)GetValue(PasswordConfirmProperty);
            set => SetValue(PasswordConfirmProperty, value);
        }

        public static readonly DependencyProperty PasswordConfirmProperty =
           DependencyProperty.Register(nameof(PasswordConfirm), typeof(string), typeof(BindablePasswordBoxConfirm),
                   new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                           PasswordConfirmPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void PasswordConfirmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBoxConfirm)
            {
                passwordBoxConfirm.UpdatePasswordBoxConfirm();
            }
        }

        public BindablePasswordBoxConfirm()
        {
            InitializeComponent();
        }

        private void PasswordBoxConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordConfirmChanging = true;
            PasswordConfirm = passwordBoxConfirm.Password;
            _isPasswordConfirmChanging = false;
        }

        private void UpdatePasswordBoxConfirm()
        {
            if (!_isPasswordConfirmChanging)
            {
                passwordBoxConfirm.Password = PasswordConfirm;
            }
        }
    }
}
