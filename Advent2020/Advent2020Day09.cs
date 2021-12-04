using System.Linq;
using AdventOfCode.Functions;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day09 : Solution
    {
        public Advent2020Day09()
        {
            Answer1 = 1309761972;
            Answer2 = 177989832;
        }
        public override object ExecutePart1()
        {
            var kvps = DataFile.ReadAll<long>().ToList();
            var count = 25;
            for (var i = count; i < kvps.Count; i++)
            {
                var (indeces, _) = kvps.Skip(i - count).Take(count).ToList().FindSum(kvps[i]);
                if (indeces == null)
                    return kvps[i];
            }

            return int.MaxValue;
        }

        public override object ExecutePart2()
        {
            var target = 1309761972;
            var kvps = DataFile.ReadAll<long>().ToList();

            for (var i = 2; i < kvps.Count - 1; i++)
            {
                var (values, _, _) = kvps.FindContiguousSum(target, i);
                if (values == null) continue;
                return values.Min() + values.Max();
            }

            return int.MaxValue;
        }
    }
}