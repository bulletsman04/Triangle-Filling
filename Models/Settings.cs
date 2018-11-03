using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmFoundation.Wpf;

namespace Models
{
    public class Settings: ObservableObject
    {
        
        public ObservableCollection<TriangleSettings> TriangleSettingsList { get; set; }

        public Settings()
        {
            TriangleSettingsList = new ObservableCollection<TriangleSettings>();
        }

        public void RaiseEventListAdd()
        {
          RaisePropertyChanged("TriangleSettingsList");
        }
    }
}
