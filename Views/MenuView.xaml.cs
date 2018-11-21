using System;
using System.Collections.Generic;
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
using ViewModels;

namespace Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            DataContextChanged += this.HandleDataContextChanged;


        }

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Store a reference to the ViewModel.
            _menuViewModel = base.DataContext as MenuViewModel;
        }

        private void MenuView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
               
                Application.Current.MainWindow.DragMove();
            }

        }

        private MenuViewModel _menuViewModel;


        private void Documentation_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _menuViewModel.DocumentationCommand.Execute(null);
        }
    }
}
