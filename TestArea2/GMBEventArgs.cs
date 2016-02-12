using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestArea2
{
    public class GMBEventArgs : EventArgs
    {
        public string CurrentPl {get; private set;}

        public GMBEventArgs(string _newPl)
        {
            CurrentPl = _newPl;
        }

    }
}
