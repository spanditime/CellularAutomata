using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.MVVM.ViewModel
{
    class EditorViewModel
    {
        public MainViewModel VMController { get; private set; }
        public EditorViewModel(MainViewModel Controller)
        {
            VMController = Controller;
        }
    }
}
