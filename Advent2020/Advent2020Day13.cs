using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day13 : Solution
    {
        public Advent2020Day13()
        {
            Answer1 = 4808;
            Answer2 = 741745043105674;
        }
        public override object ExecutePart1()
        {
            var lines = DataFile.ReadAll<string>();
            var startTime = int.Parse(lines[0]);
            var buses = lines[1].Split(",").Where(b => b != "x").Select(int.Parse).ToList();
            var list = new List<(int id, int arrivalTime)>();
            foreach (var bus in buses)
            {
                var arrival = 0;
                while (arrival < startTime)
                    arrival += bus;
                list.Add((bus, arrival));
            }


            var min = list.Min(l => l.arrivalTime);
            var (id, arrivalTime) = list.FirstOrDefault(m1 => m1.arrivalTime == min);
            return (arrivalTime - startTime) * id;
        }

        public override object ExecutePart2()
        {
            var lines = DataFile.ReadAll<string>();
            var buses = lines[1].Split(",").Select(b => b == "x" ? 0 : int.Parse(b)).ToList();
            var sortedBuses = buses.Where(b => b != 0).ToList();
            sortedBuses.Sort();
            long t = 0;
            long step = 1;
            for (var index = 1; index < sortedBuses.Count; index++)
                while (true)
                {
                    var good = true;
                    for (var i = index; i >= 0; i--)
                    {
                        if ((t + buses.IndexOf(sortedBuses[i])) % sortedBuses[i] == 0) continue;
                        good = false;
                        break;
                    }

                    if (good)
                    {
                        step = 1;
                        for (var i = index; i >= 0; i--)
                            step *= sortedBuses[i];

                        break;
                    }

                    t += step;
                }

            return t;
        }
    }
}