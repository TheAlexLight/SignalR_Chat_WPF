using SharedItems.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.BaseConfiguration
{
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        event Action CurrentViewModelChanged;
    }
}
