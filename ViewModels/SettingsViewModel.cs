﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using MvvmFoundation.Wpf;
using PixelMapSharp;

namespace ViewModels
{
    public class SettingsViewModel
    {
        public Settings Settings { get; set; }


        public SettingsViewModel(Settings settings)
        {
            Settings = settings;
        }

        RelayCommand<string> _setTextureCommand;
        public ICommand SetTextureCommand
        {
            get
            {
                if (_setTextureCommand == null)
                {
                    _setTextureCommand = new RelayCommand<string>(this.SetTexture,
                        null);
                }
                return _setTextureCommand;
            }
        }

        public void SetTexture(string index)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"; ;

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                int i = int.Parse(index);
                switch (i)
                {
                    case 0:
                        Settings.TriangleSettingsList[i].PickedTriangleTexture =
                        LibrariesConverters.BitmapToVectors(new Bitmap(filename));
                        break;
                    case 1:
                        Settings.TriangleSettingsList[i].PickedTriangleTexture =
                            LibrariesConverters.BitmapToVectors(new Bitmap(filename));
                        break;
                    case 2:
                        Settings.NormalMap =
                            PixelMap.SlowLoad(new Bitmap(filename));
                        break;
                    case 3:
                        Settings.HeightMap =
                            PixelMap.SlowLoad(new Bitmap(filename));
                        break;
                }
                
            }
        }
    }
}
