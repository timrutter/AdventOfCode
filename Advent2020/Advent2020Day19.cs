using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day19 : Solution
    {
        public Advent2020Day19()
        {
            Answer1 = 230;
            Answer2 = 341;
        }
        private static List<string> Append(List<string> l1, List<string> l2)
        {
            return (from s1 in l1 from s2 in l2 select $"{s1}{s2}").ToList();
        }
        private static List<string> Evaluate(int rule, IReadOnlyDictionary<int, string> rules)
        {
            // var r = rules[rule];
            // if (r.StartsWith("\""))
            //     return new List<string> {$"{r.RemoveQuotes()}"};
            // return r.Split('|').SelectMany(bit => bit.Trim()
            //         .SplitToType<int>(" ")
            //         .Select(i => Evaluate(i, rules))
            //         .Aggregate(new List<string>(),
            //             (current, l) => current.Count == 0 ? l : Append(current.ToList(), l)))
            //     .ToList();
            return null;
        }

        public override object ExecutePart1()
        {
            return -1;
            var rules = DataFile.ReadAllKeyValuePairs<int, string>(": ")
                .ToDictionary(d => d.key, d => d.value);
            var strings = "Year2020\\Data\\Day19a.txt".ReadAll<string>().ToList();

            return strings.Count(s => Evaluate(0, rules).Contains(s));
        }

        public override object ExecutePart2()
        {
            return -1;
            var rules = DataFile.ReadAllKeyValuePairs<int, string>(": ")
                .ToDictionary(d => d.key, d => d.value);
            var strings = "Year2020\\Data\\Day19a.txt".ReadAll<string>().ToList();

            var ret42 = Evaluate(42, rules);
            var ret31 = Evaluate(31, rules);

            return strings.Count(s =>
            {
                var temp = s;
                var count42 = 0;
                while (temp.Length > 0)
                {
                    var s42 = ret42.FirstOrDefault(temp.StartsWith);
                    if (s42 == null) break;
                    temp = temp[s42.Length..];
                    count42++;
                    if (count42 <= 1) continue;
                    var count31 = 0;
                    var ok = true;
                    while (temp.Length > 0)
                    {
                        var s31 = ret31.FirstOrDefault(temp.EndsWith);
                        if (s31 == null)
                        {
                            ok = false;
                            break;
                        }

                        temp = temp.Substring(0, temp.Length - s31.Length);
                        count31++;
                    }

                    if (count31 == 0)
                        return false;
                    if (ok && count31 > 0 && count31 < count42 && temp.Length == 0)
                        // Console.WriteLine(s);
                        return true;
                }

                return false;
            });
        }
    }
}