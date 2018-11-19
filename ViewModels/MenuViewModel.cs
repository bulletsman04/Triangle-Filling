using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmFoundation.Wpf;

namespace ViewModels
{
    public class MenuViewModel
    {

        public MenuViewModel()
        {
        }

        RelayCommand _closeCommand;
        RelayCommand _minimizeCommand;

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(this.Close,
                        null);
                }
                return _closeCommand;
            }
        }

        public void Close()
        {
            Application.Current.MainWindow.Close();
        }
        public ICommand MinimizeCommand
        {
            get
            {
                if (_minimizeCommand == null)
                {
                    _minimizeCommand = new RelayCommand(() => Application.Current.MainWindow.WindowState = WindowState.Minimized,
                        null);
                }
                return _minimizeCommand;
            }
        }

    }
}
