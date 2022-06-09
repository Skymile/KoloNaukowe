using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests_TicTacToe.Tests;

// More: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices

[TestClass()]
public class ProgramTests
{
    [TestMethod()]
    public void GridToStringTest()
    {
        Assert.IsTrue(TicTacToe.GridToString(new bool?[] { null, null, null }).CompareTo(" 1  2  3 \n") == 0, "First");

        Assert.IsTrue(TicTacToe.GridToString(new bool?[] { true, null, false }).CompareTo(" x  2  o \n") == 0, "Second");

        Assert.IsTrue(TicTacToe.GridToString(new bool?[] { null, false, null }).CompareTo(" 1  o  3 \n") == 0, "Third");
    }

    [TestMethod()]
    public void GridItemToCharTest() { }
    // Sprawdzane w GridToStringTest(), mozna by te funkcjonalnosci rozdzielic

    [TestMethod()]
    public void IsFullTest()
    {
        bool?[] test = new bool?[Width * Height];
        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
            {
                Assert.AreEqual(
                    !TicTacToe.IsFull(test),
                    test.Contains(null)
                );

                test[i * Height + j] = Increment(test[i * Height + j]);
            }

        static bool? Increment(bool? value) => value.HasValue
            ? value.Value ? false : (bool?)null
            : true;
    }

    [TestMethod()]
    public void IsLinedTest()
    {
        Assert.ThrowsException<ArgumentNullException>(() => TicTacToe.IsLined(null!));

        bool value = false;

        bool?[] test = new bool?[Width * Height];
        for (int j = 0; j < 2; j++)
        {
            test = new bool?[Width * Height];
            for (int i = 0; i < Height; i++)
                test[Width * i + i] = value;
            Assert.IsTrue(TicTacToe.IsLined(test), $"#1 Diagonal {j}");
            value ^= true;
        }

        test[0] ^= true;
        Assert.IsFalse(TicTacToe.IsLined(test), "#1 Inner check");

        for (int j = 0; j < 2; j++)
        {
            test = new bool?[Width * Height];
            for (int i = 0; i < Height; i++)
                test[Width * Height - i - 1] = value;
            Assert.IsTrue(TicTacToe.IsLined(test), $"#2 Diagonal {j}");
            value ^= true;
        }

        test[test.Length - 1] ^= true;
        Assert.IsFalse(TicTacToe.IsLined(test), "#2 Inner check");

        for (int j = 0; j < Width; j++)
        {
            test = new bool?[Width * Height];
            for (int i = 0; i < Height; i++)
                test[i + j * Width] = value;
            Assert.IsTrue(TicTacToe.IsLined(test), $"Rows {j}");
            value ^= true;
        }

        for (int j = 0; j < Width; j++)
        {
            test = new bool?[Width * Height];
            for (int i = 0; i < Height; i++)
                test[i * Width + j] = value;
            Assert.IsTrue(TicTacToe.IsLined(test), $"Columns {j}");
            value ^= true;
        }
    }

    [TestMethod()]
    //[ExpectedException(typeof(IndexOutOfRangeException))]
    public void GridSplitTest()
    {
        bool?[] test = new bool?[] { false, true, null };

        Assert.IsFalse(TicTacToe.GridSplit(test, 0, 1, i => i)[0], "0");
        Assert.IsTrue (TicTacToe.GridSplit(test, 1, 1, i => i)[0], "1");
        Assert.IsFalse(TicTacToe.GridSplit(test, 2, 1, i => i)[0].HasValue, "2");
        Assert.IsTrue (TicTacToe.GridSplit(test, 0, 2, i => i)[1], "3");

        // Mozna tu dodac jeszcze wiele przypadkow i petli
        // Exception
        Assert.ThrowsException<IndexOutOfRangeException>(
            () => TicTacToe.GridSplit(test, 0, 4, i => i)
        );
    }

    [TestMethod()]
    public void IsSameForAllTest()
    {
        var trueValues = GenerateTrue();
        foreach (var item in trueValues)
            Assert.IsTrue(TicTacToe.IsSameForAll(item), "TrueTest");

        foreach (var item in GenerateFalse(trueValues))
            Assert.IsFalse(TicTacToe.IsSameForAll(item), "FalseTest");
    }

    private static List<bool?[]> GenerateTrue()
    {
        List<bool?[]> values = new List<bool?[]>();
        bool value = false;
        for (int i = 0; i < 2; i++)
        {
            values.Add(new bool?[] { value, value, value });
            value ^= true;
        }
        return values;
    }

    private static List<bool?[]> GenerateFalse(List<bool?[]> trueValues)
    {
        List<bool?[]> values = new List<bool?[]>();
        var bools = new bool?[] { false, false, false };
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    values.Add(bools.ToArray());
                    bools[2] ^= true;
                }
                bools[1] ^= true;
            }
            bools[0] ^= true;
        }

        for (int i = 0; i < values.Count; i++)
            if (trueValues.Any(values[i].SequenceEqual))
                values.RemoveAt(i);
        return values;
    }

    private const int Width = 3;
    private const int Height = 3;
}
