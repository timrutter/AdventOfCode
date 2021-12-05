using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day10 : Solution
    {
        public Advent2020Day10()
        {
            Answer1 = 2590;
            Answer2 = 226775649501184;
        }
        public override object ExecutePart1()
        {
            var kvps = DataFile.ReadAll<int>().ToList();
            kvps.Insert(0, 0);
            kvps.Sort();
            kvps.Add(kvps[^1] + 3);

            var list = new List<int>();
            for (var i = 1; i < kvps.Count; i++) list.Add(kvps[i] - kvps[i - 1]);
            //Console.WriteLine(list.Count(l => l == 1));
            //Console.WriteLine(list.Count(l => l == 3));

            return list.Count(l => l == 1) * list.Count(l => l == 3);
        }

        public override object ExecutePart2()
        {
            var kvps = DataFile.ReadAll<int>().ToList();
            kvps.Insert(0, 0);
            kvps.Sort();
            kvps.Add(kvps[^1] + 3);

            var dict = new Dictionary<int, long>
            {
                {kvps.Count - 1, 0},
                {kvps.Count - 2, 1}
            };

            long CountLeaves(int start)
            {
                if (start >= kvps.Count) return 0;
                if (dict.ContainsKey(start)) return dict[start];
                long count = 0;
                for (var i = start + 1; i <= start + 4; i++)
                    if (i < kvps.Count && kvps[i] - kvps[start] <= 3)
                        count += CountLeaves(i);

                dict[start] = count;
                return count;
            }

            return CountLeaves(0);
        }
    }
}