using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;
using Range = AdventOfCode.Helpers.Range;

namespace AdventOfCode.Advent2025;

public class Advent2025Day05 : Solution
{
    public Advent2025Day05()
    {
        Answer1 = 607;
        Answer2 = 342433357244012;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>();
        var ranges = new List<Range>();
        var ranging = true;
        var count = 0;
        foreach (var line in lines)
        {
            if (line == "")
            {
                ranging = false;
                continue;
            }

            if (ranging)
            {
                var bits = line.SplitToType<long>("-").ToList();
                ranges.Add(new Range(bits[0], bits[1]));
            }
            else
            {
                var ingredient = long.Parse(line);
                if (ranges.Any(r => r.InRange(ingredient) )) count++;
            }
        }

        return count;
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>();
        var ranges = lines
            .TakeWhile(line => line != "")
            .Select(line => line.SplitToType<long>("-").ToList())
            .Select(bits => new Range(bits[0], bits[1])).ToList();

        ranges = ranges.CombineRanges();
        return ranges.Sum(range => range.Count());
    }
}