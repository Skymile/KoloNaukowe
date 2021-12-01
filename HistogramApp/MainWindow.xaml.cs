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
			this.HistMainImage.Source = Algorithm.GenerateHistogram(
				Algorithm.GetHistogram(
					new Bitmap("../../../apple_noise.png"),
					HistogramType.Mean
				))
				.ToSource();

			this.EqualizedImage.Source = Algorithm.Equalize(
					new Bitmap("../../../apple_noise.png")
				)
				.ToSource();

			this.HistEqualizedImage.Source = Algorithm.GenerateHistogram(
				Algorithm.GetHistogram(
					Algorithm.Equalize(
						new Bitmap("../../../apple_noise.png")
					),
					HistogramType.Mean
				))
				.ToSource();

		}
	}
}
