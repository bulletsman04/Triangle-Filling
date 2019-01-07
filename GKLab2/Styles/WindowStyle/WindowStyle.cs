using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace GKLab2.Styles.WindowStyle
{
	internal static class LocalExtensions
	{
		public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
		{
			Window window = ((FrameworkElement)templateFrameworkElement).TemplatedParent as Window;
			if (window != null) action(window);
		}
	}

	public partial class WindowStyle
	{
		void DocButtonClick(object sender, RoutedEventArgs e)
		{
			Process.Start(@"..\..\..\Dokumentacja.txt");
		}

		void CloseButtonClick(object sender, RoutedEventArgs e)
		{
			sender.ForWindowFromTemplate(w => SystemCommands.CloseWindow(w));
		}

		void MinButtonClick(object sender, RoutedEventArgs e)
		{
			sender.ForWindowFromTemplate(w => SystemCommands.MinimizeWindow(w));
		}

	}
}
