using ChatClient.Enums;
using ChatClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces
{
    public interface IViewModelConcreteFactory<T> where T : ViewModelBase
    {
        T CreateViewModel();
    }
}
