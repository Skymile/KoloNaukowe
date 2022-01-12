using System;

using KeystrokeDynamics;

using NUnit.Framework;

namespace KeystrokeDynamicsTests
{
	public class Tests
	{
		[Test]
		public void EuclideanTest()
		{
			Assert.AreEqual(5, Distances.Euclidean(3, 4));

			for (int i = -20; i < 20; i++)
			{
				double result   = Math.Round(Distances.Euclidean(i, i) , 8);
				double expected = Math.Round(Math.Abs(i) * Math.Sqrt(2), 8);

				Assert.AreEqual(expected, result);
			}
		}

		[Test]
		public void EuclideanNotEqualTest() =>
			Assert.AreNotEqual(1, Distances.Euclidean(1, 1));
	}
}