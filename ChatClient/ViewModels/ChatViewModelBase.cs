using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.ViewModels
{
   public class ChatViewModelBase : ViewModelBase
    {
        private string _errorMessage;
        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }
    }
}
