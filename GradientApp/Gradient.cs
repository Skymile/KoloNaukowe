using System.Drawing;
using System.Drawing.Imaging;

namespace GradientApp
{
	public class Gradient
	{
		public unsafe static Bitmap Apply(
				Bitmap bmp,
				int sliderR1,
				int sliderG1,
				int sliderB1,
				int sliderR2,
				int sliderG2,
				int sliderB2
			)
		{
			var data = bmp.LockBits(
				new Rectangle(Point.Empty, bmp.Size),
				ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb
			);

			byte* ptr = (byte*)data.Scan0.ToPointer();
			int stride = data.Stride;

			int ratioX = bmp.Width / 256;
			int ratioY = bmp.Height / 256;

			for (int i = 0; i < 256; i++)
				for (int j = 0; j < 256; j++)
					for (int x = 0; x < ratioX; x++)
						for (int y = 0; y < ratioY; y++)
						{
							int r1 = sliderR1 - i;
							int g1 = sliderG1 - i;
							int b1 = sliderB1 - i;

							int r2 = i - 255 + sliderR2;
							int g2 = i - 255 + sliderG2;
							int b2 = i - 255 + sliderB2;

							if (r1 > 255) r1 = 255;
							if (g1 > 255) g1 = 255;
							if (b1 > 255) b1 = 255;
							if (r2 > 255) r2 = 255;
							if (g2 > 255) g2 = 255;
							if (b2 > 255) b2 = 255;

							if (r1 < 0) r1 = 0;
							if (g1 < 0) g1 = 0;
							if (b1 < 0) b1 = 0;
							if (r2 < 0) r2 = 0;
							if (g2 < 0) g2 = 0;
							if (b2 < 0) b2 = 0;

							int cIndex = GetIndex(i * ratioX + x, j * ratioY + y, stride);
							var color = Color.FromArgb(
								ptr[cIndex + 0],
								ptr[cIndex + 1],
								ptr[cIndex + 2]
							);

							ptr[cIndex + 0] = (byte)((color.R + r1 + r2) / 2);
							ptr[cIndex + 1] = (byte)((color.G + g1 + g2) / 2);
							ptr[cIndex + 2] = (byte)((color.B + b1 + b2) / 2);

							//var color = bmp.GetPixel(
							//	i * ratioX + x,
							//	j * ratioY + y
							//);

							//bmp.SetPixel(
							//	i * ratioX + x,
							//	j * ratioY + y,
							//	Color.FromArgb(
							//		(color.R + r1 + r2) / 2,
							//		(color.G + g1 + g2) / 2,
							//		(color.B + b1 + b2) / 2
							//	)
							//);
						}

			bmp.UnlockBits(data);
			return bmp;
		}

		// 0 1 2 3  4  5
		// 6 7 8 9 10 11
		//
		// RGB
		// Stride = 6
		// Szerokość = 2
		// Wysokość = 2
		/// 1 * 3 + 0 * 6

		private static int GetIndex(int x, int y, int stride) =>
			y * stride + x * 3;

		public static System.Windows.Media.Imaging.BitmapSource ToSource(Bitmap bmp)
		{
			var data = bmp.LockBits(
				new Rectangle(Point.Empty, bmp.Size),
				ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb
			);

			var bmpSource = System.Windows.Media.Imaging.BitmapSource.Create(
				data.Width,
				data.Height,
				96, 96,
				System.Windows.Media.PixelFormats.Bgr24,
				null,
				data.Scan0,
				data.Stride * data.Height,
				data.Stride
			);

			bmp.UnlockBits(data);
			return bmpSource;
		}
	}
}
