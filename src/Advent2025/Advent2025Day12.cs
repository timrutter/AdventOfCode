using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day12 : Solution
{
    public Advent2025Day12()
    {
        Answer1 = 555;
        Answer2 = 0;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>();
        var count = 0;

        // this is pure estimation! Looking at each shape, the min loss per shape due to tessilation as a large block of
        // the same shape is as follows
        var losses = new List<double> { 0.5, 2, 1, 1, 0, 0 };
        foreach (var region in lines.Skip(30))
        {
            var bits = region.Split(": ").ToList();
            var xy = bits[0].SplitToType<int>("x").ToList();
            var presentsToFit = bits[1].SplitToType<int>(" ").ToList();
            var totalPresentArea = presentsToFit.Select((p, i) => p * (9 - losses[i])).Sum();
            var area = xy[0] * xy[1];
            // but whenever its not big enough the area - totalPresentArea < -400 and whenever its big enough its > 150
            // i.e its never very close to fitting, only easily big enough or nowhere near big enough (carefully engineering to be the case?)
            // so can only conclude that the loses are the biggest significant statistical factor therefore
            // don't need to consider the arrangements at all, other than each set of presents being in a big block of the same
            if (totalPresentArea > area) continue;

            //Console.WriteLine(
            //    $"{xy[0]}x{xy[1]} {string.Join('|', presentsToFit)} area:{area} presentArea {totalPresentArea}");
            count++;
        }
        return count;
    }


    public override object ExecutePart2()
    {
        return 0;
    }
}