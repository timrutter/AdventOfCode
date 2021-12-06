using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Advent2015
{
    public class Advent2015Day07 : Solution
    {
        public Advent2015Day07()
        {
            Answer1 = 16076;
            Answer2 = 2797;
        }
        public override object ExecutePart1()
        {
            var lines = File.ReadAllLines(DataFile).Select(l => (
                 Regex.Match(l, @"([a-z0-9]*)? ?(AND|OR|NOT|LSHIFT|RSHIFT) ([a-z0-9]*) -> ([a-z]*)"), l)).ToList();

            var instructions = new List<(string operation, string in1, string in2, string out1)>();
            var dict = new Dictionary<string, ushort>();
            foreach (var match in lines)
            {
                if (match.Item1.Success)
                    instructions.Add((match.Item1.Groups[2].Value, match.Item1.Groups[1].Value, match.Item1.Groups[3].Value, match.Item1.Groups[4].Value));
                else
                {
                    var bits = match.l.Split(" -> ");
                    instructions.Add(("ASIGN", bits[0], "", bits[1]));

                }
            }

            var inst2 = instructions.ToList();

            void Trace()
            {

                while (true)
                {
                    for (var index = 0; index < instructions.Count; index++)
                    {
                        var line = instructions[index];
                        switch (line.operation)
                        {
                            case "NOT":
                                {
                                    if (!dict.Keys.Contains(line.in2)) continue;
                                    //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                    var val = dict[line.in2];
                                    val = (ushort)~val;

                                    dict[line.out1] = val;
                                    instructions.RemoveAt(index);
                                    index--;
                                    break;
                                }
                            case "AND":
                                {

                                    if ((!int.TryParse(line.in1, out var inNum) && !dict.Keys.Contains(line.in1)) || !dict.Keys.Contains(line.in2)) continue;
                                    //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                    var val1 = int.TryParse(line.in1, out inNum) ? inNum : dict[line.in1];
                                    var val3 = dict[line.in2];
                                    dict[line.out1] = (ushort)(val1 & val3);
                                    instructions.RemoveAt(index);
                                    index--;
                                    break;
                                }
                            case "OR":
                                {
                                    if (!dict.Keys.Contains(line.in1) || !dict.Keys.Contains(line.in2)) continue;
                                    //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                    var val1 = dict[line.in1];
                                    var val3 = dict[line.in2];
                                    dict[line.out1] = (ushort)(val1 | val3);
                                    instructions.RemoveAt(index);
                                    index--;
                                    break;
                                }
                            case "LSHIFT":
                                {
                                    if (!dict.Keys.Contains(line.in1)) continue;
                                    //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                    var val1 = dict[line.in1];
                                    var val3 = int.Parse(line.in2);
                                    dict[line.out1] = (ushort)(val1 << val3);
                                    instructions.RemoveAt(index);
                                    index--;
                                    break;
                                }
                            case "RSHIFT":
                                {
                                    if (!dict.Keys.Contains(line.in1)) continue;
                                    //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                    var val1 = dict[line.in1];
                                    var val3 = int.Parse(line.in2);
                                    dict[line.out1] = (ushort)(val1 >> val3);
                                    instructions.RemoveAt(index);
                                    index--;
                                    break;
                                }
                            case "ASIGN":
                                {
                                    if (ushort.TryParse(line.in1, out var val))
                                    {
                                        //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                        dict.Add(line.out1, val);
                                        instructions.RemoveAt(index);
                                        index--;
                                        break;
                                    }
                                    if (!dict.Keys.Contains(line.in1)) continue;
                                    //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                    dict[line.out1] = dict[line.in1];
                                    instructions.RemoveAt(index);
                                    index--;
                                    break;
                                }
                        }
                    }

                    if (dict.ContainsKey("a"))
                        return;
                    Trace();
                }
            }

            Trace();

            dict.Clear();
            instructions = inst2.ToList();
            var index = instructions.FindIndex(i => i.operation == "ASIGN" && i.out1 == "b");
            instructions[index] = ("ASIGN", "16076", "", "b");
            Trace();
            return -1;
        }

        public override object ExecutePart2()
        {
            return int.MaxValue;
        }
    }
}