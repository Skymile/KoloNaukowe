using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using RealTimeCharts.Chart;
using RealTimeCharts.Support;

namespace RealTimeCharts
{
    public class MainWindowVM
	{
		public MainWindowVM()
		{
			this.MainSource = new(default);

			this.SliderSampling.PropertyChanged += SliderValue_PropertyChanged;
			this.SliderFreq    .PropertyChanged += SliderValue_PropertyChanged;
			this.SliderAmpl    .PropertyChanged += SliderValue_PropertyChanged;
			this.SliderOffset  .PropertyChanged += SliderValue_PropertyChanged;
			this.SliderNoise   .PropertyChanged += SliderValue_PropertyChanged;

			this.TextFormula.PropertyChanged += TextFormula_PropertyChanged;

			this.SliderR.PropertyChanged += SliderValue_PropertyChanged;
			this.SliderG.PropertyChanged += SliderValue_PropertyChanged;
			this.SliderB.PropertyChanged += SliderValue_PropertyChanged;

			this.CurrentFormula.PropertyChanged += SliderValue_PropertyChanged;

			SliderValue_PropertyChanged(this, null);
		}

		private void TextFormula_PropertyChanged(object sender, PropertyChangedEventArgs e) =>
			this.CurrentFormula.Value = Formula.Custom;

		private void SliderValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.RectangleColor.Value = new SolidColorBrush(Color.FromRgb(
				(byte)this.SliderR.Value,
				(byte)this.SliderG.Value,
				(byte)this.SliderB.Value
			));

			this.AxisX1.Value = $"{this.SliderFreq.Value / 2250 * 2 / 4:N2}π";
			this.AxisX2.Value = $"{this.SliderFreq.Value / 2250 * 4 / 4:N2}π";
			this.AxisX3.Value = $"{this.SliderFreq.Value / 2250 * 6 / 4:N2}π";
			this.AxisX4.Value = $"{this.SliderFreq.Value / 2250 * 8 / 4:N2}π";

			this.SliderFreqNormalized.Value = $"{this.SliderFreq.Value / 2250 * 2:N2}π";
			this.SliderAmplNormalized.Value = $"{this.SliderAmpl.Value / 1980:N2}";
			this.SliderOffsetNormalized.Value = $"{(this.SliderOffset.Value / 3540 - 1) * 1.47:N2}";

			this.MainSource.Value = Algorithm.Chart(
				(int)this.SliderSampling.Value,
				this.SliderFreq  .Value,
				this.SliderAmpl  .Value,
				this.SliderOffset.Value,
				this.SliderNoise .Value,

				(byte)this.SliderR.Value,
				(byte)this.SliderG.Value,
				(byte)this.SliderB.Value,

				this.CurrentFormula.Value,
				this.TextFormula.Value
			).ToBitmapSource();
		}

		public Property<Formula> CurrentFormula { get; } = new Property<Formula>(Formula.Sin);
		public List<Formula> AllFormulaOptions { get; } =
			Enum.GetValues<Formula>().ToList();

		public Property<Brush> RectangleColor { get; } = new(Brushes.Cyan);

		public Property<string> AxisX1 { get; } = new("");
		public Property<string> AxisX2 { get; } = new("");
		public Property<string> AxisX3 { get; } = new("");
		public Property<string> AxisX4 { get; } = new("");

		public Property<double> SliderSampling { get; } = new(1);

		public Property<double> SliderFreq           { get; } = new(1500);
		public Property<string> SliderFreqNormalized { get; } = new("");

		public Property<double> SliderAmpl           { get; } = new(1000);
		public Property<string> SliderAmplNormalized { get; } = new("");

		public Property<double> SliderOffset           { get; } = new(5000);
		public Property<string> SliderOffsetNormalized { get; } = new("");

		public Property<double> SliderNoise    { get; } = new(0);

		public Property<double> SliderR    { get; } = new(0);
		public Property<double> SliderG    { get; } = new(128);
		public Property<double> SliderB    { get; } = new(255);

		public Property<string> TextFormula { get; } = new();

		public Property<BitmapSource> MainSource { get; }

		public ICommand ButtonClick { get; }
	}
}
