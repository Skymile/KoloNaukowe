using System.Drawing;
using System.Windows;

namespace K3mThinning
{
	// tinyurl.com/knbiometrii
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.MainImage.Source = Algorithm
				.Apply(new Bitmap(@"C:\Users\mpatr\Desktop\Imgs\cat.png"))
				.ToSource();
		}
	}
}
