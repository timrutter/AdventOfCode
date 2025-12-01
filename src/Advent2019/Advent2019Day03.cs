using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2019;

public class Advent2019Day03 : Solution
{
    public Advent2019Day03()
    {
        Answer1 = 865;
        Answer2 = 35038;
    }

    private static Dictionary<Point, (int t1, int t2)> GetIntersects(List<(string, int)> p1, List<(string, int)> p2)
    {
        var path1 = new Dictionary<Point, int>();
        var current = new Point(0, 0);
        var t = 0;
        foreach (var command in p1)
            switch (command.Item1)
            {
                case "R":
                    for (var i = 0; i < command.Item2; i++)
                    {
                        t++;
                        current = current.Right();
                        if (path1.ContainsKey(current)) continue;
                        path1.Add(current, t);
                    }

                    break;
                case "L":
                    for (var i = 0; i < command.Item2; i++)
                    {
                        t++;
                        current = current.Left();
                        if (path1.ContainsKey(current)) continue;
                        path1.Add(current, t);
                    }

                    break;
                case "U":
                    for (var i = 0; i < command.Item2; i++)
                    {
                        t++;
                        current = current.Below();
                        if (path1.ContainsKey(current)) continue;
                        path1.Add(current, t);
                    }

                    break;
                case "D":
                    for (var i = 0; i < command.Item2; i++)
                    {
                        t++;
                        current = current.Above();
                        if (path1.ContainsKey(current)) continue;
                        path1.Add(current, t);
                    }

                    break;
            }

        current = new Point(0, 0);
        t = 0;
        var intersects = new Dictionary<Point, (int t1, int t2)>();
        foreach (var command in p2)
            switch (command.Item1)
            {
                case "R":
                    for (var i = 0; i < command.Item2; i++)
                    {
                        t++;
                        current = current.Right();
                        if (path1.ContainsKey(current))
                            intersects.Add(current, (path1[current], t));
                    }

                    break;
                case "L":
                    for (var i = 0; i < command.Item2; i++)
                    {
                        t++;
                        current = current.Left();
                        if (path1.ContainsKey(current))
                            intersects.Add(current, (path1[current], t));
                    }

                    break;
                case "U":
                    for (var i = 0; i < command.Item2; i++)
                    {
                        t++;
                        current = current.Below();
                        if (path1.ContainsKey(current))
                            intersects.Add(current, (path1[current], t));
                    }

                    break;
                case "D":
                    for (var i = 0; i < command.Item2; i++)
                    {
                        t++;
                        current = current.Above();
                        if (path1.ContainsKey(current))
                            intersects.Add(current, (path1[current], t));
                    }

                    break;
            }

        return intersects;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>();
        var p1 = lines[0]
            .Split(",").Select(step => (step[..1], int.Parse(step[1..]))).ToList();
        var p2 = lines[1]
            .Split(",").Select(step => (step[..1], int.Parse(step[1..]))).ToList();

        var intersects = GetIntersects(p1, p2);


        return intersects.Select(t => Math.Abs(t.Key.X) + Math.Abs(t.Key.Y)).Min();
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>();
        var p1 = lines[0]
            .Split(",").Select(step => (step[..1], int.Parse(step[1..]))).ToList();
        var p2 = lines[1]
            .Split(",").Select(step => (step[..1], int.Parse(step[1..]))).ToList();

        var intersects = GetIntersects(p1, p2);


        return intersects.Select(t => Math.Abs(t.Value.t1) + Math.Abs(t.Value.t2)).Min();
    }
}