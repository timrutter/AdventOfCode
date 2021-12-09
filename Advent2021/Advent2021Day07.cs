using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021
{
    public class Advent2021Day07 : Solution
    {
        public Advent2021Day07()
        {
            Answer1 = 359648;
            Answer2 = 100727924;
        }
        public override object ExecutePart1()
        {
            var crabs = DataFile.SplitFileToType<int>(",").ToList();
            var last = crabs.Max();
            var dictionary = Functions.Range(0, last).Where(p => crabs.Contains(p))
                .ToDictionary(p => p, p => crabs.Count(c => c == p));
            var minFuel = int.MaxValue;
            for (var targetPos = 0; targetPos <= last; targetPos++)
            {
                var fuel = dictionary.Keys.Sum(c => Math.Abs(targetPos - c) * dictionary[c]);
                if (fuel < minFuel) 
                {
                    minFuel = fuel;
                }
            }
            return minFuel;
        }

        public override object ExecutePart2()
        {
            var crabs = DataFile.SplitFileToType<int>(",").ToList();
            var dictionary = crabs.Distinct().ToDictionary(p => p, p => crabs.Count(c => c == p));
            var posAccumulations = new List<int>{0};
            foreach(var pos in Functions.Range(1, dictionary.Keys.Max())) posAccumulations.Add(posAccumulations.Last() + pos);
            return Functions.Range(0, dictionary.Keys.Max()).Min(targetPos => dictionary.Keys.Sum(pos => posAccumulations[Math.Abs(pos - targetPos)] * dictionary[pos]));
        }
    }
}