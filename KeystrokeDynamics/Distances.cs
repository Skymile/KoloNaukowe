using System;

namespace KeystrokeDynamics
{
	public static class Distances
	{
		/// (a, b) => c
		/// (3, 4) => 5
		/// Pure
		public static double Euclidean(double a, double b) =>
			Math.Sqrt(a * a + b * b);
	}
}