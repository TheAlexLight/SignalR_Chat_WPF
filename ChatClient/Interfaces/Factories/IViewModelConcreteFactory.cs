using SharedItems.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces.Factories
{
    public interface IViewModelConcreteFactory<T> where T : ViewModelBase
    {
        T CreateViewModel();
    }
}
