using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;

namespace CellularAutomata
{
	public class Command : ICommand
	{
		public Command(Action action) =>
			this.action = action ?? throw new ArgumentNullException(nameof(action));

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => true;
		public void Execute(object parameter) => this.action();

		private readonly Action action;
	}

	public class MainWindowVM : INotifyPropertyChanged
	{
		private Bitmap bmp;

		public MainWindowVM()
		{
			this.bmp = new Bitmap(256, 256);

			var random = new Random();

			this.bmp.SetPixel(128 + random.Next(-4, 4), 128 + random.Next(-4, 4), System.Drawing.Color.White);
			this.bmp.SetPixel(128 + random.Next(-4, 4), 128 + random.Next(-4, 4), System.Drawing.Color.White);
			this.bmp.SetPixel(128 + random.Next(-4, 4), 128 + random.Next(-4, 4), System.Drawing.Color.White);
			this.bmp.SetPixel(128 + random.Next(-4, 4), 128 + random.Next(-4, 4), System.Drawing.Color.White);
			this.bmp.SetPixel(128 + random.Next(-4, 4), 128 + random.Next(-4, 4), System.Drawing.Color.White);
			this.bmp.SetPixel(128 + random.Next(-4, 4), 128 + random.Next(-4, 4), System.Drawing.Color.White);
			this.bmp.SetPixel(128 + random.Next(-4, 4), 128 + random.Next(-4, 4), System.Drawing.Color.White);

			this.MainSource = Automata.Seed(this.bmp).ToSource();

			this.ClickCmd = new Command(() =>
			{
				this.bmp = Automata.Seed(this.bmp);
				this.MainSource = Automata.Seed(this.bmp).ToSource();
			});

			//Timer timer = new Timer();
			//timer.Interval = 500;
			//timer.Elapsed += Timer_Elapsed;
			//timer.Start();
		}

		//private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		//{
		//	this.ClickCmd.Execute(null);
		//}

		public ICommand ClickCmd { get; set; }

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
}
