using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2019
{
    public class Advent2019Day03 : Solution
    {
        public Advent2019Day03()
        {
            Answer1 = 865;
            Answer2 = 35038;
        }
        private static Dictionary<(int x, int y), (int t1, int t2)> GetIntersects(List<(string, int)> p1, List<(string, int)> p2)
        {

            var path1 = new Dictionary<(int x, int y), int>();
            var current = (0, 0);
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
                            path1.Add(current, t);
                        }

                        break;
                    case "L":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1 - 1, current.Item2);
                            if (path1.ContainsKey(current)) continue;
                            path1.Add(current, t);
                        }

                        break;
                    case "U":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1, current.Item2 + 1);
                            if (path1.ContainsKey(current)) continue;
                            path1.Add(current, t);
                        }

                        break;
                    case "D":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1, current.Item2 - 1);
                            if (path1.ContainsKey(current)) continue;
                            path1.Add(current, t);
                        }

                        break;
                }
            }
            current = (0, 0);
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
                                intersects.Add(current, (path1[current], t));
                        }

                        break;
                    case "L":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1 - 1, current.Item2);
                            if (path1.ContainsKey(current))
                                intersects.Add(current, (path1[current], t));
                        }

                        break;
                    case "U":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1, current.Item2 + 1);
                            if (path1.ContainsKey(current))
                                intersects.Add(current, (path1[current], t));
                        }

                        break;
                    case "D":
                        for (int i = 0; i < command.Item2; i++)
                        {
                            t++;
                            current = (current.Item1, current.Item2 - 1);
                            if (path1.ContainsKey(current))
                                intersects.Add(current, (path1[current], t));
                        }

                        break;
                }
            }

            return intersects;
        }
        public override object ExecutePart1()
        {
            var lines = DataFile.ReadAll<string>();
            var p1 = lines[0]
                .Split(",").Select(step => (step.Substring(0, 1), int.Parse(step[1..]))).ToList();
            var p2 = lines[1]
                .Split(",").Select(step => (step.Substring(0, 1), int.Parse(step[1..]))).ToList();

            var intersects = GetIntersects(p1, p2);


            return intersects.Select(t => Math.Abs(t.Key.x) + Math.Abs(t.Key.y)).Min();
        }

        public override object ExecutePart2()
        {
            var lines = DataFile.ReadAll<string>();
            var p1 = lines[0]
                .Split(",").Select(step => (step.Substring(0, 1), int.Parse(step[1..]))).ToList();
            var p2 = lines[1]
                .Split(",").Select(step => (step.Substring(0, 1), int.Parse(step[1..]))).ToList();

            var intersects = GetIntersects(p1, p2);


            return intersects.Select(t => Math.Abs(t.Value.t1) + Math.Abs(t.Value.t2)).Min();
        }
    }
}