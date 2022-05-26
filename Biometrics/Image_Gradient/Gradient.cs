using System;
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
			BitmapData data = bmp.LockBits(
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
							int r1 = Math.Clamp(sliderR1 - i, 0, 255);
							int g1 = Math.Clamp(sliderG1 - i, 0, 255);
							int b1 = Math.Clamp(sliderB1 - i, 0, 255);

							int r2 = Math.Clamp(i - 255 + sliderR2, 0, 255);
							int g2 = Math.Clamp(i - 255 + sliderG2, 0, 255);
							int b2 = Math.Clamp(i - 255 + sliderB2, 0, 255);

							int index = GetIndex(i * ratioX + x, j * ratioY + y, stride);

							ptr[index + 0] = (byte)((ptr[index + 0] + b1 + b2) / 2);
							ptr[index + 1] = (byte)((ptr[index + 1] + g1 + g2) / 2);
							ptr[index + 2] = (byte)((ptr[index + 2] + r1 + r2) / 2);
						}

			bmp.UnlockBits(data);
			return bmp;
		}

		private static int GetIndex(int x, int y, int stride) =>
			y * stride + x * 3;
	}
}
