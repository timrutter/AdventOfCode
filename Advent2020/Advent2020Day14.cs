using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Functions;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day14 : Solution
    {
        public Advent2020Day14()
        {
            Answer1 = 12135523360904;
            Answer2 = 2741969047858;
        }
        public override object ExecutePart1()
        {
            var kvps = DataFile.ReadAllKeyValuePairs<string, string>(" = ");

            var mask = "";
            var memory = new List<long>();
            foreach (var instruction in kvps)
                switch (instruction.key)
                {
                    case "mask":
                        mask = instruction.value;
                        break;
                    default:
                        var loc = int.Parse(instruction.key.Substring(4, instruction.key.Length - 5));
                        var val = long.Parse(instruction.value);
                        while (memory.Count <= loc)
                            memory.Add(0);
                        for (var index = 0; index < mask.Length; index++)
                        {
                            var ch = mask[index];
                            switch (ch)
                            {
                                case '1':
                                    var v = (long)1 << (36 - index - 1);
                                    val |= v;
                                    break;
                                case '0':
                                    val &= ~((long)1 << (36 - index - 1));
                                    break;
                            }
                        }

                        memory[loc] = Maths.ApplyMask(mask, val);

                        break;
                }

            return memory.Sum();
        }

        public override object ExecutePart2()
        {
            var kvps = DataFile.ReadAllKeyValuePairs<string, string>(" = ");

            var mask = "";
            var memory = new Dictionary<long, long>();
            foreach (var instruction in kvps)
                switch (instruction.key)
                {
                    case "mask":
                        mask = instruction.value;
                        break;
                    default:
                        var loc = long.Parse(instruction.key.Substring(4, instruction.key.Length - 5));
                        var val = long.Parse(instruction.value);

                        for (var index = 0; index < mask.Length; index++)
                        {
                            var ch = mask[index];
                            switch (ch)
                            {
                                case '1':
                                    {
                                        var v = (long)1 << (36 - index - 1);
                                        loc |= v;
                                        break;
                                    }
                            }
                        }

                        var locs = new List<long> { loc };
                        for (var index = 0; index < mask.Length; index++)
                            if (mask[index] == 'X')
                            {
                                var ls = locs.ToList();
                                locs = new List<long>();
                                foreach (var t in ls)
                                {
                                    locs.Add(t & ~((long)1 << (36 - index - 1)));
                                    locs.Add(t | ((long)1 << (36 - index - 1)));
                                }
                            }

                        foreach (var l in locs)
                            memory[l] = val;

                        break;
                }

            return memory.Values.Sum();
        }
    }
}