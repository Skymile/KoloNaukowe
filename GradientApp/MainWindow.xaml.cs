using System.Drawing;
using System.Windows;

namespace GradientApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow() => InitializeComponent();

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var bmp = new Bitmap("../../../apple_noise.png");

			this.MainImage.Source = Gradient.Apply(bmp,
				(int)(this.MainSliderR1?.Value ?? 0),
				(int)(this.MainSliderG1?.Value ?? 0),
				(int)(this.MainSliderB1?.Value ?? 0),
				(int)(this.MainSliderR2?.Value ?? 0),
				(int)(this.MainSliderG2?.Value ?? 0),
				(int)(this.MainSliderB2?.Value ?? 0)
			).ToSource();
		}
	}
}
