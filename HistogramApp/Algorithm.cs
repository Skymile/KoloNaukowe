using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace HistogramApp
{
	public enum HistogramType
	{
		R, G, B, Mean
	}

	public static class Algorithm
	{
		public unsafe static Bitmap Equalize(Bitmap bmp)
		{
			int[] lookup = GetEqualization(GetHistogram(bmp, HistogramType.Mean));

			var data = bmp.LockBits(
				new Rectangle(Point.Empty, bmp.Size),
				ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb
			);

			int size = data.Stride * data.Height;
			int stride = data.Stride;
			byte* ptr = (byte*)data.Scan0.ToPointer();

			for (int i = 0; i < size; i++)
				ptr[i] = (byte)lookup[ptr[i]];

			bmp.UnlockBits(data);
			return bmp;
		}

		public static int[] GetEqualization(int[] histogram)
		{
			histogram[0] = 0;

			double sum = histogram.Sum();
			double[] distr = new double[256];
			for (int i = 0; i < histogram.Length; i++)
				distr[i] = histogram[i] / sum;
			for (int i = 1; i < histogram.Length; i++)
				distr[i] += distr[i - 1];

			int[] lookup = new int[256];
			for (int i = 0; i < histogram.Length; i++)
				lookup[i] = (int)((distr[i] - distr[0]) / (1 - distr[0]) * 255);

			return lookup;
		}

		public static Bitmap GenerateEqualization(int[] histogram) =>
			GenerateHistogram(GetEqualization(histogram));

		public unsafe static Bitmap GenerateHistogram(int[] histogram)
		{
			var bmp = new Bitmap(256, 256);
			var data = bmp.LockBits(
				new Rectangle(Point.Empty, bmp.Size),
				ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb
			);

			double max = histogram.Max();
			for (int i = 0; i < histogram.Length; i++)
				histogram[i] = (int)(histogram[i] / max * 255);

			int size = data.Stride * data.Height;
			int stride = data.Stride;
			byte* ptr = (byte*)data.Scan0.ToPointer();

			for (int i = 0; i < size; ++i)
				ptr[i] = 255;

			for (int x = 0; x < 256; x++)
			{
				int v = histogram[x];

				for (int y = 0; y < v; y++)
				{
					int i = x * 3 + (255 - y) * stride;

					ptr[i] = ptr[i + 1] = ptr[i + 2] = 0;
				}
			}

			bmp.UnlockBits(data);
			return bmp;
		}

		public static unsafe int[] GetHistogram(Bitmap bmp, HistogramType histType)
		{
			var data = bmp.LockBits(
				new Rectangle(Point.Empty, bmp.Size),
				ImageLockMode.ReadOnly,
				PixelFormat.Format24bppRgb
			);

			int size = data.Stride * data.Height;
			byte* ptr = (byte*)data.Scan0.ToPointer();

			int[] histogram = new int[256];

			Func<byte, byte, byte, byte> func = histType switch
			{
				HistogramType.R => (r, g, b) => r,
				HistogramType.G => (r, g, b) => g,
				HistogramType.B => (r, g, b) => b,
				HistogramType.Mean => (r, g, b) => (byte)((r + g + b) / 3),
				_ => throw new NotImplementedException(nameof(histType))
			};

			//switch (histType)
			//{
			//	case HistogramType.R:
			//		func = (r, g, b) => r;
			//		break;
			//	case HistogramType.G:
			//		func = (r, g, b) => g;
			//		break;
			//	case HistogramType.B:
			//		func = (r, g, b) => b;
			//		break;
			//	case HistogramType.Mean:
			//		func = (r, g, b) => (byte)((r + g + b) / 3);
			//		break;
			//	default: throw new NotImplementedException(nameof(histType));
			//}

			for (int i = 0; i < size; i += 3)
			{
				byte v = func(ptr[i + 2], ptr[i + 1], ptr[i]);

				++histogram[v];
			}

			bmp.UnlockBits(data);
			return histogram;
		}
	}
}
