using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace K3mThinning
{
	public static class Algorithm
	{
		public static Bitmap Apply(Bitmap bmp)
		{
			BitmapData data = bmp.LockBits(
				new Rectangle(Point.Empty, bmp.Size),
				ImageLockMode.ReadOnly,
				PixelFormat.Format24bppRgb
			);

			var wBmp = new Bitmap(bmp.Width, bmp.Height);
			BitmapData wData = wBmp.LockBits(
				new Rectangle(Point.Empty, wBmp.Size),
				ImageLockMode.WriteOnly,
				PixelFormat.Format24bppRgb
			);

			int y = data.Stride;
			int x = 3;

			int[] offsets =
			{
				  y - x,  y,  y + x,
				    - x,  0,      x,
				- y - x, -y, -y + x
			};

			int size = data.Stride * data.Height;
			byte[] arr = new byte[size];

			Marshal.Copy(data.Scan0, arr, 0, size);

			for (int i = 0; i < size; i += 3)
			{
				arr[i] = arr[i] == Zero ? Zero : One;
				arr[i + 1] = arr[i + 2] = 0;
			}

			var ones = new List<int>();
			for (int i = 0; i < size; i += 3)
				if (arr[i] == One)
					ones.Add(i);

			while (true)
			{
				var borders = new List<int>();

				for (int j = 0; j < ones.Count; j++)
				{
					int sum = 0;
					for (int k = 0; k < offsets.Length; k++)
						sum += (arr[ones[j] + offsets[k]] == One ? 1 : 0) * Matrix[k];

					if (arr[ones[j]] == One &&
						MarkBorders.Contains(sum))
						borders.Add(ones[j]);
				}

				if (borders.Count <= 0)
					break;

				foreach (var lookup in Lookups)
					for (int i = 0; i < borders.Count; i++)
					{
						int sum = 0;
						for (int k = 0; k < offsets.Length; k++)
							sum += (arr[borders[i] + offsets[k]] == One ? 1 : 0) * Matrix[k];

						if (arr[borders[i]] == One &&
							lookup.Contains(sum))
							arr[borders[i]] = Zero;
					}

				for (int j = 0; j < ones.Count; j++)
				{
					int sum = 0;
					for (int k = 0; k < offsets.Length; k++)
						sum += (arr[ones[j] + offsets[k]] == One ? 1 : 0) * Matrix[k];

					if (FinalThinning.Contains(sum))
						arr[ones[j]] = Zero;
				}
			}

			Marshal.Copy(arr, 0, wData.Scan0, size);

			bmp.UnlockBits(data);
			wBmp.UnlockBits(wData);
			return wBmp;
		}

		private static HashSet<int> MakeHashset(params int[] array)
		{
			HashSet<int> set = new HashSet<int>();
			foreach (var i in array)
				set.Add(i);
			return set;
		}

		private const byte Zero = byte.MaxValue;
		private const byte One  = byte.MinValue;

		private static HashSet<int> MarkBorders   = MakeHashset(3, 6, 7, 12, 14, 15, 24, 28, 30, 31, 48, 56, 60, 62, 63, 96, 112, 120, 124, 126, 127, 129, 131, 135, 143, 159, 191, 192, 193, 195, 199, 207, 223, 224, 225, 227, 231, 239, 240, 241, 243, 247, 248, 249, 251, 252, 253, 254);

		private static HashSet<int> A1 = MakeHashset(7, 14, 28, 56, 112, 131, 193, 224                                                                                                                                                                                          );
		private static HashSet<int> A2 = MakeHashset(7, 14, 15, 28, 30, 56, 60, 112, 120, 131, 135, 193, 195, 224, 225, 240                                                                                                                                                     );
		private static HashSet<int> A3 = MakeHashset(7, 14, 15, 28, 30, 31, 56, 60, 62, 112, 120, 124, 131, 135, 143, 193, 195, 199, 224, 225, 227, 240, 241, 248                                                                                                               );
		private static HashSet<int> A4 = MakeHashset(7, 14, 15, 28, 30, 31, 56, 60, 62, 63, 112, 120, 124, 126, 131, 135, 143, 159, 193, 195, 199, 207, 224, 225, 227, 231, 240, 241, 243, 248, 249, 252                                                                        );
		private static HashSet<int> A5 = MakeHashset(7, 14, 15, 28, 30, 31, 56, 60, 62, 63, 112, 120, 124, 126, 131, 135, 143, 159, 191, 193, 195, 199, 207, 224, 225, 227, 231, 239, 240, 241, 243, 248, 249, 251, 252, 254                                                    );

		private static HashSet<int> FinalThinning = MakeHashset(3, 6, 7, 12, 14, 15, 24, 28, 30, 31, 48, 56, 60, 62, 63, 96, 112, 120, 124, 126, 127, 129, 131, 135, 143, 159, 191, 192, 193, 195, 199, 207, 223, 224, 225, 227, 231, 239, 240, 241, 243, 247, 248, 249, 251, 252, 253, 254);

		private static int[] Matrix =
		{
			128,  1, 2,
			 64,  0, 4,
		     32, 16, 8
		};

		private static HashSet<int>[] Lookups =
		{
			A1, A2, A3, A4, A5
		};
	}
}
