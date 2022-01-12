using System.Linq;

namespace KeystrokeDynamics
{
	public static class IO
	{
		public static Keystroke ParseLine(string line) =>
			ParseArray(line.Split(',').Select(i => i.Trim()).ToArray());

		public static Keystroke ParseArray(string[] values) =>
			values.Length == 3
				? new(values[0], int.Parse(values[1]), int.Parse(values[2]))
				: default;
	}
}