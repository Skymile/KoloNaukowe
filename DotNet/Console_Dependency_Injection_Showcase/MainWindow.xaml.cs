using System.Windows;

namespace DepInj;

// IoC - Inversion of Control
//  Standardowy przepływ - Użycie bibliotek ogólnego przeznaczenia (pełna kontrola)
//  IoC - Biblioteka używa naszych implementacji bez naszej kontroli 
// Dependency Injection (DI) - automatycznie wstrzykiwanie zależności
//  Odseparowanie konstrukcji od używania obiektów
//public interface IDatabase<T>
//{
//    void Add(T value);
//}
//
//public class SqlDatabase<T> : IDatabase<T>
//{
//    public SqlDatabase(IMainVM vm)
//    {
//
//    }
//
//    public void Add(T value) { ... }
//}
//
//public class ListDatabase<T> : IDatabase<T>
//{
//    public void Add(T value) { ... }
//
//    List<T> collection
//}
//
//public class MockupDatabase<T> : IDatabase<T>
//{
//    public void Add(T value) { }
//}

public class MainVM : IMainVM
{
    public string ViewName { get; set; } = "Abcdef";
}

public interface IMainVM
{
    public string ViewName { get; set; }
}

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();
    public MainWindow(IMainVM vm) : this() => this.DataContext = vm;
}
