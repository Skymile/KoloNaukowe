using System.Windows;

namespace RealTimeCharts
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow() => InitializeComponent();

		private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			if (this.DataContext is MainWindowVM vm)
				vm.CurrentFormula.Value = Chart.Formula.Custom;
		}
	}
}
