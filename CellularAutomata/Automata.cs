using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
	public static class Automata
	{
		public static Bitmap Seed(Bitmap bmp)
		{
			int stride = bmp.Width * 3;

			int GetPixel(int x, int y) =>
				x * 3 + y * stride;

			return bmp;
		}
	}
}
