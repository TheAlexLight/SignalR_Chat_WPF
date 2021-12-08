using ChatClient.Interfaces.Authorization;
using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.ViewModels
{
    public class UserCredentialsViewModel : ViewModelBase, IDataErrorInfo, IUserSettings
    {
        public UserCredentialsViewModel()
        {
            Password = string.Empty;
            PasswordConfirm = string.Empty;
        }

        private string _username;
        private string _email;

        private string _password;
        private string _passwordConfirm;

        private string _usernameSettings;
        private string _emailSettings;

        public string Error { get; }

        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;

                propertyName ??= string.Empty;

                if (propertyName != string.Empty
                        && (propertyName == nameof(PasswordConfirm) || propertyName == nameof(Password))
                        && PasswordConfirm != null && Password != null)
                {
                    if (!Password.Equals(PasswordConfirm, StringComparison.Ordinal))
                    {
                        result = "Passwords do not match";
                    }
                }

                return result;
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordConfirm));
            }
        }

        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Password));
            }
        }

        public string UsernameSettings
        {
            get => _usernameSettings;
            set
            {
                _usernameSettings = value;
                OnPropertyChanged();
            }
        }

        public string EmailSettings
        {
            get => _emailSettings;
            set
            {
                _emailSettings = value;
                OnPropertyChanged();
            }
        }
    }
}
