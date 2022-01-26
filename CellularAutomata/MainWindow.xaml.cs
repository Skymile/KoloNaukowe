using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace CellularAutomata
{
	public class MainWindowVM : INotifyPropertyChanged
	{
		public ImageSource MainSource
		{
			get => this.mainSource;
			set
			{
				this.mainSource = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MainSource)));
			}
		}

		private ImageSource mainSource;

		public event PropertyChangedEventHandler PropertyChanged;
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var vm = new MainWindowVM();
			this.DataContext = vm;
			var bmp = new Bitmap(256, 256);
			bmp.SetPixel(128, 128, System.Drawing.Color.White);
			bmp.SetPixel(129, 129, System.Drawing.Color.White);
			bmp.SetPixel(128, 129, System.Drawing.Color.White);
			vm.MainSource = bmp.ToSource();
		}
	}
}
