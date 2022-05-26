using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RealTimeCharts.Chart
{
	public enum Formula
	{
		Sin,
		SinSquare,
		SinTimesHalfSin,
		SinPlusHalfSin,
		Cos,
		Sinc,
		Rectangular,
		Sawtooth,
		Triangular,
		Noise,
		Custom,
	}

	public static class Algorithm
	{
		public unsafe static Bitmap Chart(
				int sampling,
				double frequency,
				double amplitude,
				double offset,
				double noise,
				byte r,
				byte g,
				byte b,
				Formula formulaType,
				string custom
			)
		{
			offset = (offset - 3500) / 20;
			amplitude /= 2000;
			frequency /= 1000;
			noise /= 4000;

			var bmp = new Bitmap(320, 256);
			var data = bmp.LockBits(
				new Rectangle(0, 0, bmp.Width, bmp.Height),
				ImageLockMode.WriteOnly,
				PixelFormat.Format24bppRgb
			);

			int stride = data.Width * 3;
			int size = data.Width * data.Height * 3;
			byte[] arr = new byte[size];

			IntPtr ptr = data.Scan0;
			Marshal.Copy(ptr, arr, 0, size);

			for (int i = 0; i < size; i++)
				arr[i] = 255;

			var random = new Random();

			static double sawTooth(double x) =>
				2 * (x - Math.Floor(0.5 + x));

			Func<double, double> formula = formulaType switch
			{
				Formula.Sin         => x => Math.Sin(x),
				Formula.Custom      => x => Math.Sin(x * 2), // Placeholder
				Formula.SinSquare   => x => Math.Sin(x) * Math.Sin(x),
				Formula.SinTimesHalfSin => x =>
					Math.Sin(x * 2) *
					Math.Sin(x),
				Formula.SinPlusHalfSin => x =>
					Math.Sin(x * 2) +
					Math.Sin(x),
				Formula.Cos         => x => Math.Cos(x),
				Formula.Sinc        => x => Math.Sin(x) / x,
				Formula.Rectangular => x => Math.Sin(x) > 0 ? 1 : -1,
				Formula.Sawtooth    => sawTooth,
				Formula.Triangular  => x => 2 * Math.Abs(sawTooth(x)) - 1,
				Formula.Noise       => x => random.NextDouble() * 2 - 1,
				_ => throw new NotImplementedException(nameof(formulaType))
			};

			for (int x = 1; x <= bmp.Width; x += sampling)
			{
				double func =
					formula(x * Math.PI / 360 * frequency) +
					(random.NextDouble() - 0.5) * noise;

				int h = Math.Clamp(
					(int)((func + 1) / 2 * bmp.Height * amplitude + offset),
					0,
					bmp.Height
				);

				for (int i = 0; i < sampling; i++)
					if (x + i < bmp.Width)
						for (int y = 0; y < h; ++y)
						{
							int o = (x + i - 1) * 3 + (255 - y) * stride;

							arr[o + 0] = b;
							arr[o + 1] = g;
							arr[o + 2] = r;
						}
			}

			Marshal.Copy(arr, 0, ptr, size);
			bmp.UnlockBits(data);
			return bmp;
		}
	}
}
