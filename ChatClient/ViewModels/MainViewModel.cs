using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel(ChatViewModel chatViewModel)
        {
            ChatViewModel = chatViewModel;
        }

        public ChatViewModel ChatViewModel;
    }
}
