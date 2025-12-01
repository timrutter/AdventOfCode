using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2019;

public class Advent2019Day01 : Solution
{
    public Advent2019Day01()
    {
        Answer1 = 3224048;
        Answer2 = 4833211;
    }

    public override object ExecutePart1()
    {
        return DataFile.ReadAll<int>().Select(i => i / 3 - 2).Sum();
    }

    public override object ExecutePart2()
    {
        return DataFile.ReadAll<int>().Select(i =>
        {
            var sum = 0;
            var res = i;
            while (res > 0)
            {
                var val = res / 3 - 2;
                res = val;
                sum += res > 0 ? res : 0;
            }

            return sum;
        }).Sum();
    }
}