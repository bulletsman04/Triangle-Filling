using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using MvvmFoundation.Wpf;

namespace ViewModels
{
    public class MainViewModel
    {
        public MenuViewModel MenuViewModel { get; set; }
        public CanvasViewModel CanvasViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }
        public Settings Settings { get; set; }
        public PropertyObserver<Settings> SettingsObserver { get; set; }
        public PropertyObserver<TriangleSettings> TriangleSettingObserver { get; set; }

        public MainViewModel()
        {
            Settings = new Settings();
            MenuViewModel = new MenuViewModel();
            CanvasViewModel = new CanvasViewModel(Settings);
            SettingsViewModel = new SettingsViewModel(Settings);
            RegisterPropertiesChanged();
        }

        public void RegisterPropertiesChanged()
        {
            SettingsObserver = new PropertyObserver<Settings>(Settings).RegisterHandler(n => n.TriangleSettingsList,ListChangedHandler);
        }

        private void ListChangedHandler(Settings s)
        {
            
            TriangleSettingObserver = new PropertyObserver<TriangleSettings>(s.TriangleSettingsList[0]).RegisterHandler(n => n.PickedColor, PropertyChangedHandler);
        }

        private void PropertyChangedHandler(TriangleSettings ts)
        {
            CanvasViewModel.WorkingArea?.RepaintBitmap();
        }

    }
}
