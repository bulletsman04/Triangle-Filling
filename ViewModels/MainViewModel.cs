using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace ViewModels
{
    public class MainViewModel
    {
        public MenuViewModel MenuViewModel { get; set; }
        public CanvasViewModel CanvasViewModel { get; set; }
        public MainViewModel()
        {
            MenuViewModel = new MenuViewModel();
            CanvasViewModel = new CanvasViewModel();
        }
    }
}
