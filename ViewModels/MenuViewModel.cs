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

     
        
        
    }
}
