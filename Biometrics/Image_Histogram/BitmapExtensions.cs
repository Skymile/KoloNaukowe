using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace HistogramApp
{
	public static class BitmapExtensions
	{
		public static BitmapSource ToSource(this Bitmap bmp)
		{
			var data = bmp.LockBits(
				new Rectangle(Point.Empty, bmp.Size),
				ImageLockMode.ReadOnly,
				PixelFormat.Format24bppRgb
			);

			var src = BitmapSource.Create(
				bmp.Width,
				bmp.Height,
				96f, 96f,
				System.Windows.Media.PixelFormats.Bgr24,
				null,
				data.Scan0,
				data.Stride * data.Height,
				data.Width * 3
			);

			bmp.UnlockBits(data);
			return src;
		}
	}
}
