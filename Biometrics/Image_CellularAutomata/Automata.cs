using System.Drawing;
using System.Runtime.InteropServices;

namespace CellularAutomata
{
	public static class Automata
	{
		public static unsafe Bitmap Seed(Bitmap readBmp)
		{
			int stride = readBmp.Width * 3;
			int GetPixel(int x, int y) =>
				x * 3 + y * stride;

			var writeBmp = new Bitmap(readBmp.Width, readBmp.Height);

			var readData = readBmp.LockBits(
				new Rectangle(Point.Empty, readBmp.Size),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format24bppRgb
			);
			var writeData = writeBmp.LockBits(
				new Rectangle(Point.Empty, readBmp.Size),
				System.Drawing.Imaging.ImageLockMode.WriteOnly,
				System.Drawing.Imaging.PixelFormat.Format24bppRgb
			);

			byte[] r = new byte[stride * readBmp.Height];
			byte[] w = new byte[stride * readBmp.Height];

			Marshal.Copy(readData.Scan0, r, 0, r.Length);

			for (int x = 1; x < readBmp.Width - 1; x++)
				for (int y = 1; y < readBmp.Height - 1; y++)
				{
					int white = 0;
					for (int i = -1; i <= 1; ++i)
						for (int j = -1; j <= 1; ++j)
							if (r[GetPixel(x + i, y + j)] == byte.MaxValue)
								++white;

					int index = GetPixel(x, y);

					// B3/S12345
					if (r[index] != byte.MaxValue)
					{ // Czarny środkowy
						w[index + 0] =
						w[index + 1] =
						w[index + 2] =
							white == 3
								? byte.MaxValue
								: r[index];
					}
					else if (white > 0)
					{ // Biały środkowy
						w[index + 0] =
						w[index + 1] =
						w[index + 2] =
							white <= 5
								? byte.MaxValue
								: r[index];
					}
				}

			Marshal.Copy(w, 0, writeData.Scan0, r.Length);

			readBmp.UnlockBits(readData);
			writeBmp.UnlockBits(writeData);
			return writeBmp;
		}
	}
}
