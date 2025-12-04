using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day02 : Solution
{
    public Advent2025Day02()
    {
        Answer1 = 13919717792;
        Answer2 = 14582313461;
    }

    public override object ExecutePart1()
    {
        var arr = DataFile.ReadAll<string>()[0].Split(",");
        long total = 0;
        foreach (var range in arr)
        {
            var bits = range.SplitToType<long>("-").ToList();
            var start = bits[0];
            var end = bits[1];
            for (var i = start; i <= end; i++)
                if (IsWrong(i))
                    total += i;
        }

        return total;
    }

    public static bool IsWrong(long i)
    {
        var s = i.ToString();
        if (s.Length % 2 == 1) return false;
        return s[..(s.Length / 2)] == s[(s.Length / 2)..];
    }

    public static bool IsWrong2(long i)
    {
        var s = i.ToString();
        for (var div = 2; div <= s.Length; div++)
            if (s.Length % div == 0)
            {
                var bit = s[..(s.Length / div)];
                var step = s.Length / div;
                var equal = true;
                for (var pos = step; pos < s.Length; pos += step)
                    if (bit != s[pos..(pos + step)])
                    {
                        equal = false;
                        break;
                    }

                if (equal) return true;
            }

        return false;
    }

    public override object ExecutePart2()
    {
        var arr = DataFile.ReadAll<string>()[0].Split(",");
        long total = 0;
        foreach (var range in arr)
        {
            var bits = range.SplitToType<long>("-").ToList();
            var start = bits[0];
            var end = bits[1];
            for (var i = start; i <= end; i++)
                if (IsWrong2(i))
                    total += i;
        }

        return total;
    }
}