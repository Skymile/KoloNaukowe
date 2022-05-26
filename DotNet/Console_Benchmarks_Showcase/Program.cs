using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

var bmk = new BenchmarkTest();
var arr1 = bmk.TestInternalA(bmk.Data);
var arr2 = bmk.TestInternalB(bmk.Data);

Console.WriteLine(string.Join('\n', arr1.Chunk(3).Select(i => string.Join('\t', i))));
Console.WriteLine();
Console.WriteLine(string.Join('\n', arr2.Chunk(3).Select(i => string.Join('\t', i))));

//BenchmarkRunner.Run<BenchmarkTest>();

public class Data
{
    public int MatrixWidth  { get; set; }
    public int MatrixHeight { get; set; }
    public int Channels     { get; set; }
    public int Width        { get; set; }
}

//[SimpleJob(RunStrategy)]
public class BenchmarkTest
{
    [GlobalSetup]
    public void Setup() { }

    [Benchmark]
    public void TestA() => TestInternalA(Data);
    [Benchmark]
    public void TestB() => TestInternalB(Data);

    public int[] TestInternalA(Data data)
    {
        var offsets = new int[data.MatrixWidth * data.MatrixHeight];
        for (int x = 0; x < data.MatrixWidth; x++)
        {
            int xOffset = (x - 1) * data.Channels;

            for (int y = 0; y < data.MatrixHeight; y++)
            {
                int yOffset = (y - 1) * data.Width;

                offsets[x + y * data.MatrixWidth] = xOffset + yOffset;
            }
        }
        return offsets;
    }

    public int[] TestInternalB(Data data)
    {
        var offsets = new int[data.MatrixWidth * data.MatrixHeight];
        int val = -data.Channels * 2 - data.Width;
        int resetOffset = data.Width - data.Channels * 3;

        int height = data.MatrixHeight;
        int width = data.MatrixWidth;
        int channels = data.Channels;

        if (height == 3 && width == 3)
        {
            offsets[0] = val += channels;
            offsets[1] = val += channels;
            offsets[2] = val += channels;
            val += resetOffset;
            offsets[3] = val += channels;
            offsets[4] = val += channels;
            offsets[5] = val += channels;
            val += resetOffset;
            offsets[6] = val += channels;
            offsets[7] = val += channels;
            offsets[8] = val += channels;
        }
        else
        {
            int o = -1;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    offsets[++o] = val += channels;
                val += resetOffset;
            }
        }

        return offsets;
    }

    public readonly Data Data = new() { MatrixWidth = 3, MatrixHeight = 3, Width = 100, Channels = 3 };
}

