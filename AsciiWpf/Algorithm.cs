using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace AsciiWpf
{
    public static class Algorithm
    {
        private static byte[] GetMask(int w, int h, byte value, int chunkSize)
        {
            int chunk = value / chunkSize;
            byte[] bytes = new byte[w * h];

            if (chunk > 0)
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = 
                        i % chunk == 0
                            ? byte.MinValue 
                            : byte.MaxValue;

            return bytes;
        }

        public static Bitmap ApplyAscii(Bitmap bmp,
                int matrixWidth  = 5,
                int matrixHeight = 5,
                int chunkSize = 32
            )
        {
            if (matrixWidth <= 0 || matrixHeight <= 0)
                return bmp;

            var data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.ReadWrite,
                bmp.PixelFormat
            );

            byte[] bytes = new byte[data.Stride * data.Height];

            const int channels = 3;

            byte[] cells = new byte[matrixHeight * matrixWidth];

            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            for (int k = 0; k < channels; k++)
                for (int x = 0; x < data.Width - matrixWidth - channels; x += matrixWidth)
                    for (int y = 0; y < data.Height - matrixHeight - channels; y += matrixHeight)
                    {
                        int index = x * channels + y * data.Stride;
                
                        int c = 0;
                        for (int i = 0; i < matrixWidth; i++)
                            for (int j = 0; j < matrixHeight; j++)
                            {
                                cells[c++] = bytes[index
                                    + i * channels
                                    + j * data.Stride
                                    + k
                                ];
                            }
                
                        byte mean = (byte)cells.Average(i => i);
                        byte[] output = GetMask(matrixWidth, matrixHeight, mean, chunkSize); 
                
                        c = 0;
                        for (int i = 0; i < matrixWidth; i++)
                            for (int j = 0; j < matrixHeight; j++)
                            {
                                    bytes[index
                                        + i * channels
                                        + j * data.Stride
                                        + k
                                    ] = output[c];
                                ++c;
                            }
                    }
            Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);

            bmp.UnlockBits(data);
            return bmp;
        }
    }
}
