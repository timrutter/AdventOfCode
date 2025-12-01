using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2015;

public class Advent2015Day02 : Solution
{
    public Advent2015Day02()
    {
        Answer1 = 1586300;
        Answer2 = 3737498;
    }

    public override object ExecutePart1()
    {
        var data = File.ReadAllLines(DataFile);
        var count = 0;
        foreach (var t in data)
        {
            var bits = t.SplitToType<int>().ToList();
            var x = bits[0];
            var y = bits[1];
            var z = bits[2];
            var a1 = x * y;
            var a2 = x * z;
            var a3 = y * z;
            count += a1 * 2 + a2 * 2 + a3 * 2 + new[] { a1, a2, a3 }.Min();
        }

        return count;
    }

    public override object ExecutePart2()
    {
        var data = File.ReadAllLines(DataFile);
        var count = 0;
        foreach (var t in data)
        {
            var bits = t.SplitToType<int>().ToList();
            var x = bits[0];
            var y = bits[1];
            var z = bits[2];
            var a1 = 2 * x + 2 * y;
            var a2 = 2 * x + 2 * z;
            var a3 = 2 * y + 2 * z;
            count += new[] { a1, a2, a3 }.Min() + x * y * z;
        }

        return count;
    }
}