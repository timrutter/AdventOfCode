using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day15 : Solution
    {
        public Advent2020Day15()
        {
            Answer1 = 475;
            Answer2 = 11261;
        }
        public override object ExecutePart1()
        {
            var input = new List<int> { 6, 4, 12, 1, 20, 0, 16 };

            var spoken = new List<int>();
            for (var i = 0; i < 50; i++)
            {
                if (i < input.Count)
                {
                    spoken.Add(input[i]);
                    continue;
                }

                var ind = spoken.Take(spoken.Count - 1).ToList().LastIndexOf(spoken.Last());
                if (ind == -1)
                    spoken.Add(0);
                else
                    spoken.Add(i - (ind + 1));
            }

            return spoken[49];
        }

        public override object ExecutePart2()
        {
            var input = new List<int> { 6, 4, 12, 1, 20, 0, 16 };

            var spoken = new Dictionary<int, int>();
            var last = 0;
            for (var i = 0; i < input.Count; i++)
                spoken.Add(input[i], i);

            for (var i = input.Count + 1; i < 30000000; i++)
                if (!spoken.ContainsKey(last))
                {
                    spoken[last] = i - 1;
                    last = 0;
                }
                else
                {
                    var next = i - 1 - spoken[last];
                    spoken[last] = i - 1;
                    last = next;
                }

            return last;
        }
    }
}