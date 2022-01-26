using System.Windows;
using System.Windows.Controls;

namespace GeneratorUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var grid = new Grid();
			var btn0 = new Button() { Content = "Abc 0" };
			var btn1 = new Button() { Content = "Abc 1" };

			this.Content = grid;
			// auto - GridLength.Auto
			// *    -   1, GridUnitType.Star
			// 200  - 200, GridUnitType.Pixel
			grid.ColumnDefinitions.Add(
				new ColumnDefinition() { Width = GridLength.Auto });
			grid.ColumnDefinitions.Add(
				new ColumnDefinition() { Width = new GridLength(200, GridUnitType.Pixel) });

			grid.Children.Add(btn0);
			grid.Children.Add(btn1);

			Grid.SetColumn(btn0, 0);
			Grid.SetColumn(btn1, 1);
		}
	}
}
