using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day08 : Solution
{
    public Advent2021Day08()
    {
        Answer1 = 264;
        Answer2 = 1063760;
    }

    public override object ExecutePart1()
    {
        var signals = DataFile.ReadAllKeyValuePairs<string, string>("|");
        var lengths = new[] { 2, 3, 4, 7 };
        var outputs = signals.Select(s => s.value).SelectMany(v =>
            v.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)).ToList();
        return outputs.Count(v => lengths.Contains(v.Length));
    }

    private static Dictionary<string, int> GetCodes(IReadOnlyCollection<string> values)
    {
        var ret = new Dictionary<int, string>();
        const string all = "abcdefg";
        ret[1] = values.First(v => v.Length == 2);
        ret[4] = values.First(v => v.Length == 4);
        ret[7] = values.First(v => v.Length == 3);
        ret[8] = values.First(v => v.Length == 7);

        var len6 = values.Where(v => v.Length == 6).ToList();
        var v069Missing = len6.Select(v => all.First(c => !v.Contains(c))).ToList();
        var wire4 = v069Missing.First(c => !ret[4].Contains(c));
        ret[9] = len6.First(v => !v.Contains(wire4));
        ret[0] = len6.Where(l => l != ret[9]).First(l => ret[1].All(l.Contains));
        ret[6] = len6.First(l => l != ret[9] && l != ret[0]);
        var len5 = values.Where(v => v.Length == 5).ToList();
        ret[3] = len5.First(l => ret[1].All(l.Contains));
        ret[2] = len5.Where(l => l != ret[3]).First(l => l.Contains(wire4));
        ret[5] = len5.First(l => l != ret[3] && l != ret[2]);

        return ret.ToDictionary(r => r.Value, r => r.Key);
    }

    public override object ExecutePart2()
    {
        var signals = DataFile.ReadAllKeyValuePairs<string, string>("|");
        var outputSum = 0;
        foreach (var (key, value) in signals)
        {
            var codes = GetCodes(key.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList());
            var os = value.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var val = "";
            foreach (var o in os)
            {
                var odistinct = o.Distinct().ToList();
                var k = codes.Keys.First(c => odistinct.Count == c.Length && odistinct.All(c.Contains));
                val += codes[k];
            }

            outputSum += int.Parse(val);
        }

        return outputSum;
    }
}