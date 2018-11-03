using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using MvvmFoundation.Wpf;

namespace ViewModels
{
    public class SettingsViewModel
    {
        public Settings Settings { get; set; }

        public SettingsViewModel(Settings settings)
        {
            Settings = settings;
        }

        RelayCommand<string> _setTriangleTextureCommand;
        public ICommand SetTriangleTextureCommand
        {
            get
            {
                if (_setTriangleTextureCommand == null)
                {
                    _setTriangleTextureCommand = new RelayCommand<string>(this.SetTriangleTexture,
                        null);
                }
                return _setTriangleTextureCommand;
            }
        }

        public void SetTriangleTexture(string index)
        {
            ;
        }
    }
}
