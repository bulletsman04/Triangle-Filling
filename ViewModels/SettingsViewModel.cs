using System;
using System.Collections.Generic;
using System.Drawing;
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
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            
            dlg.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            dlg.DefaultExt = ".png";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Settings.TriangleSettingsList[int.Parse(index)].PickedTriangleTexture = new Bitmap(filename);
            }
        }
    }
}
