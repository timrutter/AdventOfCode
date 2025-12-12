using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day12 : Solution
{
    public Advent2025Day12()
    {
        Answer1 = null;
        Answer2 = null;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>();
        var lineNo = 0;

        var count = 0;

        foreach (var region in lines.Skip(lineNo))
        {
            var losses = new List<double> { 0.5, 2, 1, 1, 0, 0 };
            var bits = region.Split(": ").ToList();
            var xy = bits[0].SplitToType<int>().ToList();
            var presentsToFit = bits[1].SplitToType<int>(" ").ToList();
            var totalPresentArea = presentsToFit.Select((p, i) => p * (9 - losses[i])).Sum();
            var area = xy[0] * xy[1];
            if (totalPresentArea > area)
            {
                Console.WriteLine("TOO BIG");
                continue;
            }

            Console.WriteLine(
                $"{xy[0]}x{xy[1]} {string.Join('|', presentsToFit)} area:{area} presentArea {totalPresentArea}");
            count++;
        }
        return count;
    }


    public override object ExecutePart2()
    {
        return int.MaxValue;
    }
}