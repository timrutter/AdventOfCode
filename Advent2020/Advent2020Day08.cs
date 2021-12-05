using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day08 : Solution
    {
        public Advent2020Day08()
        {
            Answer1 = 1331;
            Answer2 = 1121;
        }
        public override object ExecutePart1()
        {
            var kvps = DataFile.ReadAllKeyValuePairs(" ").ToList();
            return Computer.RunProgram(kvps).Item1;
        }

        public override object ExecutePart2()
        {
            var kvps = DataFile.ReadAllKeyValuePairs(" ").ToList();
            for (var i = 0; i < kvps.Count; i++)
                switch (kvps[i].key)
                {
                    case "jmp":
                    {
                        kvps[i] = ("nop", kvps[i].value);
                        var (acc, err) = Computer.RunProgram(kvps);
                        if (err == 0) return acc;
                        kvps[i] = ("jmp", kvps[i].value);
                        break;
                    }
                    case "nop":
                    {
                        kvps[i] = ("jmp", kvps[i].value);
                        var (acc, err) = Computer.RunProgram(kvps);
                        if (err == 0) return acc;
                        kvps[i] = ("nop", kvps[i].value);
                        break;
                    }
                }

            return int.MaxValue;
        }
    }
}