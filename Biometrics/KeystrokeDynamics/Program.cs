using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeystrokeDynamics
{
	class Program
	{
		static void Main(string[] args)
		{
			var keystrokes = new List<Keystroke[]>();
			var sb = new StringBuilder();
			for (int i = 1; i < 15; i++)
			{
				sb.Append("../../../Samples/#");
				sb.Append(i.ToString().PadLeft(2, '0'));
				sb.Append("_1.txt");

				keystrokes.Add(IO.ParseFile(sb.ToString()).ToArray());

				sb.Clear();
			}
			keystrokes.Add(IO.ParseFile("../../../Samples/#01_2.txt").ToArray());
			keystrokes.Add(IO.ParseFile("../../../Samples/#01_3.txt").ToArray());

			double[] avg = keystrokes
				.Select(i => i.Select(i => i.DwellTime).Average()).ToArray();

			var set = new List<SampleSet>();
			for (int i = 0; i < keystrokes.Count; i++)
				set.Add(new SampleSet(i, keystrokes[i]));

			int k = Classifiers.KNN(set[0], set.Skip(1).ToList(), 1, Distances.Euclidean);

			;
		}
	}
}