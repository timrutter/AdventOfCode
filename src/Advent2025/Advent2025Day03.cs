using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day03 : Solution
{
    public Advent2025Day03()
    {
        Answer1 = 17196L;
        Answer2 = 171039099596062L;
    }

    public override object ExecutePart1()
    {
        var arr = DataFile.ReadAll<string>();
        return arr.Sum(Get2Max);
    }

    public override object ExecutePart2()
    {
        var arr = DataFile.ReadAll<string>();
        long sum = 0;

        Parallel.ForEach(arr, line =>
        {
            var max = GetMax(line, 12);
            Console.WriteLine(max);
            Interlocked.Add(ref sum, max);
        });
        return sum;
    }

    private static long Get2Max(string line)
    { 
        var max = 0;
        for (var j = 0; j < line.Length - 1; j++)
        {
            for (var k = j + 1; k < line.Length; k++)
            {
                var num = int.Parse($"{line[j]}{line[k]}");
                max = Math.Max(num, max);
            }
        }

        return max;
    }

    private static readonly ConcurrentDictionary<string, long> Seen = new();

    public static long GetMax(string line, int length)
    {
        if (Seen.TryGetValue($"{line}_{length}", out var m1))
            return m1;
        long max = 0;
        if (line.Length == length)
        {
            Seen[$"{line}_{length}" ] = long.Parse(line);
            return Seen[$"{line}_{length}" ];
        }
        if (length == 1)
        { 
            Seen[$"{line}_{length}" ] =  line.Select(c => c - '0').Max();
            return Seen[$"{line}_{length}" ];
        }
        
        for (var i = 0; i <= line.Length - length; i++)
        {
            for (var j = i + 1; j <= line.Length - (length - 1); j++)
            {
                var s = line[j..];
                var submax = GetMax(s, length - 1);
                var s2 = $"{line[i]}{submax}";
                max = Math.Max(max, long.Parse( s2));
            }
        }

        Seen[$"{line}_{length}" ] = max;
        return max;
    }
}