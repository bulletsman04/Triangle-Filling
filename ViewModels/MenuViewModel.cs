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

        // RelayCommand<T> for commands with parameter
        RelayCommand _closeCommand;
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
        RelayCommand _minimizeCommand;
        public ICommand MinimizeCommand
        {
            get
            {
                if (_minimizeCommand == null)
                {
                    // Add animation (now minimizing is too sudden)
                    _minimizeCommand = new RelayCommand(() => Application.Current.MainWindow.WindowState = WindowState.Minimized,
                        null);
                }
                return _minimizeCommand;
            }
        }

        RelayCommand _maximizeCommand;
        public ICommand MaximizeCommand
        {
            get
            {
                if (_maximizeCommand == null)
                {
                    // Add animation (now minimizing is too sudden)
                    _maximizeCommand = new RelayCommand(() =>
                        {
                            Application.Current.MainWindow.WindowState =
                                Application.Current.MainWindow.WindowState == WindowState.Maximized
                                    ? WindowState.Normal
                                    : WindowState.Maximized;
                        },
                        null);
                }
                return _maximizeCommand;
            }
        }



    }
}
