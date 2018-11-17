using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Models;
using MvvmFoundation.Wpf;
using Point3D = Models.Point3D;

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
        public PropertyObserver<TriangleSettings> TriangleSettingObserver2 { get; set; }
        private static System.Timers.Timer _timer;
        private int _xInc=3;
        private bool _mutex = false;
        

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
            SettingsObserver = new PropertyObserver<Settings>(Settings)
                .RegisterHandler(n => n.TriangleSettingsList, ListChangedHandler)
                .RegisterHandler(n => n.LightPoint, PropertyChangedHandler)
                .RegisterHandler(n => n.LightColor, PropertyChangedHandler)
                .RegisterHandler(n => n.IsLightConst, PropertyChangedHandler)
                .RegisterHandler(n => n.IsLightMouse, PropertyChangedHandler)
                .RegisterHandler(n => n.IsLightAnimation, PropertyChangedHandler)
                .RegisterHandler(n => n.IsLightAnimation, StartLightAnimation)
                .RegisterHandler(n => n.HeightMap, NMapChangedHandler)
                .RegisterHandler(n => n.NormalMap, NMapChangedHandler)
                .RegisterHandler(n => n.IsNormalConst, NMapChangedHandler)
                .RegisterHandler(n => n.IsHeightConst, NMapChangedHandler)
                .RegisterHandler(n => n.IsPhong, PropertyChangedHandler)
                .RegisterHandler(n => n.LambertRate, PropertyChangedHandler)
                .RegisterHandler(n => n.PhongRate, PropertyChangedHandler)
                .RegisterHandler(n => n.MPhong, PropertyChangedHandler)
                .RegisterHandler(n => n.HeightRate, NMapChangedHandler);
        }

        private void ListChangedHandler(Settings s)
        {

            TriangleSettingObserver = new PropertyObserver<TriangleSettings>(s.TriangleSettingsList[0])
                .RegisterHandler(n => n.PickedColor, PropertyChangedHandler)
                .RegisterHandler(n => n.PickedTriangleTexture, PropertyChangedHandler)
                .RegisterHandler(n => n.IsColor, PropertyChangedHandler);

            TriangleSettingObserver2 = new PropertyObserver<TriangleSettings>(s.TriangleSettingsList[1])
                    .RegisterHandler(n => n.PickedColor, PropertyChangedHandler)
                    .RegisterHandler(n => n.PickedTriangleTexture, PropertyChangedHandler)
                    .RegisterHandler(n => n.IsColor, PropertyChangedHandler);
        }

        private void PropertyChangedHandler(TriangleSettings ts)
        {
            CanvasViewModel.WorkingArea?.RepaintBitmap();
        }
        private void PropertyChangedHandler(Settings ts)
        {
            CanvasViewModel.WorkingArea?.RepaintBitmap();
        }
        private void NMapChangedHandler(Settings ts)
        {
            Settings.CalculateNMap();
            CanvasViewModel.WorkingArea?.RepaintBitmap();
        }

        private void StartLightAnimation(Settings s)
        {
            if (Settings.IsLightAnimation)
            {
               if(_timer == null)
                   SetTimer();
               else
               {
                   _timer.Start();
               }
                Settings.LightPoint = new Point3D(0,-Settings.Width/2,Settings.LightAnimRadius);
                
            }
            else
            {
                _timer.Stop();
            }
           
        }

        private  void SetTimer()
        {
            _timer = new System.Timers.Timer(10);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            
            if (Settings.LightPoint.X >= Settings.Width - _xInc - 1 && _xInc > 0)
                _xInc *= -1;
            else if (Settings.LightPoint.X < _xInc && _xInc < 0 )
                _xInc *= -1;

            Settings.LightPoint.X += _xInc;
            if (_mutex == false)
            {
                Application.Current.Dispatcher.Invoke(() =>
            {
                    _mutex = true;
                    CanvasViewModel.WorkingArea.RepaintBitmap();
                    _mutex = false;
                
            });
            }


        }
    }
}
