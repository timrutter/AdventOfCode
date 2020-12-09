using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Advent2020
    {
        #region Types

        public class Bag
        {
            #region Constructors

            public Bag(Bag parent,string color, List<(int, string)> children, int count = 1)
            {
                Parent = parent;
                Color = color;
                Count = count;
                Children = children?.Select(c => new Bag(this,c.Item2, null, c.Item1))
                    .ToList() ?? new List<Bag>();
            }

            public override string ToString()
            {
                return $"{Count} {Color}";
            }

            #endregion

            #region Properties

            public Bag Parent { get; }
            public string Color { get; }
            public int Count { get; }
            public List<Bag> Children { get; }

            #endregion

            #region Methods

            public List<Bag> FindBags(string color)
            {
                var ret = new List<Bag>();
                if (Color == color)
                    ret.Add(this);
                ret.AddRange(Children.SelectMany(c => c.FindBags(color)).Where(b => b != null));
                return ret;
            }

            #endregion

            public List<Bag> GetAllParents()
            {
                List<Bag > ret = new List<Bag>();

                var p = Parent;
                while (p != null)
                {
                    ret.Add(p);
                    p = p.Parent;
                }
                return ret;
            }

            public void Dump()
            {
                var ps = GetAllParents();
                var s = ToString();
                Console.WriteLine($"{s}".PadLeft(ps.Count * 2 +s.Length ));
                foreach (var child in Children)
                {
                    child.Dump();
                }
            }

            public int CountBags()
            {
                int count = 1;
                foreach (var child in Children)
                {
                    count += child.CountBags();
                }

                return count;
            }
        }

        #endregion

        #region Methods

        public static int Puzzle1Part1()
        {
            var arr = "Data2020\\Day01.txt".ReadAll<int>();
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
            var arr = "Data2020\\Day01.txt".ReadAll<int>();
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
            var kvps = "Data2020\\Day02.txt".ReadAllKeyValuePairs();
            var count = 0;
            foreach (var s in kvps)
            {
                var pword = s.value.Trim();
                var bits = s.key.Split(" ");
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
            var kvps = "Data2020\\Day02.txt".ReadAllKeyValuePairs();
            var count = 0;
            foreach (var s in kvps)
            {
                var pword = s.value.Trim();
                var bits = s.key.Split(" ");
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
            var trees = File.ReadAllLines("Data2020\\Day03.txt");
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
            var trees = File.ReadAllLines("Data2020\\Day03.txt");
            return Puzzle3Part1Internal(trees, 1, 1) *
                   Puzzle3Part1Internal(trees, 3, 1) *
                   Puzzle3Part1Internal(trees, 5, 1) *
                   Puzzle3Part1Internal(trees, 7, 1) *
                   Puzzle3Part1Internal(trees, 1, 2);
        }

        public static int Puzzle4Part1()
        {
            var passports = "Data2020\\Day04.txt".ReadAllBlankLineSeparatedRecords(true);
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
            var passports = "Data2020\\Day04.txt".ReadAllBlankLineSeparatedRecords(true);
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
            var lines = File.ReadAllLines("Data2020\\Day05.txt");

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
            var lines = File.ReadAllLines("Data2020\\Day05.txt");

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
            return "Data2020\\Day06.txt".ReadAllBlankLineSeparatedRecords()
                .Sum(q => q.RemoveAllWhiteSpace().Distinct().Count());
        }

        public static int Puzzle6Part2()
        {
            return "Data2020\\Day06.txt".ReadAllBlankLineSeparatedRecords().Select(l => l.SplitLines())
                .Sum(s => "abcdefghijklmnopqrstuvwxyz".Count(c => s.All(s1 => s1.Contains(c))));
        }

        public static int Puzzle7Part1()
        {
            var lines = "Data2020\\Day07.txt".ReadAllKeyValuePairs("contain");
            var dict = new List<Bag>();
            foreach (var line in lines)
            {
                var list = new List<(int, string)>();

                if (line.value != " no other bags.")
                {
                    var bits = line.value.Split(",");
                    foreach (var bit in bits)
                        list.Add((int.Parse(bit.Substring(1, 2)),CleanColour( bit.Substring(2))));
                }

                var color = CleanColour(line.key);
                var b = FindBags(dict, color);
                if (b.Count == 0)
                    dict.Add(new Bag(null,CleanColour(line.key), list));
                else
                    foreach (var bag in b)
                        bag.Children.AddRange(list.Select(l => new Bag(bag,l.Item2, null, l.Item1)));
            }

            
            var hashset = new HashSet<string>();
            GetAllParents(hashset,dict,"shiny gold");
            return hashset.Count();
        }

        private static void GetAllParents(HashSet<string> hashset, List<Bag> dict, string name)
        {
            var bags = FindBags(dict, name).ToList();
            foreach (var bag in bags)
            {
                foreach (var allParent in bag.GetAllParents())
                {
                    if (hashset.Contains(allParent.Color)) continue;
                    hashset.Add(allParent.Color);
                    GetAllParents(hashset,dict,allParent.Color);
                }
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
            var lines = "Data2020\\Day07.txt".ReadAllKeyValuePairs("contain");
            var dict = new Dictionary<string,Bag>();
            foreach (var line in lines)
            {
                var list = new List<(int, string)>();

                if (line.value != " no other bags.")
                {
                    var bits = line.value.Split(",");
                    foreach (var bit in bits)
                        list.Add((int.Parse(bit.Substring(1, 2)), CleanColour( bit.Substring(2))));
                }

                var bagName = CleanColour(line.key);
                Bag bag = dict.GetOrAdd(bagName,() => new Bag(null, bagName, null));
               
                foreach (var l in list)
                {
                    Bag childBag;
                    if (dict.ContainsKey(l.Item2))
                        childBag = dict[l.Item2];
                    else
                    {
                        childBag = new Bag(bag, l.Item2, null, l.Item1);
                        dict.Add(l.Item2,childBag);
                    }

                    for (int i = 0; i < l.Item1; i++)
                        bag.Children.Add(childBag);
                }

            }

            
            return  dict["shiny gold"].CountBags() -1;
        }


        public static int Puzzle8Part1()
        {
           var kvps =  "Data2020\\Day08.txt".ReadAllKeyValuePairs(" ").ToList();
           return Computer.RunProgram(kvps).Item1;
        }

        
        public static int Puzzle8Part2()
        {
            var kvps =  "Data2020\\Day08.txt".ReadAllKeyValuePairs(" ").ToList();
            for (int i = 0; i < kvps.Count; i++)
            {
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
            }

            return int.MaxValue;
        }

        public static long Puzzle9Part1()
        {
            var kvps =  "Data2020\\Day09.txt".ReadAll<long>().ToList();
            int count = 25;
            for (int i = count; i < kvps.Count; i++)
            {
                var (indeces, _) = kvps.Skip( i -count).Take(count).ToList().FindSum(kvps[i]);
                if (indeces  == null)
                    return kvps[i];
            }

            return int.MaxValue;
        }

        public static long Puzzle9Part2()
        {
            var target = 1309761972;
            var kvps =  "Data2020\\Day09.txt".ReadAll<long>().ToList();

            for (int i = 2; i < kvps.Count -1; i++)
            {
                var (values, _, _) = kvps.FindContiguousSum(target,i);
                if (values == null) continue;
                return values.Min() + values.Max();
            }

            return int.MaxValue;
        }

        public static int Puzzle10Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle10Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle11Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle11Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle12Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle12Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle13Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle13Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle14Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle14Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle15Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle15Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle16Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle16Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle17Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle17Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle18Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle18Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle19Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle19Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle20Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle20Part2()
        {
           return int.MaxValue;
        }

        public static int Puzzle21Part1()
        {
           return int.MaxValue;
        }

        public static int Puzzle21Part2()
        {
           return int.MaxValue;
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
        public const int InfiniteLoopDetected = 1;
        public static (int acc, int err) RunProgram(List<(string key, string value)> kvps)
        {
            var acc = 0;
            var ind = 0;
            var visited = new HashSet<int>();
            while (true)
            {
                if (ind >= kvps.Count) return (acc, 0);
                if (visited.Contains(ind))
                    return (acc,InfiniteLoopDetected);
                visited.Add(ind);
                switch (kvps[ind].key)
                {
                    case "acc":
                        acc += int.Parse( kvps[ind].value);
                        ind++;
                        break;
                    case "jmp":
                        ind +=  int.Parse(kvps[ind].value);
                        break;
                    case "nop":
                        ind++;
                        break;
                }

            }
        }
    }
}