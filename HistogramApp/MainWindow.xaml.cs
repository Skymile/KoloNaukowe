using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

namespace HistogramApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;

			//this.MainImage.Source = new Bitmap("../../../apple_noise.png")
			//	.ToSource();
			//this.EqualizedImage.Source = Algorithm.Equalize(
			//		new Bitmap("../../../apple_noise.png")
			//	)
			//	.ToSource();

			this.HistMainImage.Source = Algorithm.GenerateHistogram(
				Algorithm.GetHistogram(
					new Bitmap("../../../apple_noise.png"),
					HistogramType.Mean
				))
				.ToSource();
			this.HistEqualizedImage.Source = Algorithm.GenerateHistogram(
				Algorithm.GetHistogram(
					Algorithm.Equalize(
						new Bitmap("../../../apple_noise.png")
					),
					HistogramType.Mean
				))
				.ToSource();
			//*/
		}

		public IEnumerable<ISeries> MainSeries { get; set; } = new ObservableCollection<ISeries>()
		{
			new LineSeries<int>
			{
				Values = new ObservableCollection<int>(
					Algorithm.GetHistogram(
						new Bitmap("../../../apple_noise.png"),
						HistogramType.Mean
					)
				),
				Fill = null,
				GeometrySize = 0,
				Stroke = new SolidColorPaint(new SkiaSharp.SKColor(0, 0, 0, 255)),
			}
		};

		public IEnumerable<ISeries> EqualizedSeries { get; set; } = new ObservableCollection<ISeries>()
		{
			new LineSeries<int>
			{
				Values = new ObservableCollection<int>(
					Algorithm.GetHistogram(
						Algorithm.Equalize(new Bitmap("../../../apple_noise.png")),
						HistogramType.Mean
					)
				),
				Fill = null,
				GeometrySize = 0,
				Stroke = new SolidColorPaint(new SkiaSharp.SKColor(0, 0, 0, 255)),
			}
		};

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
