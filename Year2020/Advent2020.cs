using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Functions;
// ReSharper disable UnusedMember.Global

namespace AdventOfCode.Year2020
{
    public static class Advent2020
    {
        #region Types

        private class Board2 : IEquatable<Board2>
        {
            #region Constructors

            public Board2(int leftHash, int rightHash, int bottomHash, int topHash, string id, Board board)
            {
                LeftEdgeHash = leftHash;
                RightEdgeHash = rightHash;
                TopEdgeHash = topHash;
                BottomEdgeHash = bottomHash;
                ID = id;
                Board = board;
            }

            #endregion

            #region Properties

            public int LeftEdgeHash { get; }
            public int RightEdgeHash { get; }
            public int TopEdgeHash { get; }
            public int BottomEdgeHash { get; }
            public string ID { get; }
            public Board Board { get; }

            #endregion

            #region Methods

            public override string ToString()
            {
                return $"{ID} {GetHashCode()}";
            }

            public bool Equals(Board2 other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return LeftEdgeHash == other.LeftEdgeHash && RightEdgeHash == other.RightEdgeHash &&
                       TopEdgeHash == other.TopEdgeHash && BottomEdgeHash == other.BottomEdgeHash;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Board2) obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(LeftEdgeHash, RightEdgeHash, TopEdgeHash, BottomEdgeHash);
            }

            #endregion
        }

        #endregion

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
            return hashset.Count;
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
            kvps.Add(kvps[^1] + 3);

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
            var rules = lines.TakeWhile( s => !string.IsNullOrEmpty(s)).ToList();
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
            var rules = lines.TakeWhile( s => !string.IsNullOrEmpty(s)).ToList();
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
                var xmax = array.Max(t => t.x) + 1;
                var ymax = array.Max(t => t.y) + 1;
                var zmax = array.Max(t => t.z) + 1;
                for (var x = array.Min(t => t.x) - 1; x <= xmax; x++)
                for (var y = array.Min(t => t.y) - 1; y <= ymax; y++)
                for (var z = array.Min(t => t.z) - 1; z <= zmax; z++)
                {
                    var neighbours = BoardExtensions.GetNeighbours(x, y, z);
                    var activeCount = neighbours.Count(n => array.Contains((n.x, n.y, n.z)));
                    if (array.Contains((x, y, z)) && (activeCount == 2 || activeCount == 3))
                        newArray.Add((x, y, z));
                    else if (!array.Contains((x, y, z)) && activeCount == 3)
                        newArray.Add((x, y, z));
                }

                array = newArray;

