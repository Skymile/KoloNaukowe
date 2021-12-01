using System.Drawing;
using System.Windows;

namespace HistogramApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			this.MainImage.Source = new Bitmap("../../../apple_noise.png")
				.ToSource();
		}
	}
}
