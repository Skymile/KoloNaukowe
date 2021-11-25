using System;
using System.Windows.Input;

namespace RealTimeCharts.Support
{
	public class Command : ICommand
	{
		public Command(Action action) => this.action = action;

		public void Execute(object parameter) => this.action();

		public bool CanExecute(object parameter) => true;

		public event EventHandler CanExecuteChanged;

		private readonly Action action;
	}
}
