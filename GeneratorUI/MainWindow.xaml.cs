using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GeneratorUI
{
	class TestClass
	{
		public string    TestString { get; set; } = "Abcdefg";
		public int       TestInt    { get; set; } = 8;
		public List<int> TestList   { get; set; } = new List<int>() { 1, 5, 7, 0 };
		public double    TestDouble { get; set; } = 20.4;
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// auto - GridLength.Auto
		// *    -   1, GridUnitType.Star
		// 200  - 200, GridUnitType.Pixel
		public MainWindow()
		{
			InitializeComponent();

			var scrollViewer = new ScrollViewer();
			this.Content = scrollViewer;

			var grid = new Grid();
			scrollViewer.Content = grid;

			var props = obj.GetType().GetProperties();

			for (int i = 0; i < props.Length; i++)
			{
				grid.RowDefinitions.Add(
					new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) }
				);
				var label  = new Label();
				var txtBox = new TextBox();
				txtBox.FontFamily = new System.Windows.Media.FontFamily("Consolas");
				txtBox.FontSize = 24;

				if (props[i].PropertyType != typeof(string) &&
					props[i].PropertyType.GetInterface(nameof(IEnumerable)) is not null)
				{
					if (props[i].GetValue(obj) is IEnumerable enu)
					{
						label.Content = props[i].Name;
						txtBox.Text = string.Join(" ", enu.Cast<object>());

						int t = i;
						list.Add(() =>
						{
							var eleType = props[t].PropertyType.GenericTypeArguments.First();
							var lst = txtBox.Text.Split()
								.Select(k => Convert.ChangeType(k, eleType))
								.ToList();
							var list = new List<int>();
							foreach (var k in lst)
								list.Add((int)k);
							props[t].SetValue(
								obj,
								list
								//Convert.ChangeType(lst, props[t].PropertyType)
							);
						});
					}
				}
				else
				{
					label.Content = props[i].Name;
					txtBox.Text = props[i].GetValue(obj).ToString();
					int t = i;
					list.Add(() =>
					{
						props[t].SetValue(
							obj,
							Convert.ChangeType(txtBox.Text, props[t].PropertyType)
						);
					});
				}

				var stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
				stackPanel.Children.Add(label);
				stackPanel.Children.Add(txtBox);
				grid.Children.Add(stackPanel);
				Grid.SetRow(stackPanel, i);
			}

			grid.RowDefinitions.Add(
				new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) }
			);
			var btnSave = new Button();
			btnSave.Content = "Save Object";
			btnSave.Click += BtnSave_Click;
			grid.Children.Add(btnSave);
			Grid.SetRow(btnSave, grid.RowDefinitions.Count);
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			foreach (var i in list)
				i();
			;
		}

		private TestClass obj = new TestClass();
		private List<Action> list = new();
	}
}
