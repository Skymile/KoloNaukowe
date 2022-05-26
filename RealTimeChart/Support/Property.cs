using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RealTimeCharts.Support
{
    public class Property<T> : INotifyPropertyChanged
    {
        public Property(T value = default) =>
            this.field = value;

        public T Value { get => this.field; set => SetValue(ref this.field, value); }

        protected void SetValue(ref T field, T value, [CallerMemberName] string name = "")
        {
            field = value;
            PropertyChanged?.Invoke(this, new(name));
        }

        public static implicit operator string(Property<T> val) => val.field?.ToString();

        public event PropertyChangedEventHandler PropertyChanged;

        private T field;
    }
}
