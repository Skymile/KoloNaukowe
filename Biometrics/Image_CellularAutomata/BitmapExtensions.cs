using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace CellularAutomata
{
	public static class BitmapExtensions
	{
		public static BitmapSource ToSource(this Bitmap bmp)
		{
			BitmapData data = bmp.LockBits(
				new Rectangle(Point.Empty, bmp.Size),
				ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb
			);

			var bmpSource = BitmapSource.Create(
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
