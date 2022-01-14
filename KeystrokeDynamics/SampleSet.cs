using System;
using System.Linq;

namespace KeystrokeDynamics
{
	public class SampleSet
	{
		public SampleSet(int id, Keystroke[] keystrokes)
		{
			this.Id = id;
			this.Keystrokes = keystrokes ?? throw new ArgumentNullException(nameof(keystrokes));
		}

		public int Id { get; set; }
		public Keystroke[] Keystrokes { get; set; }

		public double[] Dwells =>
			this.Keystrokes.Select(i => (double)i.DwellTime).ToArray();
	}
}
