using System;

namespace KeystrokeDynamics
{
	public struct Keystroke
	{
		public Keystroke(string key, int pressedTime, int releasedTime)
		{
			this.Key = key ?? throw new ArgumentNullException(nameof(key));
			this.PressedTime = pressedTime;
			this.ReleasedTime = releasedTime;
		}

		public readonly string Key;
		public readonly int    PressedTime;
		public readonly int    ReleasedTime;
	}
}