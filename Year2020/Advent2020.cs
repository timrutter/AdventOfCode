using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AdventOfCode.Functions;

namespace AdventOfCode.Year2020
{

    public static class Advent2020
    {
        #region Methods

        public static int Puzzle1Part1()
        {
            var arr = "Year2020\\Data\\Day01.txt".ReadAll<int>();
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

        public static int Puzzle1Part2()
        {
            var arr = "Year2020\\Data\\Day01.txt".ReadAll<int>();
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

        public static int Puzzle2Part1()
        {
            var kvps = "Year2020\\Data\\Day02.txt".ReadAllKeyValuePairs();
            var count = 0;
            foreach (var (key, value) in kvps)
            {
                var pword = value.Trim();
                var bits = key.Split(" ");
                var letter = bits[1][0];
                bits = bits[0].Split("-");
                var min = int.Parse(bits[0]);
                var max = int.Parse(bits[1]);
                var letcount = pword.Count(c => c == letter);
                if (letcount >= min && letcount <= max)
                    count++;
            }

            return count;
        }

        public static int Puzzle2Part2()
        {
            var kvps = "Year2020\\Data\\Day02.txt".ReadAllKeyValuePairs();
            var count = 0;
            foreach (var (key, value) in kvps)
            {
                var pword = value.Trim();
                var bits = key.Split(" ");
                var letter = bits[1][0];
                bits = bits[0].Split("-");
                var min = int.Parse(bits[0]);
                var max = int.Parse(bits[1]);

                var minChar = pword.Length >= min ? pword[min - 1] : (char) 0;
                var maxChar = pword.Length >= max ? pword[max - 1] : (char) 0;
                if ((minChar == letter) ^ (maxChar == letter))
                    count++;
            }

            return count;
        }

        public static int Puzzle3Part1()
        {
            var trees = File.ReadAllLines("Year2020\\Data\\Day03.txt");
            return Puzzle3Part1Internal(trees, 3, 1);
        }

        private static int Puzzle3Part1Internal(string[] trees, int xInc, int yInc)
        {
            var count = 0;
            var x = 0;
            for (var y = 0; y < trees.Length; y += yInc)
            {
                if (trees[y][x] == '#') count++;
                x = (x + xInc) % trees[0].Length;
            }

            return count;
        }

        public static int Puzzle3Part2()
        {
            var trees = File.ReadAllLines("Year2020\\Data\\Day03.txt");
            return Puzzle3Part1Internal(trees, 1, 1) *
                   Puzzle3Part1Internal(trees, 3, 1) *
                   Puzzle3Part1Internal(trees, 5, 1) *
                   Puzzle3Part1Internal(trees, 7, 1) *
                   Puzzle3Part1Internal(trees, 1, 2);
        }

        public static int Puzzle4Part1()
        {
            var passports = "Year2020\\Data\\Day04.txt".ReadAllBlankLineSeparatedRecords(true);
            var count = 0;
            var required = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};


            foreach (var passport in passports)
            {
                var bits = passport.Trim().Split(" ");
                var keys = bits.Select(b => b.Split(":")[0]).ToList();
                if (required.All(r => keys.Contains(r)))
                    count++;
            }


            return count;
        }

        public static int Puzzle4Part2()
        {
            var passports = "Year2020\\Data\\Day04.txt".ReadAllBlankLineSeparatedRecords(true);
            var count = 0;
            var required = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

            foreach (var passport in passports)
            {
                var bits = passport.Trim().Split(" ");
                var dict = bits.ToDictionary(b => b.Split(":")[0], b => b.Split(":")[1]);
                var valid = required.All(r => dict.Keys.Contains(r));
                if (!valid) continue;
                foreach (var kvp in dict)
                {
                    switch (kvp.Key)
                    {
                        case "byr":
                        {
                            valid &= kvp.Value.Length == 4 && int.TryParse(kvp.Value, out var yr) && yr >= 1920 &&
                                     yr <= 2002;
                            break;
                        }
                        case "iyr":
                        {
                            valid &= kvp.Value.Length == 4 && int.TryParse(kvp.Value, out var yr) && yr >= 2010 &&
                                     yr <= 2020;
                            break;
                        }
                        case "eyr":
                        {
                            valid &= kvp.Value.Length == 4 && int.TryParse(kvp.Value, out var yr) && yr >= 2020 &&
                                     yr <= 2030;
                            break;
                        }
                        case "hgt":
                        {
                            valid &= kvp.Value.EndsWith("cm")
                                ? int.TryParse(kvp.Value.Substring(0, kvp.Value.Length - 2), out var hecm) &&
                                  hecm >= 150 && hecm <= 193
                                : kvp.Value.EndsWith("in") &&
                                  int.TryParse(kvp.Value.Substring(0, kvp.Value.Length - 2), out var he) && he >= 59 &&
                                  he <= 76;
                            break;
                        }
                        case "hcl":
                        {
                            valid &= kvp.Value.Length == 7 && kvp.Value.StartsWith("#") &&
                                     kvp.Value.Substring(1).All(c => "0123456789abcdef".Contains(c));
                            break;
                        }
                        case "ecl":
                        {
                            var colours = new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
                            valid &= colours.Contains(kvp.Value);
                            break;
                        }
                        case "pid":
                        {
                            valid &= kvp.Value.Length == 9 && kvp.Value.All(char.IsDigit);
                            break;
                        }
                        case "cid":
                            break;
                    }

                    if (!valid) break;
                }

                if (valid)
                    count++;
            }

            return count;
        }

        public static int Puzzle5Part1()
        {
            var lines = File.ReadAllLines("Year2020\\Data\\Day05.txt");

            var max = int.MinValue;
            foreach (var l in lines)
            {
                int row = 0, column = 0, rowinc = 127, columninc = 7;
                foreach (var c in l)
                    switch (c)
                    {
                        case 'B':
                            rowinc /= 2;
                            row += rowinc + 1;
                            break;
                        case 'F':
                            rowinc /= 2;
                            break;
                        case 'R':
                            columninc /= 2;
                            column += columninc + 1;
                            break;
                        case 'L':
                            columninc /= 2;
                            break;
                    }

                max = Math.Max(max, row * 8 + column);
            }

            return max;
        }

        public static int Puzzle5Part2()
        {
            var lines = File.ReadAllLines("Year2020\\Data\\Day05.txt");

            var mat = new bool [128, 8];
            foreach (var l in lines)
            {
                int row = 0, column = 0, rowinc = 128, columninc = 8;
                foreach (var c in l)
                    switch (c)
                    {
                        case 'B':
                            rowinc /= 2;
                            row += rowinc;
                            break;
                        case 'F':
                            rowinc /= 2;
                            break;
                        case 'R':
                            columninc /= 2;
                            column += columninc;
                            break;
                        case 'L':
                            columninc /= 2;
                            break;
                    }

                mat[row, column] = true;
            }

            var found = false;
            for (var i = 0; i < mat.Length; i++)
            {
                var r = i / 128 + i / 8;
                var c = i % 8;
                if (!mat[r, c])
                {
                    if (!found) continue;
                    return r * 8 + c;
                }

                found = true;
            }

            return int.MaxValue;
        }

        public static int Puzzle6Part1()
        {
            return "Year2020\\Data\\Day06.txt".ReadAllBlankLineSeparatedRecords()
                .Sum(q => q.RemoveAllWhiteSpace().Distinct().Count());
        }

        public static int Puzzle6Part2()
        {
            return "Year2020\\Data\\Day06.txt".ReadAllBlankLineSeparatedRecords().Select(l => l.SplitLines())
                .Sum(s => "abcdefghijklmnopqrstuvwxyz".Count(c => s.All(s1 => s1.Contains(c))));
        }

        public static int Puzzle7Part1()
        {
            var lines = "Year2020\\Data\\Day07.txt".ReadAllKeyValuePairs("contain");
            var dict = new List<Bag>();
            foreach (var line in lines)
            {
                var list = new List<(int, string)>();

                if (line.value != " no other bags.")
                {
                    var bits = line.value.Split(",");
                    foreach (var bit in bits)
                        list.Add((int.Parse(bit.Substring(1, 2)), CleanColour(bit.Substring(2))));
                }

                var color = CleanColour(line.key);
                var b = FindBags(dict, color);
                if (b.Count == 0)
                    dict.Add(new Bag(null, CleanColour(line.key), list));
                else
                    foreach (var bag in b)
                        bag.Children.AddRange(list.Select(l => new Bag(bag, l.Item2, null, l.Item1)));
            }


            var hashset = new HashSet<string>();
            GetAllParents(hashset, dict, "shiny gold");
            return hashset.Count();
        }

        private static void GetAllParents(HashSet<string> hashset, List<Bag> dict, string name)
        {
            var bags = FindBags(dict, name).ToList();
            foreach (var bag in bags)
            foreach (var allParent in bag.GetAllParents())
            {
                if (hashset.Contains(allParent.Color)) continue;
                hashset.Add(allParent.Color);
                GetAllParents(hashset, dict, allParent.Color);
            }
        }

        private static List<Bag> FindBags(List<Bag> dict, string color)
        {
            return dict.SelectMany(d => d.FindBags(color)).Where(b => b != null).ToList();
        }

        private static string CleanColour(string str)
        {
            return Regex.Replace(str, " bag.*", "").Trim();
        }

        public static int Puzzle7Part2()
        {
            var lines = "Year2020\\Data\\Day07.txt".ReadAllKeyValuePairs("contain");
            var dict = new Dictionary<string, Bag>();
            foreach (var line in lines)
            {
                var list = new List<(int, string)>();

                if (line.value != " no other bags.")
                {
                    var bits = line.value.Split(",");
                    foreach (var bit in bits)
                        list.Add((int.Parse(bit.Substring(1, 2)), CleanColour(bit.Substring(2))));
                }

                var bagName = CleanColour(line.key);
                var bag = dict.GetOrAdd(bagName, () => new Bag(null, bagName, null));

                foreach (var l in list)
                {
                    Bag childBag;
                    if (dict.ContainsKey(l.Item2))
                    {
                        childBag = dict[l.Item2];
                    }
                    else
                    {
                        childBag = new Bag(bag, l.Item2, null, l.Item1);
                        dict.Add(l.Item2, childBag);
                    }

                    for (var i = 0; i < l.Item1; i++)
                        bag.Children.Add(childBag);
                }
            }


            return dict["shiny gold"].CountBags() - 1;
        }


        public static int Puzzle8Part1()
        {
            var kvps = "Year2020\\Data\\Day08.txt".ReadAllKeyValuePairs(" ").ToList();
            return Computer.RunProgram(kvps).Item1;
        }


        public static int Puzzle8Part2()
        {
            var kvps = "Year2020\\Data\\Day08.txt".ReadAllKeyValuePairs(" ").ToList();
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

        public static long Puzzle9Part1()
        {
            var kvps = "Year2020\\Data\\Day09.txt".ReadAll<long>().ToList();
            var count = 25;
            for (var i = count; i < kvps.Count; i++)
            {
                var (indeces, _) = kvps.Skip(i - count).Take(count).ToList().FindSum(kvps[i]);
                if (indeces == null)
                    return kvps[i];
            }

            return int.MaxValue;
        }

        public static long Puzzle9Part2()
        {
            var target = 1309761972;
            var kvps = "Year2020\\Data\\Day09.txt".ReadAll<long>().ToList();

            for (var i = 2; i < kvps.Count - 1; i++)
            {
                var (values, _, _) = kvps.FindContiguousSum(target, i);
                if (values == null) continue;
                return values.Min() + values.Max();
            }

            return int.MaxValue;
        }

        public static int Puzzle10Part1()
        {
            var kvps = "Year2020\\Data\\Day10.txt".ReadAll<int>().ToList();
            kvps.Insert(0, 0);
            kvps.Sort();
            kvps.Add(kvps[kvps.Count - 1] + 3);

            var list = new List<int>();
            for (var i = 1; i < kvps.Count; i++) list.Add(kvps[i] - kvps[i - 1]);
            //Console.WriteLine(list.Count(l => l == 1));
            //Console.WriteLine(list.Count(l => l == 3));

            return list.Count(l => l == 1) * list.Count(l => l == 3);
        }

        public static long Puzzle10Part2()
        {
            var kvps = "Year2020\\Data\\Day10.txt".ReadAll<int>().ToList();
            kvps.Insert(0, 0);
            kvps.Sort();
            kvps.Add(kvps[^1] + 3);

            var dict = new Dictionary<int, long>
            {
                {kvps.Count - 1, 0},
                {kvps.Count - 2, 1}
            };

            long CountLeaves(int start)
            {
                if (start >= kvps.Count) return 0;
                if (dict.ContainsKey(start)) return dict[start];
                long count = 0;
                for (var i = start + 1; i <= start + 4; i++)
                    if (i < kvps.Count && kvps[i] - kvps[start] <= 3)
                        count += CountLeaves(i);

                dict[start] = count;
                return count;
            }

            return CountLeaves(0);
        }

        public static int Puzzle11Part1()
        {
            var board2 = "Year2020\\Data\\Day11.txt".ReadBoard();
            //board2.Dump();

            int CountOccupied(Board board, int x, int y)
            {
                var count = 0;
                if (board.Jump(x, y).Left().Value == '#') count++;
                if (board.Jump(x, y).Right().Value == '#') count++;
                if (board.Jump(x, y).Up().Value == '#') count++;
                if (board.Jump(x, y).Down().Value == '#') count++;
                if (board.Jump(x, y).UpLeft().Value == '#') count++;
                if (board.Jump(x, y).UpRight().Value == '#') count++;
                if (board.Jump(x, y).DownLeft().Value == '#') count++;
                if (board.Jump(x, y).DownRight().Value == '#') count++;
                return count;
            }

            var changeCount = int.MaxValue;
            while (changeCount > 0)
            {
                changeCount = 0;
                var board1 = new Board(board2);

                for (var x = 0; x < board1.Width; x++)
                for (var y = 0; y < board1.Height; y++)
                    switch (board1.Jump(x, y).Value)
                    {
                        case '.':
                            continue;
                        case 'L' when CountOccupied(board1, x, y) == 0:
                            board2.SetValueAt(x, y, '#');
                            changeCount++;
                            break;
                        case '#' when CountOccupied(board1, x, y) >= 4:
                            board2.SetValueAt(x, y, 'L');
                            changeCount++;
                            break;
                    }

                //board2.Dump();
            }

            return board2.CountValues('#');
        }

        public static int Puzzle11Part2()
        {
            var board2 = "Year2020\\Data\\Day11.txt".ReadBoard();

            int CountOccupied(Board board, int x, int y)
            {
                var count = 0;

                if (board.Jump(x, y).Left().LeftWhile(c => c == '.').Value == '#') count++;
                if (board.Jump(x, y).Right().RightWhile(c => c == '.').Value == '#') count++;
                if (board.Jump(x, y).Up().UpWhile(c => c == '.').Value == '#') count++;
                if (board.Jump(x, y).Down().DownWhile(c => c == '.').Value == '#') count++;
                if (board.Jump(x, y).UpLeft().UpLeftWhile(c => c == '.').Value == '#') count++;
                if (board.Jump(x, y).UpRight().UpRightWhile(c => c == '.').Value == '#') count++;
                if (board.Jump(x, y).DownLeft().DownLeftWhile(c => c == '.').Value == '#') count++;
                if (board.Jump(x, y).DownRight().DownRightWhile(c => c == '.').Value == '#') count++;

                return count;
            }

            var changeCount = int.MaxValue;
            while (changeCount > 0)
            {
                changeCount = 0;
                var board1 = new Board(board2);
                for (var x = 0; x < board1.Width; x++)
                for (var y = 0; y < board1.Height; y++)
                    switch (board1.Jump(x, y).Value)
                    {
                        case '.':
                            continue;
                        case 'L' when CountOccupied(board1, x, y) == 0:
                            board2.SetValueAt(x, y, '#');
                            changeCount++;
                            break;
                        case '#' when CountOccupied(board1, x, y) >= 5:
                            board2.SetValueAt(x, y, 'L');
                            changeCount++;
                            break;
                    }

                //board2.Dump();
            }

            return board2.CountValues('#');
        }

        public static int Puzzle12Part1()
        {
            var instructions = "Year2020\\Data\\Day12.txt".ReadAll<string>();
            var epos = 0;
            var npos = 0;
            var direction = 90;
            string[] directions = {"N", "E", "S", "W"};

            void Move(string dir, int distance)
            {
                switch (dir)
                {
                    case "N":
                        npos += distance;
                        break;
                    case "S":
                        npos -= distance;
                        break;
                    case "E":
                        epos += distance;
                        break;
                    case "W":
                        epos -= distance;
                        break;
                }
            }

            foreach (var instruction in instructions)
            {
                var command = instruction.Substring(0, 1);
                var distance = int.Parse(instruction.Substring(1));
                switch (command)
                {
                    case "N":
                    case "S":
                    case "E":
                    case "W":
                        Move(command, distance);
                        //Console.WriteLine($"{instruction}: epos={epos} npos={npos}");
                        break;
                    case "F":
                        Move(directions[direction / 90], distance);
                        //Console.WriteLine($"{instruction}: epos={epos} npos={npos}");
                        break;
                    case "R":
                        direction = (direction + distance) % 360;
                        //Console.WriteLine($"{instruction}: direction={direction}");
                        break;
                    case "L":
                        direction = (direction - distance) % 360;
                        if (direction < 0) direction += 360;
                        //Console.WriteLine($"{instruction}: direction={direction}");
                        break;
                }
            }

            return Math.Abs(epos) + Math.Abs(npos);
        }

        public static int Puzzle12Part2()
        {
            var instructions = "Year2020\\Data\\Day12.txt".ReadAll<string>();
            var epos = 0;
            var npos = 0;
            var wpepos = 10;
            var wpnpos = 1;

            void MoveWaypoint(string dir, int distance)
            {
                switch (dir)
                {
                    case "N":
                        wpnpos += distance;
                        break;
                    case "S":
                        wpnpos -= distance;
                        break;
                    case "E":
                        wpepos += distance;
                        break;
                    case "W":
                        wpepos -= distance;
                        break;
                }
            }

            void RotateWaypoint(int directionChange)
            {
                switch (directionChange)
                {
                    case 90:
                    {
                        var temp = wpnpos;
                        wpnpos = -wpepos;
                        wpepos = temp;
                        break;
                    }
                    case 180:
                        wpepos = -wpepos;
                        wpnpos = -wpnpos;
                        break;
                    case 270:
                    {
                        var temp = wpnpos;
                        wpnpos = wpepos;
                        wpepos = -temp;
                        break;
                    }
                }
            }

            foreach (var instruction in instructions)
            {
                var command = instruction.Substring(0, 1);
                var distance = int.Parse(instruction.Substring(1));
                switch (command)
                {
                    case "N":
                    case "S":
                    case "E":
                    case "W":
                    {
                        MoveWaypoint(command, distance);
                        //Console.WriteLine($"{instruction}: epos={epos} npos={npos}");
                        break;
                    }
                    case "F":
                    {
                        epos += wpepos * distance;
                        npos += wpnpos * distance;
                        //Console.WriteLine($"{instruction}: epos={epos} npos={npos}");
                        break;
                    }
                    case "R":
                    {
                        var directionChange = distance % 360;
                        RotateWaypoint(directionChange);
                        break;
                    }
                    case "L":
                    {
                        var directionChange = -distance % 360 + 360;
                        RotateWaypoint(directionChange);
                        break;
                    }
                }
            }

            return Math.Abs(epos) + Math.Abs(npos);
        }

        public static int Puzzle13Part1()
        {
            var lines = "Year2020\\Data\\Day13.txt".ReadAll<string>();
            var startTime = int.Parse(lines[0]);
            var buses = lines[1].Split(",").Where(b => b != "x").Select(b => int.Parse(b)).ToList();
            var list = new List<(int id, int arrivalTime)>();
            foreach (var bus in buses)
            {
                var arrival = 0;
                while (arrival < startTime)
                    arrival += bus;
                list.Add((bus, arrival));
            }


            var min = list.Min(l => l.arrivalTime);
            var m = list.FirstOrDefault(m => m.arrivalTime == min);
            return (m.arrivalTime - startTime) * m.id;
        }

        public static long Puzzle13Part2()
        {
            var lines = "Year2020\\Data\\Day13.txt".ReadAll<string>();
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

        public static long Puzzle14Part1()
        {
            var kvps = "Year2020\\Data\\Day14.txt".ReadAllKeyValuePairs<string, string>(" = ");

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
                                    var v = (long) 1 << (36 - index - 1);
                                    val |= v;
                                    break;
                                case '0':
                                    val &= ~((long) 1 << (36 - index - 1));
                                    break;
                            }
                        }

                        memory[loc] = Maths.ApplyMask(mask, val);

                        break;
                }

            return memory.Sum();
        }


