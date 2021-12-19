using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers; 
namespace AdventOfCode.Advent2021
{
    public class Advent2021Day14 : Solution
    {
        private readonly Dictionary<string, string> _lines;
        private const string Code = "SVCHKVFKCSHVFNBKKPOC";
        public Advent2021Day14()
        {
            Answer1 = (long) 3058;
            Answer2 = 3447389044530;
            _lines = DataFile.ReadAllFromFileAndSplitToType<string>(" -> ").ToDictionary(c => c[0], c => c[1]);
        }

        private static Dictionary<string, long>  createDictionary(string line)
        {
            var otherDict = new Dictionary<string, long>();
            for (int i = 0; i < line.Length - 1; i++)
            {
                var pair = line[i] + line[i + 1].ToString();
                if (!otherDict.ContainsKey(pair)) otherDict[pair] = 0;
                otherDict[pair]++;
            }

            return otherDict;
        }
        public override object ExecutePart1()
        {
            return Execute(10);
        }
        public override object ExecutePart2()
        {
            return Execute(40);
        }


        private object Execute(int steps)
        {
            var codeDict = createDictionary(Code);
            for (int i = 0; i < steps; i++)
            {
                var origDict = codeDict.Where(c => codeDict[c.Key] > 0).ToDictionary(c => c.Key, c => c.Value);

                foreach (string pair in origDict.Keys)
                {
                    codeDict[pair] -= origDict[pair];
                    var pair1 = pair[0] + _lines[pair];
                    var pair2 = _lines[pair] + pair[1];
                    if (!codeDict.ContainsKey(pair1)) codeDict[pair1] = 0;
                    if (!codeDict.ContainsKey(pair2)) codeDict[pair2] = 0;
                    codeDict[pair1] += origDict[pair];
                    codeDict[pair2] += origDict[pair];
                }
            }

            var letCounts = new Dictionary<char, long>();
            foreach ((string pair, long count) in codeDict.Where(v => v.Value != 0))
            {
                if (!letCounts.ContainsKey(pair[0])) letCounts.Add(pair[0], 0);
                letCounts[pair[0]] += count;
            }

            letCounts[Code.Last()]++;
            return letCounts.Values.Max() - letCounts.Values.Min();
        }
    }
}