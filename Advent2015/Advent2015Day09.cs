using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2015
{
    public class Advent2015Day09 : Solution
    {
        public Advent2015Day09()
        {
            Answer1 = 251;
            Answer2 = 898;
        }
        public override object ExecutePart1()
        {
            var kvps = DataFile.ReadAllKeyValuePairs<string, int>(" = ").ToList();
            var distances = kvps.Select(k =>
            {
                var bits = k.key.Split(" to ");
                return (bits[0], bits[1], k.value);
            }).ToList();
            distances.AddRange(kvps.Select(k =>
            {
                var bits = k.key.Split(" to ");
                return (bits[1], bits[0], k.value);
            }));

            var uNames = distances.Select(r => r.Item1).Distinct().ToList();

            var routes = new List<int>();
            void GetRouteOptions(List<string> visited, string start, int distanceTravelled)
            {
                if (visited.Contains(start)) return;
                visited.Add(start);
                if (visited.Count == uNames.Count())
                {
                    routes.Add(distanceTravelled);
                    return;
                }
                foreach (var route in distances.Where(d => d.Item1 == start))
                {
                    GetRouteOptions(visited.ToList(), route.Item2, distanceTravelled + route.value);
                }
            }
            foreach (var d in uNames)
                GetRouteOptions(new List<string>(), d, 0);

            return routes.Max();
        }

        public override object ExecutePart2()
        {
            var text = File.ReadAllText(DataFile);

            string LookAndSay(string s)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < s.Length; i++)
                {
                    var startChar = s[i];
                    var startI = i;
                    while (i + 1 < s.Length && s[i + 1] == startChar)
                        i++;
                    sb.Append($"{i - startI + 1}{startChar}");
                }

                return sb.ToString();
            }

            for (int i = 0; i < 50; i++)
            {
                text = LookAndSay(text);
            }

            return text.Length;
        }
    }
}