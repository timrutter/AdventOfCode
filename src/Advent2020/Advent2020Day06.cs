using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020;

public class Advent2020Day06 : Solution
{
    public Advent2020Day06()
    {
        Answer1 = 6351;
        Answer2 = 3143;
    }

    public override object ExecutePart1()
    {
        return DataFile.ReadAllBlankLineSeparatedRecords()
            .Sum(q => q.RemoveAllWhiteSpace().Distinct().Count());
    }

    public override object ExecutePart2()
    {
        return DataFile.ReadAllBlankLineSeparatedRecords().Select(l => l.SplitLines())
            .Sum(s => "abcdefghijklmnopqrstuvwxyz".Count(c => s.All(s1 => s1.Contains(c))));
    }
}