        public static long Puzzle14Part2()
        {
            var kvps = "Year2020\\Data\\Day14.txt".ReadAllKeyValuePairs<string, string>(" = ");

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
                                    var v = (long) 1 << (36 - index - 1);
                                    loc |= v;
                                    break;
                                }
                            }
                        }

                        var locs = new List<long> {loc};
                        for (var index = 0; index < mask.Length; index++)
                            if (mask[index] == 'X')
                            {
                                var ls = locs.ToList();
                                locs = new List<long>();
                                foreach (var t in ls)
                                {
                                    locs.Add(t & ~((long) 1 << (36 - index - 1)));
                                    locs.Add(t | ((long) 1 << (36 - index - 1)));
                                }
                            }

                        foreach (var l in locs)
                            memory[l] = val;

                        break;
                }

            return memory.Values.Sum();
        }

        public static int Puzzle15Part1()
        {
            var input = new List<int> {6, 4, 12, 1, 20, 0, 16};

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

        public static int Puzzle15Part2()
        {
            var input = new List<int> {6, 4, 12, 1, 20, 0, 16};

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

        public static int Puzzle16Part1()
        {
            var lines = "Year2020\\Data\\Day16.txt".ReadAll<string>().ToList();
            var rules = EnumerableExtensions.TakeWhile(lines, s => !string.IsNullOrEmpty(s)).ToList();
            var nearbyTickets = lines.Skip(rules.Count + 6).Select(s => s.SplitToType<int>(",")).ToList();

            Dictionary<string, List<(int min, int max)>> dict = rules.ToDictionary(s => s.Split(":")[0], s =>
            {
                return s.Split(": ")[1].Split(" or ")
                    .Select(b =>
                    {
                        var bits2 = b.Split("-");
                        return (int.Parse(bits2[0]), int.Parse(bits2[1]));
                    }).ToList();
            });
            return nearbyTickets.SelectMany(nt =>
                nt.Where(n => dict.All(r => r.Value.All(range => n < range.min || n > range.max)))).Sum();
        }

        public static long Puzzle16Part2()
        {
            var lines = "Year2020\\Data\\Day16.txt".ReadAll<string>().ToList();
            var rules = EnumerableExtensions.TakeWhile(lines, s => !string.IsNullOrEmpty(s)).ToList();
            var myTicket = lines[rules.Count + 2].SplitToType<int>(",").ToList();
            var nearbyTickets = lines.Skip(rules.Count + 5).Select(s => s.SplitToType<int>(",").ToList()).ToList();
            nearbyTickets.Insert(0, myTicket);
            Dictionary<string, List<(int min, int max)>> dict = rules.ToDictionary(s => s.Split(":")[0], s =>
            {
                return s.Split(": ")[1].Split(" or ")
                    .Select(b =>
                    {
                        var bits2 = b.SplitToType<int>("-").ToList();
                        return (bits2[0], bits2[1]);
                    }).ToList();
            });
            var validNearbyTickets = nearbyTickets.Where(nt =>
                nt.All(n => dict.Any(r => r.Value.Any(range => n.IsInRange(range))))).ToList();
            var possibleFields = new List<List<string>>();
            for (var i = 0; i < rules.Count; i++)
            {
                var fields = validNearbyTickets.Select(v => v[i]).ToList();
                fields.Sort();
                possibleFields.Add(dict.Where(d =>
                        fields.All(f => d.Value.Any(range => f.IsInRange(range)))).Select(d => d.Key)
                    .ToList());
            }

            var orderedFields = new Dictionary<int, string>();
            for (var i = 0; i < rules.Count; i++)
            {
                var pf = possibleFields.First(p => p.Count == i + 1);
                orderedFields.Add(possibleFields.IndexOf(pf), pf.Except(orderedFields.Values).First());
            }

            long sum = 1;
            for (var i = 0; i < orderedFields.Count; i++)
                if (orderedFields[i].StartsWith("departure"))
                    sum *= myTicket[i];

            return sum;
        }


        public static int Puzzle17Part1()
        {
            var lines = "Year2020\\Data\\Day17.txt".ReadAll<string>().ToList();
            var array = new HashSet<(int x, int y, int z)>();
            for (var y = lines.Count - 1; y >= 0; y--)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                    if (line[x] == '#')
                        array.Add((x, y, 0));
            }

            
            //Console.WriteLine($"======INITIAL======");
            //Dump(array);
            for (var cycle = 1; cycle <= 6; cycle++)
            {
                var newArray = new HashSet<(int x, int y, int z)>();
                //Console.WriteLine($"======CYCLE={cycle}======");
                var xmax = array.Max(t => t.x) +1;
                var ymax = array.Max(t => t.y) +1;
                var zmax = array.Max(t => t.z) +1;
                for (var x = array.Min(t => t.x) -1; x <= xmax ; x++)
                for (var y = array.Min(t => t.y) -1; y <= ymax ; y++)
                for (var z = array.Min(t => t.z) -1; z <= zmax ; z++)
                {
                    var neighbours = BoardExtensions.GetNeighbours(x,y,z);
                    var activeCount = neighbours.Count(n => array.Contains((n.x, n.y, n.z)));
                    if (array.Contains((x, y, z)) && (activeCount == 2 || activeCount == 3))
                        newArray.Add((x,y,z));
                    else if (!array.Contains((x,y,z)) &&  activeCount == 3)
                        newArray.Add((x,y,z));
                }

                array = newArray;
                
                //Dump(array);
                //Console.WriteLine($"On={array.Count}");
            }
            return array.Count;
        }
        

        private static void Dump(HashSet<(int x, int y, int z)> array)
        {
            for (var z = array.Min(t => t.z); z <= array.Max(t => t.z); z++)
            {
                Console.WriteLine($"z = {z}");
                for (var y = array.Min(t => t.y); y <= array.Max(t => t.y); y++)
                {
                    for (var x = array.Min(t => t.x); x <= array.Max(t => t.x); x++)
                        Console.Write(array.Contains((x,y,z))  ? '#' : '.');
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        private static void Dump2(HashSet<(int x, int y, int z, int w)> array)
        {
            for (var w = array.Min(t => t.w); w <= array.Max(t => t.w); w++)
            for (var z = array.Min(t => t.z); z <= array.Max(t => t.z); z++)
            {
                Console.WriteLine($"z = {z}, w={w}");
                for (var y = array.Min(t => t.y); y <= array.Max(t => t.y); y++)
                {
                    for (var x = array.Min(t => t.x); x <= array.Max(t => t.x); x++)
                        Console.Write(array.Contains((x,y,z,w))  ? '#' : '.');
                    Console.WriteLine();
                }
                
                Console.WriteLine();
                
            }

        }
    

        public static int Puzzle17Part2()
        {
            var lines = "Year2020\\Data\\Day17.txt".ReadAll<string>().ToList();
            var array = new HashSet<(int x, int y, int z, int w)>();
            for (var y = lines.Count - 1; y >= 0; y--)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                    if (line[x] == '#')
                        array.Add((x, y, 0, 0));
            }

           
           // Console.WriteLine($"======INITIAL======");
           // Dump2(array);
            for (var cycle = 1; cycle <= 6; cycle++)
            {
                var newArray = new HashSet<(int x, int y, int z, int w)>();
                //Console.WriteLine($"======CYCLE={cycle}======");
                var xmin = array.Min(t => t.x) -1;
                var xmax = array.Max(t => t.x) +1;
                var ymin = array.Min(t => t.y) -1;
                var ymax = array.Max(t => t.y) +1;
                var zmin = array.Min(t => t.z) -1;
                var zmax = array.Max(t => t.z) +1;
                var wmin = array.Min(t => t.w) -1;
                var wmax = array.Max(t => t.w) +1;
                for (var x = xmin; x <= xmax ; x++)
                for (var y = ymin; y <= ymax ; y++)
                for (var z = zmin; z <= zmax ; z++)
                for (var w = wmin; w <= wmax ; w++)
                {
                    var neighbours = BoardExtensions.GetNeighbours(x,y,z,w);
                    var activeCount = neighbours.Count(n => array.Contains((n.x, n.y, n.z, n.w)));
                    var con = array.Contains((x, y, z, w));
                    if (con && (activeCount == 2 || activeCount == 3))
                        newArray.Add((x,y,z,w));
                    else if (!con &&  activeCount == 3)
                        newArray.Add((x,y,z,w));
                }

                array = newArray;
                
                //Dump2(array);
                //Console.WriteLine($"On={array.Count}");
            }
            return array.Count;
        }

        public static long Puzzle18Part1()
        {
            
            var sums = "Year2020\\Data\\Day18.txt".ReadAll<string>().ToList();
            long ret = 0;
            int i ;
            long Evaluate(string s)
            {
                
                long total = 0;
                var add = -1;
                for (; i < s.Length; i++)
                {
                    var c = s[i];
                    if (char.IsWhiteSpace(c)) continue;
                    if (char.IsDigit(c) && add == -1) total = int.Parse(c.ToString());
                    else if (char.IsDigit(c) && add > -1)
                    {
                        if (add == 0)
                            total += int.Parse(c.ToString());
                        else total *= int.Parse(c.ToString());
                    }
                    else if (c == '+') add = 0;
                    else if (c == '*') add = 1;
                    else if (c == '(')
                    {
                        switch (add)
                        {
                            case -1 :
                                i++;
                                total = Evaluate(s);
                                break;
                            case 0:
                                i++;
                                total += Evaluate(s);
                                break;
                            case 1:
                                i++;
                                total *= Evaluate(s);
                                break;
                                
                        }
                    }
                    else if (c == ')')
                    {
                        return total;
                    }
                }

                return total;
            }
            foreach (var s in sums)
            {
                i = 0;
                var t = Evaluate(s);
                //Console.WriteLine(t);
                ret += t;
            }
            return ret;
        }

        public static long Puzzle18Part2()
        {
            var sums = "Year2020\\Data\\Day18.txt".ReadAll<string>().ToList();

            static string ReplaceAdds(string s1)
            {
                return Regex.Replace(s1, @"[0-9]+( \+ [0-9]+)+", 
                    match => match.Value.SplitToType<long>("+").Sum().ToString() );
            }

            static long EvaluateRegex(string s1)
            {
                var s2 = s1;
                while (s2.Contains('('))
                {
                    s2 = Regex.Replace(s2, @"\([0-9 +*]+\)",
                        match => EvaluateNoBrackets(match.Value.TrimStart('(').TrimEnd(')')).ToString());
                }

                return EvaluateNoBrackets(s2);
            }

            static long EvaluateNoBrackets(string s) => ReplaceAdds(s).SplitToType<long>(" * ").Product();

            return sums.Sum(EvaluateRegex);
        }

       private static  List<string> Evaluate( int rule, IReadOnlyDictionary<int, string> rules)
        {
            var r = rules[rule];
            if (r.StartsWith("\""))
                return new List<string>{ $"{r.RemoveQuotes()}"};
            return r.Split('|').SelectMany(bit => bit.Trim()
                    .SplitToType<int>(" ")
                    .Select(i => Evaluate(i, rules))
                    .Aggregate(new List<string>(), (current, l) => current.Count == 0 ? l : Append(current.ToList(), l))).ToList();
        }

        public static int Puzzle19Part1()
        {
            var rules = "Year2020\\Data\\Day19.txt".ReadAllKeyValuePairs<int,string>(": ").ToDictionary(d => d.key, d => d.value);
            var strings = "Year2020\\Data\\Day19a.txt".ReadAll<string>().ToList();

            return strings.Count(s => Evaluate(0, rules).Contains(s));
        }
        private static List<string> Append(List<string> l1, List<string> l2)
        {
            return (from s1 in l1 from s2 in l2 select $"{s1}{s2}").ToList();
        }
        public static int Puzzle19Part2()
        {
            var rules = "Year2020\\Data\\Day19.txt".ReadAllKeyValuePairs<int,string>(": ").ToDictionary(d => d.key, d => d.value);
            var strings = "Year2020\\Data\\Day19a.txt".ReadAll<string>().ToList();

            var ret42 = Evaluate(42, rules);
            var ret31 = Evaluate(31, rules);
            
            return strings.Count(s =>
            {
                var temp = s;
                int count42 = 0;
                while(temp.Length > 0)
                {
                    
                    var s42 = ret42.FirstOrDefault(temp.StartsWith);
                    if (s42 == null) break;
                    temp = temp[s42.Length..];
                    count42++;
                    if (count42 <= 1) continue;
                    var count31 = 0;
                    bool ok = true;
                    while(temp.Length > 0)
                    {
                        var s31 = ret31.FirstOrDefault(temp.EndsWith);
                        if (s31 == null)
                        {
                            ok = false;
                            break;
                        }
                        temp = temp.Substring(0,temp.Length - s31.Length);
                        count31++;

                    }

                    if (count31 == 0)
                        return false;
                    if (ok && count31 > 0 && count31 < count42 && temp.Length == 0)
                    {
                        // Console.WriteLine(s);
                        return true;
                    }
                }

                return false;
            });
        }

        public static Dictionary<string, Board> Puzzle20LoadData()
        {
            var strings = "Year2020\\Data\\Day20.txt".ReadAll<string>();
            var dict = new Dictionary<string, Board>();
            for (int i = 0; i < strings.Length; i++)
            {
                string s = strings[i];
                i++;
                var b = new Board(10,10);
                b.LoadFromStrings(strings.Skip(i).Take(10));
                i += 10;
                dict.Add(s, b);
            }

            return dict;
        } 
        public static long Puzzle20Part1()
        {
            var dict1 = Puzzle20LoadData();
            var size = (int)Math.Sqrt(dict1.Count);

            List<Board> GetAllBoards(Board b)
            {
                var ret = new List<Board>();
                ret.Add(b);
                ret.Add(b.RotateACW());
                ret.Add( ret[1].RotateACW());
                ret.Add( ret[2].RotateACW());
                ret.Add( ret[3].RotateACW());
                ret.Add( ret[0].FlipX());
                ret.Add( ret[0].FlipY());
                ret.Add( ret[1].FlipX());
                ret.Add( ret[1].FlipY());
                ret.Add( ret[2].FlipX());
                ret.Add( ret[2].FlipY());
                ret.Add( ret[3].FlipX());
                ret.Add( ret[3].FlipY());
                return ret;
            }

            var boards = new Dictionary<string, List<Board>>();

            bool MatchRight(Board b1, Board b2)
            {
                return b1.RightEdgeHash() == b2.LeftEdgeHash();
            }
            bool MatchLeft(Board b1, Board b2)
            {
                return b2.RightEdgeHash() == b1.LeftEdgeHash();
            }
            bool MatchTop(Board b1, Board b2)
            {
                return b1.TopEdgeHash() == b2.BottomEdgeHash();
            }
            bool MatchBottom(Board b1, Board b2)
            {
                return b2.TopEdgeHash() == b1.BottomEdgeHash();
            }
            
            var correctBoards = new Dictionary<string, Board>();
            bool MatchBoards(string b1Key, string b2Key,Board b1,Board b2)
            {
                b1.Dump();
                b2.Dump();
                if (MatchRight(b1, b2))
                {
                    correctBoards[b1Key] = b1;
                    correctBoards[b2Key] = b2;
                    
                    if (!b1.BoardsRight.Contains((b2Key,b2)))
                        b1.BoardsRight.Add( (b2Key,b2));
                    if (!b2.BoardsLeft.Contains((b1Key,b1)))
                        b2.BoardsLeft.Add((b1Key,b1));
                    return true;
                }

                if (MatchLeft(b1, b2))
                {
                    correctBoards[b1Key] = b1;
                    correctBoards[b2Key] = b2;
                    
                    if (!b1.BoardsLeft.Contains((b2Key,b2)))
                        b1.BoardsLeft.Add( (b2Key,b2));
                    if (!b2.BoardsRight.Contains((b1Key,b1)))
                        b2.BoardsRight.Add((b1Key,b1));
                    return true;
                }
                if (MatchTop(b1, b2))
                {
                    correctBoards[b1Key] = b1;
                    correctBoards[b2Key] = b2;
                    if (!b1.BoardsTop.Contains((b2Key,b2)))
                        b1.BoardsTop.Add( (b2Key,b2));
                    if (!b2.BoardsBottom.Contains((b1Key,b1)))
                        b2.BoardsBottom .Add((b1Key,b1));
                    return true;
                }
                if (MatchBottom(b1, b2))
                {
                    correctBoards[b1Key] = b1;
                    correctBoards[b2Key] = b2;
                    if (!b1.BoardsBottom.Contains((b2Key,b2)))
                        b1.BoardsBottom.Add( (b2Key,b2));
                    if (!b2.BoardsTop.Contains((b1Key,b1)))
                        b2.BoardsTop .Add((b1Key,b1));
                    return true;
                }

                return false;
            }
            foreach (var kvp in dict1)
                boards.Add(kvp.Key, GetAllBoards(kvp.Value));
            foreach (var board1 in boards)
            foreach (var board2 in boards)
            {
                if (board1.Key == board2.Key ) continue;
                if (correctBoards.ContainsKey(board1.Key))
                {
                    if (correctBoards.ContainsKey(board2.Key)) 
                        if (MatchBoards(board1.Key , board2.Key, correctBoards[board1.Key], correctBoards[board2.Key]))
                        {
                            //Console.WriteLine($"{board1.Key} matches {board2.Key} using b1 ");
                            continue;
                        }
                    for (var index = 0; index < board2.Value.Count; index++)
                    {
                        var board2Board = board2.Value[index];
                        if (MatchBoards(board1.Key, board2.Key, correctBoards[board1.Key], board2Board))
                        {
                            //board2Board.Dump();
                            //Console.WriteLine($"{board1.Key} matches {board2.Key} using b1 {index}");
                        }
                    }

                    continue;
                }

                if (correctBoards.ContainsKey(board2.Key))
                {
                    for (var index = 0; index < board1.Value.Count; index++)
                    {
                        var board1Board = board1.Value[index];
                        if (MatchBoards(board1.Key, board2.Key, board1Board, correctBoards[board2.Key]))
                        {
                            //board1Board.Dump();
                            correctBoards[board2.Key].Dump();
                           // Console.WriteLine($"{board1.Key} matches {board2.Key} using b1 {index}");
                            break;
                        }
                    }

                    continue;
                }

                for (var i = 0; i < board1.Value.Count; i++)
                {
                    var board1Board = board1.Value[i];
                    bool found = false;
                    for (var index = 0; index < board2.Value.Count; index++)
                    {
                        var board2Board = board2.Value[index];
                        if (MatchBoards(board1.Key, board2.Key, board1Board, board2Board))
                        {
                            //board1Board.Dump();
                            //board2Board.Dump();
                            //Console.WriteLine($"{board1.Key} matches {board2.Key} using b1 {i} and b2 {index}");
                            found = true;
                            break;
                        }
                    }

                    if (found) break;
                }
            }

            
            var tls = correctBoards.Where(v =>
                 v.Value.BoardsRight.Count> 0 && v.Value.BoardsBottom.Count > 0);
            var lefts = correctBoards.Where(v =>
                 v.Value.BoardsRight.Count> 0 && v.Value.BoardsBottom.Count > 0 && v.Value.BoardsTop.Count > 0);
            var bls = correctBoards.Where(v =>
                 v.Value.BoardsRight.Count> 0  && v.Value.BoardsTop.Count > 0);
            

            List<List< (string id, Board board)>> GetLineOptions( (string id, Board board) board, int level)
            {
                if (level == size -1)
                    return board.board.BoardsRight.Select(br => new List<(string id, Board board)>{br}).ToList();
                var options = new List<List<(string id, Board board)>>();
               
                foreach (var b in board.board.BoardsRight)
                {
                    
                    var opts = GetLineOptions(b, level + 1);
                    foreach (var o in opts)
                    {
                        if (o.Contains(b)) continue;
                        o.Insert(0,b);
                        options.Add(o);
                    }
                }

                return options;

            }

            
            List<(string id, Board Board)[,]> arrayOptions = new List<(string id, Board Board)[,]>();
            var tlopts = new List<List<(string id, Board board)>>();
            var leftopts = new List<List<(string id, Board board)>>();
            var blopts = new List<List<(string id, Board board)>>();
            foreach (var tl in tls)
            {
                var options = GetLineOptions((tl.Key, tl.Value), 1);
                foreach (var o in options)
                {
                    o.Insert(0,(tl.Key, tl.Value));
                    
                }
                tlopts.AddRange(options);
              
            }
            foreach (var bl in bls)
            {
                var options = GetLineOptions((bl.Key, bl.Value), 1);
                foreach(var o in options)
                    o.Insert(0,(bl.Key, bl.Value));
                blopts.AddRange(options);
              
            }
            foreach (var l in lefts)
            {
                var options = GetLineOptions((l.Key, l.Value), 1);
                foreach(var o in options)
                    o.Insert(0,(l.Key, l.Value));
                leftopts.AddRange(options);
            }
            foreach (var opt in tlopts)
            {
                arrayOptions.Add(new (string id, Board Board)[size,size]);
                for (int i = 0; i < size; i++)
                    arrayOptions.Last()[i, 0] = opt[i];
            }
             bool CanFit(List<(string id, Board board)> leftopt, (string id, Board Board)[,] ao, in int y)
            {
                for (int x = 0; x < size; x++)
                for (int y1 = 0; y1 < y; y1++)
                {
                    if (leftopt.Contains(ao[x, y1])) return false;
                }
                
                for (int i = 0; i < leftopt.Count; i++)
                    if (!leftopt[i].board.BoardsTop.Contains(ao[i, y]))
                        return false;

                return true;
            }

             List<List<(string id, Board board)>> appropriateLefts = null;
            for (int y = 1; y < size -1; y++)
            {
                var arrOpsNew = new List<(string id, Board Board)[,]>();
                for (int x = 0; x < size; x++)
                {
                    var allAbove = arrayOptions.Select(a => a[x, y - 1]).Distinct().ToList();
                    appropriateLefts =
                        appropriateLefts?.Where(l =>
                        {
                            return  
                                   allAbove.Any(t => t.Board.BoardsBottom.Contains(l[x]));
                        }).ToList() ??
                        leftopts.Where(l => allAbove.Any(t => t.Board.BoardsBottom.Contains(l[x]))).ToList();
                    
                }

                var ls = appropriateLefts.ToList();
                for (var index = 0; index < ls.Count; index++)
                {
                    var t = ls[index];
                    foreach (var ao in arrayOptions)
                    {
                        if (CanFit(t, ao, y - 1))
                        {
                            var newAo = new (string id, Board Board)[size, size];
                            for (int x = 0; x < size; x++)
                            for (int y1 = 0; y1 < size; y1++)
                            {
                                newAo[x, y1] = ao[x, y1];
                            }

                            for (int j = 0; j < t.Count; j++)
                            {
                                newAo[j, y] = t[j];
                            }

                            arrOpsNew.Add(newAo);
                        }
                    }
                }

                arrayOptions = arrOpsNew;
            }
            var arrOpsNew2= new List<(string id, Board Board)[,]>();
            for (var i = 0; i < blopts.Count; i++)
            {
                foreach (var ao in arrayOptions)
                {
                    var newAo = new (string id, Board Board)[size, size];
                    for (int x = 0; x < size; x++)
                    for (int y1 = 0; y1 < size; y1++)
                    {
                        newAo [x,y1] = ao[x, y1];
                    }
                    if (CanFit(blopts[i], newAo, size -2))
                    {
                        for (int j = 0; j < blopts[i].Count; j++)
                            newAo[j, size - 1] = blopts[i][j];
                        arrOpsNew2.Add(newAo);
                    }
                }

            }

            arrayOptions = arrOpsNew2;
            var first = arrayOptions[0];
            for (int y = 0; y < first.GetLength(1) * 10; y++)
            {
                for (int x = 0; x < first.GetLength(0)*10; x++)

                {
                    var board = first[x/10, y/10];
                    Console.Write(board.Board.ValueAt(x%10, y%10));
                    if (x % 10 == 9)
                        Console.Write(" ");
                }
                Console.WriteLine();
                if (y % 10 == 9)
                    Console.WriteLine();
            }
            for (int y = 0; y < first.GetLength(1); y++)
            {
                for (int x = 0; x < first.GetLength(0) ; x++)
                {
                    Console.Write(first[x,y].id);
                    Console.Write(" ");
                }
                Console.WriteLine();

            }
            var num =
                long.Parse(arrayOptions[0][0, 0].id.Substring(5, 4)) *
                long.Parse(arrayOptions[0][0, size -1].id.Substring(5, 4)) *
                long.Parse(arrayOptions[0][size -1, 0].id.Substring(5, 4)) *
                long.Parse(arrayOptions[0][size -1, size -1].id.Substring(5, 4));
            return num;
        }

        

        public static int Puzzle20Part2()
        {
            
            return int.MaxValue;
        }

        public static int Puzzle21Part1()
        {
            var lines = "Year2020\\Data\\Day21.txt".ReadAll<string>().ToList();
            var lines2 = new List<(List<string> ingredients, List<string> allergens)>();
            foreach (var line in lines)
            {
                var split = line.Split("(contains ");
                lines2.Add((split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(),split[1].TrimEnd(')').Split(", ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()));
            }
            var allIngredients = lines2.SelectMany(f => f.ingredients).Distinct();
            var allAllergens = lines2.SelectMany(l => l.allergens).Distinct().ToDictionary(d => d, d => new List<string>());
            foreach (var allergen in allAllergens.Keys.ToList())
            {
                var foods = lines2.Where(l => l.allergens.Contains(allergen)).ToList();
                var commonIngredients = foods
                    .SelectMany(f => f.ingredients).Distinct().Where(f => foods.All(f2 => f2.ingredients.Contains(f)));
                allAllergens[allergen] = commonIngredients.ToList();

            }

            var dictionary = new Dictionary<string, string>();
            while (allAllergens.Any())
            {
                foreach (var allAllergen in allAllergens.Keys.ToList())
                {
                    var unknown = allAllergens[allAllergen].Except(dictionary.Values).ToList();
                    if (unknown.Count == 1)
                    {
                        dictionary[allAllergen] = unknown.First();
                        allAllergens.Remove(allAllergen);
                    }
                }
            }

            var noAllergens = allIngredients.Except(dictionary.Values).ToList();
            return noAllergens.Sum(a => lines2.Sum(l2 => l2.ingredients.Count(i => i == a)));
        }

        public static string Puzzle21Part2()
        {
            var lines = "Year2020\\Data\\Day21.txt".ReadAll<string>().ToList();
            var lines2 = new List<(List<string> ingredients, List<string> allergens)>();
            foreach (var line in lines)
            {
                var split = line.Split("(contains ");
                lines2.Add((split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(),split[1].TrimEnd(')').Split(", ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()));
            }
            var allAllergens = lines2.SelectMany(l => l.allergens).Distinct().ToDictionary(d => d, d => new List<string>());
            foreach (var allergen in allAllergens.Keys.ToList())
            {
                var foods = lines2.Where(l => l.allergens.Contains(allergen)).ToList();
                var commonIngredients = foods
                    .SelectMany(f => f.ingredients).Distinct().Where(f => foods.All(f2 => f2.ingredients.Contains(f)));
                allAllergens[allergen] = commonIngredients.ToList();

            }

            var dictionary = new Dictionary<string, string>();
            while (allAllergens.Any())
            {
                foreach (var allAllergen in allAllergens.Keys.ToList())
                {
                    var unknown = allAllergens[allAllergen].Except(dictionary.Values).ToList();
                    if (unknown.Count == 1)
                    {
                        dictionary[allAllergen] = unknown.First();
                        allAllergens.Remove(allAllergen);
                    }
                }
            }

            var allergens = dictionary.Keys.ToList();
            allergens.Sort();
            var ings = allergens.Select(a => dictionary[a]).ToList();
            return String.Join(",", ings);
        }

        public static int Puzzle22Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle22Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle23Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle23Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle24Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle24Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle25Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle25Part2()
        {
            return int.MaxValue;
        }

        #endregion
    }

    public static class Computer
    {
        #region Fields

        public const int InfiniteLoopDetected = 1;

        #endregion

        #region Methods

        public static (int acc, int err) RunProgram(List<(string key, string value)> kvps)
        {
            var acc = 0;
            var ind = 0;
            var visited = new HashSet<int>();
            while (true)
            {
                if (ind >= kvps.Count) return (acc, 0);
                if (visited.Contains(ind))
                    return (acc, InfiniteLoopDetected);
                visited.Add(ind);
                switch (kvps[ind].key)
                {
                    case "acc":
                        acc += int.Parse(kvps[ind].value);
                        ind++;
                        break;
                    case "jmp":
                        ind += int.Parse(kvps[ind].value);
                        break;
                    case "nop":
                        ind++;
                        break;
                }
            }
        }

        #endregion
    }
}