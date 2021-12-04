using System.Collections.Generic;
using AdventOfCode.Functions;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day01 : Solution
    {
        public Advent2020Day01()
        {
            Answer1 = 918339;
            Answer2 = 23869440;
        }
        public override object ExecutePart1()
        {
            var arr = DataFile.ReadAll<int>();
            //var hash = new HashSet<int>();
            // foreach (var t in arr)
            // {
            //     var diff = 2020 - t;
            //     if (hash.Contains(diff)) return  diff * t;
            //     hash.Add(t);
            // }

            //return int.MaxValue;
            var (indeces, _) = arr.FindSum(2020);
            return arr[indeces[0]] * arr[indeces[1]];
        }

        public override object ExecutePart2()
        {
            var arr = DataFile.ReadAll<int>();
            var hash = new Dictionary<int, int>();
            for (var index = 0; index < arr.Length; index++)
            {
                var t = arr[index];
                var diff = 2020 - t;
                if (hash.ContainsKey(diff)) return t * (2020 - t - hash[diff]) * hash[diff];

                for (var i = 0; i < index; i++)
                {
                    var sum = t + arr[i];
                    if (sum >= 2020) continue;
                    if (!hash.ContainsKey(sum))
                        hash.Add(t + arr[i], t);
                }
            }

            return int.MaxValue;
        }
    }
}