using System;
using System.Collections.Generic;
using System.Linq;

namespace KeystrokeDynamics
{
	public delegate double Distance(IEnumerable<double> a, IEnumerable<double> b);

	public static class Distances
	{
		public static double Euclidean(IEnumerable<double> a, IEnumerable<double> b) =>
			Math.Sqrt(a.Zip(b, (i, j) => i - j).Sum(i => i * i));
		public static double Manhattan(IEnumerable<double> a, IEnumerable<double> b) =>
			a.Zip(b, (i, j) => Math.Abs(i - j)).Sum();
		public static double Chebyshev(IEnumerable<double> a, IEnumerable<double> b) =>
			Math.Sqrt(a.Zip(b, (i, j) => i - j).Max());
		// Zip dla Manhattan
		//  a 1  2 3 4
		//  b 8  9 0 3
		// => 9 11 3 7

		/// (a, b) => c
		/// (3, 4) => 5
		/// Pure
		public static double Euclidean(double a, double b) =>
			Math.Sqrt(a * a + b * b);

		public static double Manhattan(double a, double b) =>
			a + b;

		public static double Chebyshev(double a, double b) =>
			Math.Max(a, b);
	}
}
// Manhattan
//   0123456
// 0 0123
// 1    4
// 2    567
/// x + y
// Chebyshev
//   0123456
// 0 0
// 1  1
// 2   2345
///	Max(x, y)