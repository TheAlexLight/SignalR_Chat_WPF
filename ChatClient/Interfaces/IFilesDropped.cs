using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Interfaces
{
    public interface IFilesDropped
    {
        void OnFilesDropped(string[] files);
    }
}
