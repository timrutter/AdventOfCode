using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2016;

public class Advent2016Day04 : Solution
{
    public Advent2016Day04()
    {
        Answer1 = 245102;
        Answer2 = 324;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>();
        var reg = new Regex(@"([a-z-]*)([0-9]*)\[([a-z]*)\]");
        var sum = 0;
        foreach (var line in lines)
        {
            var m = reg.Match(line);
            var g1 = m.Groups[1].Value;
            var g2 = m.Groups[2].Value;
            var g3 = m.Groups[3].Value;
            var counts = Characters.LowerAlphabet.ToDictionary(c => c, c => g1.Count(c1 => c1 == c));

            if (g3 == Get5(counts)) sum += int.Parse(g2);
        }

        return sum;
    }

    private string Get5(Dictionary<char, int> counts)
    {
        var ret = "";
        var i = 0;
        var max = counts.Values.Max();
        while (ret.Length < 5)
        {
            var cnts = counts.Where(c => c.Value == max - i).ToList();
            foreach (var c in cnts) ret += c.Key;

            i++;
        }

        return ret[..5];
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>();
        var reg = new Regex(@"([a-z-]*)([0-9]*)\[([a-z]*)\]");
        foreach (var line in lines)
        {
            var m = reg.Match(line);
            var g1 = m.Groups[1].Value;
            var g2 = int.Parse(m.Groups[2].Value);
            var g3 = m.Groups[3].Value;
            var counts = Characters.LowerAlphabet.ToDictionary(c => c, c => g1.Count(c1 => c1 == c));

            if (g3 != Get5(counts)) continue;
            g1 = new string(g1.Select(c => c == '-' ? ' ' : (char)((c - 97 + g2) % 26 + 97)).ToArray());
            if (g1 == "northpole object storage ") return g2;
        }

        return int.MaxValue;
    }
}