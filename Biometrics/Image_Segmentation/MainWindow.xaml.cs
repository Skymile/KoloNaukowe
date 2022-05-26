using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace Segmentation
{
    // Segmentacja
    /// Grupowanie pikseli na bazie spełniania kryteriów
    /// Grupy pikseli stykające się lokalnie 
    /// Rozdzielenie obrazu na jakieś logiczne podczęści tło/obiekt
    // Ekstrakcja
    /// Crossing Number => Na przykład, czy dany czarny piksel ma jednego czarnego sąsiada. 
    /// (Zakończenie linii, bo ma tylko 1 sąsiada)
    // Szkieletyzacja
    /// K3M, KMM, Zhang-Suen, Zhang-Wang
    /// Zamiana zbinaryzowanego obrazu na równoważny topologicznie szkielet (linie o grubości 1 piksela)
    // Binaryzacja
    /// Zamiana obrazu na obraz 2 wartości (czarne/białe, 0/1)
    /// Progowanie lokalne 
    ///     Niblack, Sauvola, Phansalkar, Bersnen
    /// LuWu, Kapur => na bazie entropii (różnią się +- i minimalna wartość brana a w drugim maksymalna)
    /// Progowanie globalne => jeden próg na cały obraz 
    ///     Otsu
    // Filtry nieliniowe
    /// Median
    /// Median Of Medians => Mediana median 
    ///   3x3 => 3 wektory po 3 piksele, mediana z każdego z wektorów,
    ///          3 mediany => mediana z 3 wartości
    // Filtry liniowe
    /// Rozmycie Gaussa
    ///   1 2 1
    ///   2 4 2
    ///   1 2 1
    /// 
    /// Rozmycie polowe (Box Blur)
    ///   1 1 1
    ///   1 1 1
    ///   1 1 1
    ///   
    /// Suma > 1
    ///   
    /// Laplaciany
    ///   1  1  1
    ///   1 -8  1
    ///   1  1  1
    ///   
    ///   0  1  0
    ///   1 -4  1
    ///   0  1  0
    ///   
    /// 
    /// Filtr Sobela
    ///    1  2  1
    ///    0  0  0
    ///   -1 -2 -1
    ///   
    /// Suma = 0
    ///
    /// Filtr wyostrzający
    ///   1   2  1
    ///   2  -9  2
    ///   1   2  1
    ///   
    /// Suma = 1

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainImg.Source = new Bitmap("../../../apple.png")
                .ToSource();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainImg.Source = Algorithm
                .Apply(new Bitmap("../../../apple.png"), (int)e.NewValue)
                .ToSource();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            MainImg.Source = new Bitmap("../../../apple.png")
                .ToSource();
        }
    }
}