                //Dump(array);
                //Console.WriteLine($"On={array.Count}");
            }

            return array.Count;
        }


        // private static void Dump(HashSet<(int x, int y, int z)> array)
        // {
        //     for (var z = array.Min(t => t.z); z <= array.Max(t => t.z); z++)
        //     {
        //         Console.WriteLine($"z = {z}");
        //         for (var y = array.Min(t => t.y); y <= array.Max(t => t.y); y++)
        //         {
        //             for (var x = array.Min(t => t.x); x <= array.Max(t => t.x); x++)
        //                 Console.Write(array.Contains((x, y, z)) ? '#' : '.');
        //             Console.WriteLine();
        //         }
        //
        //         Console.WriteLine();
        //     }
        // }

        // private static void Dump2(HashSet<(int x, int y, int z, int w)> array)
        // {
        //     for (var w = array.Min(t => t.w); w <= array.Max(t => t.w); w++)
        //     for (var z = array.Min(t => t.z); z <= array.Max(t => t.z); z++)
        //     {
        //         Console.WriteLine($"z = {z}, w={w}");
        //         for (var y = array.Min(t => t.y); y <= array.Max(t => t.y); y++)
        //         {
        //             for (var x = array.Min(t => t.x); x <= array.Max(t => t.x); x++)
        //                 Console.Write(array.Contains((x, y, z, w)) ? '#' : '.');
        //             Console.WriteLine();
        //         }
        //
        //         Console.WriteLine();
        //     }
        // }


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
                var xmin = array.Min(t => t.x) - 1;
                var xmax = array.Max(t => t.x) + 1;
                var ymin = array.Min(t => t.y) - 1;
                var ymax = array.Max(t => t.y) + 1;
                var zmin = array.Min(t => t.z) - 1;
                var zmax = array.Max(t => t.z) + 1;
                var wmin = array.Min(t => t.w) - 1;
                var wmax = array.Max(t => t.w) + 1;
                for (var x = xmin; x <= xmax; x++)
                for (var y = ymin; y <= ymax; y++)
                for (var z = zmin; z <= zmax; z++)
                for (var w = wmin; w <= wmax; w++)
                {
                    var neighbours = BoardExtensions.GetNeighbours(x, y, z, w);
                    var activeCount = neighbours.Count(n => array.Contains((n.x, n.y, n.z, n.w)));
                    var con = array.Contains((x, y, z, w));
                    if (con && (activeCount == 2 || activeCount == 3))
                        newArray.Add((x, y, z, w));
                    else if (!con && activeCount == 3)
                        newArray.Add((x, y, z, w));
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
            int i;

            long DoEvaluate(string s)
            {
                long total = 0;
                var add = -1;
                for (; i < s.Length; i++)
                {
                    var c = s[i];
                    if (char.IsWhiteSpace(c)) continue;
                    if (char.IsDigit(c) && add == -1)
                    {
                        total = int.Parse(c.ToString());
                    }
                    else if (char.IsDigit(c) && add > -1)
                    {
                        if (add == 0)
                            total += int.Parse(c.ToString());
                        else total *= int.Parse(c.ToString());
                    }
                    else if (c == '+')
                    {
                        add = 0;
                    }
                    else if (c == '*')
                    {
                        add = 1;
                    }
                    else if (c == '(')
                    {
                        switch (add)
                        {
                            case -1:
                                i++;
                                total = DoEvaluate(s);
                                break;
                            case 0:
                                i++;
                                total += DoEvaluate(s);
                                break;
                            case 1:
                                i++;
                                total *= DoEvaluate(s);
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
                var t = DoEvaluate(s);
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
                    match => match.Value.SplitToType<long>("+").Sum().ToString());
            }

            static long EvaluateRegex(string s1)
            {
                var s2 = s1;
                while (s2.Contains('('))
                    s2 = Regex.Replace(s2, @"\([0-9 +*]+\)",
                        match => EvaluateNoBrackets(match.Value.TrimStart('(').TrimEnd(')')).ToString());

                return EvaluateNoBrackets(s2);
            }

            static long EvaluateNoBrackets(string s)
            {
                return ReplaceAdds(s).SplitToType<long>(" * ").Product();
            }

            return sums.Sum(EvaluateRegex);
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

        public static int Puzzle19Part1()
        {
            return -1;
            var rules = "Year2020\\Data\\Day19.txt".ReadAllKeyValuePairs<int, string>(": ")
                .ToDictionary(d => d.key, d => d.value);
            var strings = "Year2020\\Data\\Day19a.txt".ReadAll<string>().ToList();

            return strings.Count(s => Evaluate(0, rules).Contains(s));
        }

        private static List<string> Append(List<string> l1, List<string> l2)
        {
            return (from s1 in l1 from s2 in l2 select $"{s1}{s2}").ToList();
        }

        public static int Puzzle19Part2()
        {
            return -1;
            var rules = "Year2020\\Data\\Day19.txt".ReadAllKeyValuePairs<int, string>(": ")
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

        private static Dictionary<string, Board> Puzzle20LoadData()
        {
            var strings = "Year2020\\Data\\Day20.txt".ReadAll<string>();
            var dict = new Dictionary<string, Board>();
            for (var i = 0; i < strings.Length; i++)
            {
                var s = strings[i];
                i++;
                var b = new Board(10, 10);
                b.LoadFromStrings(strings.Skip(i).Take(10));
                i += 10;
                dict.Add(s, b);
            }

            return dict;
        }

        private static List<Board> GetAllBoards(Board b)
        {
            var ret = new List<Board> {b, b.RotateACW()};
            ret.Add(ret[1].RotateACW());
            ret.Add(ret[2].RotateACW());
            ret.Add(ret[0].FlipX());
            ret.Add(ret[0].FlipY());
            ret.Add(ret[1].FlipX());
            ret.Add(ret[1].FlipY());
            ret.Add(ret[2].FlipX());
            ret.Add(ret[2].FlipY());
            ret.Add(ret[3].FlipX());
            ret.Add(ret[3].FlipY());
            return ret;
        }

        public static long Puzzle20Part1()
        {
            var dict1 = Puzzle20LoadData();
            var size = (int) Math.Sqrt(dict1.Count);

            var allBoards = dict1.SelectMany(kvp => GetAllBoards(kvp.Value)
                .Select(b => new Board2(b.LeftEdgeHash(), b.RightEdgeHash(), b.BottomEdgeHash(),
                    b.TopEdgeHash(), kvp.Key, b)).Distinct()).ToList();

            List<Board2[,]> FillNextSquare(List<Board2[,]> options, int x, int y)
            {
                while (true)
                {
                    var ret = new List<Board2[,]>();
                    foreach (var o in options)
                    {
                        foreach (var board2 in allBoards.Where(b => o.Cast<Board2>().All(b2 => b2 == null || b.ID != b2.ID)))
                            if ((x == 0 || board2.LeftEdgeHash == o[x - 1, y].RightEdgeHash) && (y == 0 || board2.TopEdgeHash == o[x, y - 1].BottomEdgeHash))
                            {
                                var newO = new Board2[size, size];
                                for (var i = 0; i < size; i++)
                                for (var j = 0; j < size; j++)
                                    newO[i, j] = o[i, j];
                                newO[x, y] = board2;
                                ret.Add(newO);
                            }
                    }

                    Console.WriteLine(ret.Count);
                    if (x + 1 == size)
                    {
                        x = 0;
                        y++;
                    }
                    else
                    {
                        x++;
                    }

                    if (y == size) return ret;
                    options = ret;
                }
                
            }


            var arrayOptions = FillNextSquare(allBoards.Select(b =>
            {
                var ret = new Board2[size, size];
                ret[0, 0] = b;
                return ret;
            }).ToList(), 1, 0);

            var first = arrayOptions[0];
            for (var y = 0; y < first.GetLength(1) * 10; y++)
            {
                for (var x = 0; x < first.GetLength(0) * 10; x++)

                {
                    var board = first[x / 10, y / 10].Board.FlipY();
                    
                    Console.Write(board.ValueAt(x % 10, y % 10));
                    if (x % 10 == 9)
                        Console.Write(" ");
                }

                Console.WriteLine();
                if (y % 10 == 9)
                    Console.WriteLine();
            }

            for (var y = 0; y < first.GetLength(1); y++)
            {
                for (var x = 0; x < first.GetLength(0); x++)
                {
                    Console.Write(first[x, y].ID);
                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            var num =
                long.Parse(arrayOptions[0][0, 0].ID.Substring(5, 4)) *
                long.Parse(arrayOptions[0][0, size - 1].ID.Substring(5, 4)) *
                long.Parse(arrayOptions[0][size - 1, 0].ID.Substring(5, 4)) *
                long.Parse(arrayOptions[0][size - 1, size - 1].ID.Substring(5, 4));
            return num;
        }


        public static int Puzzle20Part2()
        {
            var strings = "Year2020\\Data\\Day20a.txt".ReadAll<string>();
            var size = 12;
            var boardStart = new Board(size * 8, size * 8);
            var x2 = 0;
            var y2 = 0;
            for (var y = 0; y < strings.Length; y++)
            {
                if (y % 10 == 0 || y % 10 == 9) continue;
                for (var x = 0; x < strings[0].Length; x++)
                {
                    if (x % 10 == 0 || x % 10 == 9) continue;
                    boardStart.SetValueAt(x2, y2, strings[y][x]);
                    x2++;
                }

                y2++;
                x2 = 0;
            }

            boardStart.Dump();
            var boards = GetAllBoards(boardStart);

            var pattern = new List<string>
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            Board boardWithMonsters = null;
            var count = 0;
            foreach (var board in boards)
            {
                for (var x = 0; x < size * 8 - pattern[0].Length; x++)
                {
                    for (var y = 0; y < size * 8 - 3; y++)
                    {
                        var match = true;

                        for (var yP = 0; yP < pattern.Count; yP++)
                        for (var xP = 0; xP < pattern[0].Length; xP++)
                        {
                            if (pattern[yP][xP] != '#') continue;
                            if (board.ValueAt(x + xP, y + yP) != '#')
                                match = false;
                        }

                        if (match)
                        {
                            count++;
                            for (var yP = 0; yP < pattern.Count; yP++)
                            for (var xP = 0; xP < pattern[0].Length; xP++)
                            {
                                if (pattern[yP][xP] != '#') continue;
                                board.SetValueAt(x + xP, y + yP, 'O');
                            }

                        }
                    }

                }

                if (count > 0)
                {
                    boardWithMonsters = board;
                    break;

                }
            }

            var countRough = 0;
            for (var x = 0; x < size * 8 ; x++)
            {
                for (var y = 0; y < size * 8; y++)
                {
                    if (boardWithMonsters?.ValueAt(x,y) == '#')
                        countRough++;
                }

            }
            return countRough;
        }

        public static int Puzzle21Part1()
        {
            var lines = "Year2020\\Data\\Day21.txt".ReadAll<string>().ToList();
            var lines2 = new List<(List<string> ingredients, List<string> allergens)>();
            foreach (var line in lines)
            {
                var split = line.Split("(contains ");
                lines2.Add((split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    split[1].TrimEnd(')').Split(", ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()));
            }

            var allIngredients = lines2.SelectMany(f => f.ingredients).Distinct();
            var allAllergens = lines2.SelectMany(l => l.allergens).Distinct()
                .ToDictionary(d => d, d => new List<string>());
            foreach (var allergen in allAllergens.Keys.ToList())
            {
                var foods = lines2.Where(l => l.allergens.Contains(allergen)).ToList();
                var commonIngredients = foods
                    .SelectMany(f => f.ingredients).Distinct().Where(f => foods.All(f2 => f2.ingredients.Contains(f)));
                allAllergens[allergen] = commonIngredients.ToList();
            }

            var dictionary = new Dictionary<string, string>();
            while (allAllergens.Any())
                foreach (var allAllergen in allAllergens.Keys.ToList())
                {
                    var unknown = allAllergens[allAllergen].Except(dictionary.Values).ToList();
                    if (unknown.Count == 1)
                    {
                        dictionary[allAllergen] = unknown.First();
                        allAllergens.Remove(allAllergen);
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
                lines2.Add((split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    split[1].TrimEnd(')').Split(", ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()));
            }

            var allAllergens = lines2.SelectMany(l => l.allergens).Distinct()
                .ToDictionary(d => d, d => new List<string>());
            foreach (var allergen in allAllergens.Keys.ToList())
            {
                var foods = lines2.Where(l => l.allergens.Contains(allergen)).ToList();
                var commonIngredients = foods
                    .SelectMany(f => f.ingredients).Distinct().Where(f => foods.All(f2 => f2.ingredients.Contains(f)));
                allAllergens[allergen] = commonIngredients.ToList();
            }

            var dictionary = new Dictionary<string, string>();
            while (allAllergens.Any())
                foreach (var allAllergen in allAllergens.Keys.ToList())
                {
                    var unknown = allAllergens[allAllergen].Except(dictionary.Values).ToList();
                    if (unknown.Count == 1)
                    {
                        dictionary[allAllergen] = unknown.First();
                        allAllergens.Remove(allAllergen);
                    }
                }

            var allergens = dictionary.Keys.ToList();
            allergens.Sort();
            var ings = allergens.Select(a => dictionary[a]).ToList();
            return string.Join(",", ings);
        }

        public static long Puzzle22Part1()
        {
            // var strings = "Year2020\\Data\\Day22.txt".ReadAll<string>();
            // var player1Cards = strings.Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ReadAll<int>().ToList();
            // var player2Cards = strings.Skip(3 + player1Cards.Count()).ReadAll<int>().ToList();
            //
            // while (player1Cards.Count() > 0 && player2Cards.Count() > 0)
            // {
            //     if (player1Cards[0] > player2Cards[0])
            //     {
            //         player1Cards.Add(player1Cards[0]);
            //         player1Cards.Add(player2Cards[0]);
            //     }
            //     else 
            //     {
            //         player2Cards.Add(player2Cards[0]);
            //         player2Cards.Add(player1Cards[0]);
            //     }
            //     player1Cards.RemoveAt(0);
            //     player2Cards.RemoveAt(0);
            // }
            //
            // long score = 0;
            // if (player1Cards.Count > 0)
            //     for (int i = player1Cards.Count - 1; i >= 0; i--)
            //     {
            //         score += player1Cards[i] * (player1Cards.Count - i);
            //     }
            // else 
            //     for (int i = player2Cards.Count - 1; i >= 0; i--)
            //     {
            //         score += player2Cards[i] * (player2Cards.Count - i);
            //     }
            // return score;
            return -1;
        }

        private static List<int> _primes = new List<int>
        {
            2,3,5,7,11,13,17,19,23,29 
            ,31,37,41,43,47,53,59,61,67,71 
            ,73,79,83,89,97,101,103,107,109,113 
            ,127,131,137,139,149,151,157,163,167,173 
            ,179,181,191,193,197,199,211,223,227,229 
            ,233,239,241,251,257,263,269,271,277,281 
            ,283,293,307,311,313,317,331,337,347,349 
            ,353,359,367,373,379,383,389,397,401,409 
            ,419,421,431,433,439,443,449,457,461,463 
            ,467,479,487,491,499,503,509,521,523,541 
            ,547,557,563,569,571,577,587,593,599,601 
            ,607,613,617,619,631,641,643,647,653,659 
            ,661,673,677,683,691,701,709,719,727,733 
            ,739,743,751,757,761,769,773,787,797,809 
            ,811,821,823,827,829,839,853,857,859,863 
            ,877,881,883,887,907,911,919,929,937,941 
            ,947,953,967,971,977,983,991,997,1009,1013 
        };

        private static int GetHash(List<short> player1Cards, List<short> player2Cards)
        {
            int hc= player1Cards.Count * _primes[101] + player2Cards.Count *_primes[102];
            int hc2= 0;
            int hc3= 0;
            for(int i=0;i<player1Cards.Count;++i)
            {
                hc2+= unchecked( _primes[i] *player1Cards[i]);
            }
            for(int i=0;i<player2Cards.Count;++i)
            {
                hc3+= unchecked(_primes[(i + player1Cards.Count)] *player2Cards[i]);
            }

            hc = unchecked(_primes.Last() * hc + _primes[19] * hc2 + _primes[20] * hc3);
            return hc;
        }
        private static int Combat(List<short> player1Cards, List<short> player2Cards,  Dictionary<int, int>  seenBefore)
        {
            HashSet<int> seenThis = new HashSet<int>();
            var startHash = GetHash(player1Cards, player2Cards);
            if (seenBefore.ContainsKey(startHash))
            {
                return seenBefore[startHash];
            }
            var hash = startHash;
            int winner = 1;
            while (player1Cards.Any() && player2Cards.Any())
            {
                if (seenThis.Contains(hash))
                {
                    seenBefore.Add(startHash,1);
                    return 1;
                }
                seenThis.Add(hash);
                var c1 = player1Cards.First();
                var c2 = player2Cards.First();
                if (player1Cards.Count >= c1 + 1 && player2Cards.Count >= c2 + 1)
                     winner = Combat(player1Cards.Skip(1).Take(c1).ToList(), player2Cards.Skip(1).Take(c2).ToList(),   seenBefore);
                else if (player1Cards[0] > player2Cards[0])
                    winner = 1;
                else
                    winner = 2;

                if (winner == 1)
                {
                    player1Cards.Add(c1);
                    player1Cards.Add(c2);
                }
                else
                {
                    player2Cards.Add(c2);
                    player2Cards.Add(c1);
                }
                player1Cards.RemoveAt(0);
                player2Cards.RemoveAt(0);
                hash = GetHash(player1Cards, player2Cards);
            }
            
            seenBefore.Add(startHash,winner);
            return winner;
            
        }

        public static long Puzzle22Part2()
        {
            var strings = "Year2020\\Data\\Day22.txt".ReadAll<string>();
            var player1Cards = strings.Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ReadAll<short>().ToList();
            var player2Cards = strings.Skip(3 + player1Cards.Count).ReadAll<short>().ToList();

            var winner = Combat(player1Cards, player2Cards,  new Dictionary<int, int>());

            long score = 0;
            var cards = (winner == 1 ? player1Cards : player2Cards);
            for (int i = cards.Count - 1; i >= 0; i--)
                score += cards[i] * (cards.Count - i);
            
            return score;
        }

        public static string Puzzle23Part1()
        {
            var cups = "562893147".Select(c => int.Parse(c.ToString())).ToList();

            var current = 0;

            int RemoveNextCupIndex(int currentValue)
            {
                var index = (cups.IndexOf(currentValue) + 1) % cups.Count;
                var ret = cups[index];
                cups.RemoveAt(index);
                return ret;
            }
            int GetDestinationCup(int index)
            {
                var i = cups[index];
                while (true)
                {
                    i--;
                    if (i < cups.Min()) i = cups.Max();
                    var dIndex = cups.IndexOf(i);
                    if (dIndex != -1) return dIndex;
                }
                
            }
            for (int i = 0; i < 100; i++)
            {
                
                var currentValue = cups[current];
                var pickedUp = new List<int>
                {
                    RemoveNextCupIndex(currentValue),
                    RemoveNextCupIndex(currentValue),
                    RemoveNextCupIndex(currentValue)
                };
                var d = (GetDestinationCup(cups.IndexOf(currentValue)) ) ;
                if (d == cups.Count - 1)
                {
                    cups.Add( pickedUp[0]);
                    cups.Add( pickedUp[1]);
                    cups.Add( pickedUp[2]);
                }
                else
                {
                    cups.Insert((d + 1) % (cups.Count), pickedUp[2]);
                    cups.Insert((d + 1) % (cups.Count), pickedUp[1]);
                    cups.Insert((d + 1) % (cups.Count), pickedUp[0]);
                }
                current = (cups.IndexOf(currentValue) + 1) % cups.Count;
                // for (int j = 0; j < cups.Count; j++)
                // {
                //     if (j == current)
                //         Console.Write($"({cups[j]}) ");
                //     else 
                //         Console.Write($"{cups[j]} ");
                // }
                // Console.WriteLine();
            }

            var s = new StringBuilder();
            for (int i = 1; i < cups.Count; i++)
            {
                var index = (cups.IndexOf(1) + i) % cups.Count;
                s.Append(cups[index]);
            }
            return s.ToString();
        }

        
        public static long Puzzle23Part2()
        {
            var cups = "562893147".Select(c => int.Parse(c.ToString())).ToList();
            
            const int max = 1000000;
            for (var i = cups.Count + 1; i <= max; i++)
            {
                cups.Add(i);
            }
            var circularLinkedList = new UniqueCircularLinkList<int>(cups);
           
            CircularLinkListNode<int> RemoveCups()
            {
                var ret = circularLinkedList.Current.Next;
                circularLinkedList.Current.SetNext(circularLinkedList.Current.Next.Next.Next.Next.Value);
                circularLinkedList.Current.Next.SetPrevious(circularLinkedList.Current.Value);

                return ret;
            }

            CircularLinkListNode<int> GetDestinationCup(CircularLinkListNode<int> pickedUp)
            {
                var value = circularLinkedList.Current.Value;
                while (true)
                {
                    value--;
                    if (value < 1) value = max;
                    if (pickedUp.Value == value ||
                        pickedUp.Next.Value == value ||
                        pickedUp.Next.Next.Value == value) continue;
                    return circularLinkedList.Find(value);
                }
            }

            for (var i = 0; i < 10000000; i++)
            {
                var pickedUp = RemoveCups();
                var d = GetDestinationCup(pickedUp);
                var temp = d.Next;
                d.SetNext(pickedUp.Value);
                pickedUp.Next.Next.SetNext(temp.Value);
                circularLinkedList.MoveNext();
            }


            return circularLinkedList.Find(1).Next.Value * (long) circularLinkedList.Find(1).Next.Next.Value;
        }


        public static int Puzzle24Part1()
        {
            var dict = GetDictionary();

            return dict.Values.Count(v => v % 2 == 1);
        }

        private static Dictionary<(int x, int y), int> GetDictionary()
        {
            var lines = "Year2020\\Data\\Day24.txt".ReadAll<string>();
            Dictionary<(int x, int y), int> dict = new Dictionary<(int x, int y), int>();
            foreach (var line in lines)
            {
                var l = line;
                int x = 0;
                int y = 0;
                while (l.Length > 0)
                {
                    if (l.StartsWith("e"))
                    {
                        x++;
                        l = l.Substring(1);
                    }
                    else if (l.StartsWith("se"))
                    {
                        y--;
                        l = l.Substring(2);
                    }
                    else if (l.StartsWith("sw"))
                    {
                        x--;
                        y--;
                        l = l.Substring(2);
                    }
                    else if (l.StartsWith("w"))
                    {
                        x--;
                        l = l.Substring(1);
                    }
                    else if (l.StartsWith("nw"))
                    {
                        y++;
                        l = l.Substring(2);
                    }
                    else if (l.StartsWith("ne"))
                    {
                        y++;
                        x++;
                        l = l.Substring(2);
                    }
                }

                if (!dict.ContainsKey((x, y)))
                    dict[(x, y)] = 0;
                dict[(x, y)]++;
            }

            return dict;
        }

        public static int Puzzle24Part2()
        {
            var hash = new HashSet<(int x, int y)>(GetDictionary().Where(d => d.Value % 2 == 1).Select(k => k.Key));
           
            int GetBlackCount((int x, int y) adjTile)
            {
                var bc = 0;
                if (hash.Contains((adjTile.x + 1, adjTile.y))) bc++;
                if (hash.Contains((adjTile.x , adjTile.y -1))) bc++;
                if (hash.Contains((adjTile.x -1, adjTile.y -1))) bc++;
                if (hash.Contains((adjTile.x -1, adjTile.y))) bc++;
                if (hash.Contains((adjTile.x, adjTile.y +1))) bc++;
                if (hash.Contains((adjTile.x + 1, adjTile.y+1))) bc++;
                return bc;
            }
            for (int i = 0; i < 100; i++)
            {
                var newHash =new HashSet<(int x, int y)>();
                foreach (var tile in hash)
                {
                    var blackCount = GetBlackCount(tile);
                   
                    if (blackCount == 1 || blackCount == 2)
                        newHash.Add(tile);

                    void CheckAdjacentWhite((int x, int y) adjTile)
                    {
                        if (hash.Contains(adjTile)) return;
                        if (GetBlackCount(adjTile) == 2)
                            newHash.Add(adjTile);
                    }

                    CheckAdjacentWhite((tile.x + 1, tile.y));
                    CheckAdjacentWhite((tile.x , tile.y -1));
                    CheckAdjacentWhite((tile.x -1, tile.y -1));
                    CheckAdjacentWhite((tile.x -1, tile.y));
                    CheckAdjacentWhite((tile.x, tile.y +1));
                    CheckAdjacentWhite((tile.x + 1, tile.y+1));

                }
                hash = newHash;
            }

            return hash.Count;
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
}