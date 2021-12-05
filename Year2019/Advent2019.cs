using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Year2019
{
    public static class Advent2019
    {
        
        #region Methods

        public static int Puzzle1Part1()
        {
            return "Year2019\\Data\\Day01.txt".ReadAll<int>().Select(i => i / 3 - 2).Sum();
        }

        public static int Puzzle1Part2()
        {
            return "Year2019\\Data\\Day01.txt".ReadAll<int>().Select(i =>
            {
                var sum = 0;
                var res = i;
                while (res > 0)
                {
                    var val = res / 3 - 2;
                    res = val;
                    sum += res > 0 ? res : 0;
                }
                return sum;
            }).Sum();

        }

        
        public static int Puzzle2Part1()
        {
            var ints =File.ReadAllText("Year2019\\Data\\Day02.txt").SplitToType<int>(",").ToList();
            ints[1] = 12;
            ints[2] = 2;
            return ints.CreateAndExecuteOpcodeComputer().Memory[0];
        }

        public static int Puzzle2Part2()
        {
            var intsStart =File.ReadAllText("Year2019\\Data\\Day02.txt").SplitToType<int>(",").ToList();
            for (int noun = 0; noun <= 99; noun++)
            for (int verb = 0; verb <= 99; verb++)
            {
                intsStart[1] = noun;
                intsStart[2] = verb;
                var ret = intsStart.CreateAndExecuteOpcodeComputer().Memory[0];
                if (ret == 19690720)
                    return 100 * noun + verb;
            }

            return -1;
        }

        public static int Puzzle3Part1()
        {
            var lines = "Year2019\\Data\\Day03.txt".ReadAll<string>();
            var p1 =lines[0]
                .Split(",").Select(step => (step.Substring(0,1),int.Parse(step[1..]))).ToList();
            var p2 =lines[1]
                .Split(",").Select(step => (step.Substring(0,1),int.Parse(step[1..]))).ToList();
           
            var intersects = GetIntersects(p1,p2);
            

            return intersects.Select(t => Math.Abs(t.Key.x) + Math.Abs(t.Key.y)).Min();
        }

        private static Dictionary<(int x, int y), (int t1, int t2)> GetIntersects(List<(string, int)> p1, List<(string, int)> p2)
        {
            
            var path1 = new Dictionary<(int x, int y), int>();
            var current = (0,0);
            var t = 0;
            foreach (var command in p1)
            {
                switch (command.Item1)
                {
                    case "R":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1 + 1, current.Item2);
                            if (path1.ContainsKey(current)) continue;
                            path1.Add(current,t);
                        }

                        break;
                    case "L":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1 - 1, current.Item2);
                            if (path1.ContainsKey(current)) continue;
                            path1.Add(current,t);
                        }

                        break;
                    case "U":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1, current.Item2 + 1);
                            if (path1.ContainsKey(current)) continue;
                            path1.Add(current,t);
                        }

                        break;
                    case "D":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1, current.Item2 - 1);
                            if (path1.ContainsKey(current)) continue;
                            path1.Add(current,t);
                        }

                        break;
                }
            }
            current = (0,0);
            t = 0;
            var intersects = new Dictionary<(int x, int y), (int t1, int t2)>();
            foreach (var command in p2)
            {
                switch (command.Item1)
                {
                    case "R":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1 + 1, current.Item2);
                            if (path1.ContainsKey(current))
                                intersects.Add(current,(path1[current], t));
                        }

                        break;
                    case "L":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1 - 1, current.Item2);
                            if (path1.ContainsKey(current))
                                intersects.Add(current,(path1[current], t));
                        }

                        break;
                    case "U":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1, current.Item2 + 1);
                            if (path1.ContainsKey(current))
                                intersects.Add(current,(path1[current], t));
                        }

                        break;
                    case "D":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1, current.Item2 - 1);
                            if (path1.ContainsKey(current))
                                intersects.Add(current,(path1[current], t));
                        }

                        break;
                }
            }

            return intersects;
        }

        public static int Puzzle3Part2()
        {
            var lines = "Year2019\\Data\\Day03.txt".ReadAll<string>();
            var p1 =lines[0]
                .Split(",").Select(step => (step.Substring(0,1),int.Parse(step[1..]))).ToList();
            var p2 =lines[1]
                .Split(",").Select(step => (step.Substring(0,1),int.Parse(step[1..]))).ToList();
           
            var intersects = GetIntersects(p1,p2);
            

            return intersects.Select(t => Math.Abs(t.Value.t1) + Math.Abs(t.Value.t2)).Min();
        }

        public static int Puzzle4Part1()
        {
            int count = 0;
            for (int i = 168630; i <= 718098; i++)
            {
                var s = i.ToString();
                if (!"0123456789".ToCharArray().Any(c => Regex.IsMatch(s, $"{c}{c}"))) continue;
                var ret = true;
                for (int j = 0; j < s.Length -1; j++)
                {
                    if (s[j] <= s[j + 1]) continue;
                    ret = false;
                    break;
                }

                if (ret) count++;

            }
            return count;
        }

        public static int Puzzle4Part2()
        {
            int count = 0;
            for (int i = 168630; i <= 718098; i++)
            {
                var s = i.ToString();
                if (!"0123456789".ToCharArray()
                    .Any(c => Regex.IsMatch(s, $"^{c}{c}[^{c}]|[^{c}]{c}{c}[^{c}]|[^{c}]{c}{c}$"))) continue;
                var ret = true;
                for (int j = 0; j < s.Length -1; j++)
                {
                    if (s[j] <= s[j + 1]) continue;
                    ret = false;
                    break;
                }

                if (ret)  count++;

            }
            return count;
        }

        public static int Puzzle5Part1()
        {
            
            return int.MaxValue;
        }

        public static int Puzzle5Part2()
        {
            
            return int.MaxValue;
        }

        public static int Puzzle6Part1()
        {
            
            return int.MaxValue;
        }

        public static int Puzzle6Part2()
        {
           
            return int.MaxValue;
        }

        public static int Puzzle7Part1()
        {
            
            return int.MaxValue;
        }
        public static int Puzzle7Part2()
        {
          
            return int.MaxValue;
        }


        public static int Puzzle8Part1()
        {
          
            return int.MaxValue;
        }

        
        public static int Puzzle8Part2()
        {
           
            return int.MaxValue;
        }

        public static int Puzzle9Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle9Part2()
        {
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
}