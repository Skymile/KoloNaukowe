
using KeystrokeDynamics;

using NUnit.Framework;

namespace KeystrokeDynamicsTests
{
	public class IOTests
	{
		[Test]
		public void ParseArrayTest()
		{
			string[] array = { "LShift", "0", "700" };
			var expected = new Keystroke("LShift", 0, 700);
			var actual = IO.ParseArray(array);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ParseLineTest()
		{
			string line = "  LShift,    0,  700";
			var expected = new Keystroke("LShift", 0, 700);
			var actual = IO.ParseLine(line);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ParseEmptyLineTest()
		{
			string line = "";
			var expected = new Keystroke();
			var actual = IO.ParseLine(line);

			Assert.AreEqual(expected, actual);
		}
	}
}
