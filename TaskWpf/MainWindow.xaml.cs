using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TaskWpf
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
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SetText("Start");

            await GetTest();
            await GetTest();

            SetText("Finish");
        }

        private Task<int> GetTest2() =>
            cached.HasValue
                ? Task.FromResult(4)
                : Task.Run(() => 4);

        private async ValueTask<int?> GetTest() =>
            cached.HasValue ? cached : (cached = LongOperation());
        private int? cached;

        private int LongOperation()
        {
            Thread.Sleep(1000);
            return Random.Shared.Next(0, 20);
        }

        public void SetText(string value)
        {
            LabelText = value;
            PropertyChanged?.Invoke(this, new(nameof(LabelText)));
        }

        public string LabelText { get; set; }
    }
}
