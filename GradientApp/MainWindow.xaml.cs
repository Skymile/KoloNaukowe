using System.Drawing;
using System.Windows;

namespace GradientApp
{
	//  0 -1  0
	// -1  5 -1
	//  0 -1  0
	// Wyostrzenie (suma = 1)

	// -1 0 0
	//  0 1 0
	//  0 0 0

	// -1 0 1
	// -2 0 2
	// -1 0 1
	// Wykrywanie krawędzi (Sobel) (suma = 0)

	// 1 2 1
	// 2 4 2
	// 1 2 1
	// Gaussian Blur (suma > 1)

	// 1 1 1
	// 1 1 1
	// 1 1 1
	// Box Blur

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow() => InitializeComponent();

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var bmp = new Bitmap("../../../apple_noise.png");

			this.MainImage.Source = Gradient.ToSource(
				Gradient.Apply(bmp,
					(int)this.MainSliderR1.Value,
					(int)this.MainSliderG1.Value,
					(int)this.MainSliderB1.Value,
					(int)this.MainSliderR2.Value,
					(int)this.MainSliderG2.Value,
					(int)this.MainSliderB2.Value
				)
			);
		}
	}
}
