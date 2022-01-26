using System.Drawing;
using System.Windows;

namespace CellularAutomata
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = new MainWindowVM();
		}

		private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			((MainWindowVM)this.DataContext).ClickCmd.Execute(null);
		}
	}
}
