using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace AsciiWpf
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public ImageSource? ImgSource { get => _imgSource; set => Set(value, ref _imgSource); }

        public byte Chunk        { get => _chunk       ; set => SetAscii(value, ref _chunk       ); }
        public byte MatrixWidth  { get => _matrixWidth ; set => SetAscii(value, ref _matrixWidth ); }
        public byte MatrixHeight { get => _matrixHeight; set => SetAscii(value, ref _matrixHeight); }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        private void SetAscii<T>(T value, ref T field, [CallerMemberName] string name = "")
        {
            Set(value, ref field, name);
            Set(
                _imgSource = Algorithm.ApplyAscii(
                    new System.Drawing.Bitmap("../../../apple.png"),
                    MatrixWidth ,
                    MatrixHeight,
                    Chunk
                ).ToSource(),
                ref _imgSource,
                nameof(ImgSource)
            );
        }

        private void Set<T>(T value, ref T field, [CallerMemberName] string name = "")
        {
            field = value;
            PropertyChanged?.Invoke(this, new(name));
        }
        
        private ImageSource? _imgSource = Algorithm.ApplyAscii(
                new System.Drawing.Bitmap("../../../apple.png")
            ).ToSource();

        private byte _chunk;
        private byte _matrixWidth;
        private byte _matrixHeight;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowVM();
        }
    }
}
