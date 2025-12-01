using System;
using System.IO;

namespace AdventOfCode.Advent2020;

public class Advent2020Day05 : Solution
{
    public Advent2020Day05()
    {
        Answer1 = 963;
        Answer2 = 592;
    }

    public override object ExecutePart1()
    {
        var lines = File.ReadAllLines(DataFile);

        var max = int.MinValue;
        foreach (var l in lines)
        {
            int row = 0, column = 0, rowinc = 127, columninc = 7;
            foreach (var c in l)
                switch (c)
                {
                    case 'B':
                        rowinc /= 2;
                        row += rowinc + 1;
                        break;
                    case 'F':
                        rowinc /= 2;
                        break;
                    case 'R':
                        columninc /= 2;
                        column += columninc + 1;
                        break;
                    case 'L':
                        columninc /= 2;
                        break;
                }

            max = Math.Max(max, row * 8 + column);
        }

        return max;
    }

    public override object ExecutePart2()
    {
        var lines = File.ReadAllLines(DataFile);

        var mat = new bool[128, 8];
        foreach (var l in lines)
        {
            int row = 0, column = 0, rowinc = 128, columninc = 8;
            foreach (var c in l)
                switch (c)
                {
                    case 'B':
                        rowinc /= 2;
                        row += rowinc;
                        break;
                    case 'F':
                        rowinc /= 2;
                        break;
                    case 'R':
                        columninc /= 2;
                        column += columninc;
                        break;
                    case 'L':
                        columninc /= 2;
                        break;
                }

            mat[row, column] = true;
        }

        var found = false;
        for (var i = 0; i < mat.Length; i++)
        {
            var r = i / 128 + i / 8;
            var c = i % 8;
            if (!mat[r, c])
            {
                if (!found) continue;
                return r * 8 + c;
            }

            found = true;
        }

        return int.MaxValue;
    }
}