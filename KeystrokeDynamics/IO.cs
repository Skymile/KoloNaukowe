using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeystrokeDynamics
{
	public static class IO
	{
		public static IEnumerable<IEnumerable<Keystroke>> ParseDirectory(string directory) =>
			Directory.Exists(directory)
				? Directory
					.GetFiles(directory)
					.Select(ParseFile)
				: throw new DirectoryNotFoundException(nameof(directory));

		public static IEnumerable<Keystroke> ParseFile(string file) =>
			File.Exists(file)
				? File
					.ReadLines(file)
					.Select(ParseLine)
				: throw new FileNotFoundException(nameof(file));

		public static Keystroke ParseLine(string line) =>
			ParseArray(line.Split(',').Select(i => i.Trim()).ToArray());

		public static Keystroke ParseArray(string[] values) =>
			values.Length == 3
				? new(values[0], int.Parse(values[1]), int.Parse(values[2]))
				: default;
	}
}