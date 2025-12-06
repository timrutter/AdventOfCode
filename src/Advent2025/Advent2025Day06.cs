using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day06 : Solution
{
    public Advent2025Day06()
    {
        Answer1 = 6503327062445L;
        Answer2 = 9640641878593L;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>();
        var sums = new List<List<string>>();
        foreach (var line in lines)
        {
            var i = 0;
            var l = line.Trim();
            while (true)
            {
                var j = l.IndexOfFirst(c => c == ' ');
                var val = j == -1 ? l : l[..j];
                if (sums.Count <= i) sums.Add([]);
                sums[i].Add(val.Trim());
                if (j == -1) break;
                l = l[j..].Trim();
                i++;
            }
        }

        long total = 0;
        foreach (var sum in sums)
            if (sum.Last() == "+")
                total += sum.Take(sum.Count - 1).Sum(int.Parse);
            else
                total += sum.Take(sum.Count - 1).Aggregate(1L, (t, v) => t * long.Parse(v));

        return total;
    }

    public override object ExecutePart2()
    {
        var board = DataFile.ReadBoard<char>();
        long total = 0;
        var nums = new List<int>();
        for (var i = board.Width - 1; i >= 0; i--)
        {
            var s = new string(board.GetColumn(i).Take(board.Height - 1).ToArray());

            if (string.IsNullOrWhiteSpace(s)) continue;
            nums.Add(int.Parse(s));
            if (board.ValueAt(i, board.Height - 1) == '+')
            {
                total += nums.Sum();
                nums.Clear();
            }

            if (board.ValueAt(i, board.Height - 1) == '*')
            {
                total += nums.Aggregate(1L, (t, v) => t * v);
                nums.Clear();
            }
        }

        return total;
    }
}