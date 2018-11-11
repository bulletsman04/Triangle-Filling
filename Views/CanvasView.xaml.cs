using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Models;
using ViewModels;

namespace Views
{
    /// <summary>
    /// Interaction logic for CanvasView.xaml
    /// </summary>
    public partial class CanvasView : UserControl
    {
        private CanvasViewModel _canvasViewModel;

        public CanvasView()
        {
            InitializeComponent();
            DataContextChanged += this.HandleDataContextChanged;


        }

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Store a reference to the ViewModel.
            _canvasViewModel = base.DataContext as CanvasViewModel;
        }
        
        private void Area_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePoint =  e.GetPosition(sender as Image);
            _canvasViewModel.HandleMouseDown(mousePoint);
            
        }

        private void Area_OnMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = e.GetPosition(sender as Image);
            _canvasViewModel.HandleMouseMove(mousePoint);
        }

        private void Area_OnLoaded(object sender, RoutedEventArgs e)
        {
            _canvasViewModel.WorkingArea = new WorkingArea((int)Area.DesiredSize.Width, (int)Area.DesiredSize.Height,_canvasViewModel.Settings);
            _canvasViewModel.image = Area;
        }

        private void Area_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _canvasViewModel.HandleMouseUp();
        }
    }
}
