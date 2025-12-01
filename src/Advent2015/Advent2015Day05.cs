using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2015;

public class Advent2015Day05 : Solution
{
    public Advent2015Day05()
    {
        Answer1 = 238;
        Answer2 = 69;
    }

    public override object ExecutePart1()
    {
        var lines = File.ReadAllLines(DataFile).ToList();
        return lines.Count(line =>
        {
            return line.Count(c => c.IsVowel()) >= 3 &&
                   Characters.LowerAlphabet.Any(c => Regex.IsMatch(line, $"{c}{c}")) &&
                   new[] { "ab", "cd", "pq", "xy" }.All(no => !line.Contains(no));
        });
    }

    public override object ExecutePart2()
    {
        var lines = File.ReadAllLines(DataFile).ToList();
        return lines.Count(line =>
        {
            var match = false;
            foreach (var c1 in Characters.LowerAlphabet)
            {
                match = Characters.LowerAlphabet.Any(c2 => Regex.IsMatch(line, $"{c1}{c2}.*{c1}{c2}"));
                if (match) break;
            }

            return match && Characters.LowerAlphabet.Any(c1 => Regex.IsMatch(line, $"{c1}.{c1}"));
        });
    }
}