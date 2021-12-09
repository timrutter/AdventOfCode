using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2016;

public class Advent2016Day01 : Solution
{
    public Advent2016Day01()
    {
        Answer1 = 332;
        Answer2 = 166;
    }

    public override object ExecutePart1()
    {
        string[] bits = File.ReadAllText(DataFile).Split(", ");
        var dirs = "NESW";
        var dir = 'N';
        int x = 0, y = 0;
        foreach (string bit in bits)
        {
            dir = bit[0] == 'R' ? dirs[Functions.Mod(dirs.IndexOf(dir) + 1, 4)] : dirs[Functions.Mod(dirs.IndexOf(dir) - 1, 4)];

            if (dir == 'N') y += int.Parse(bit[1..]);
            if (dir == 'E') x += int.Parse(bit[1..]);
            if (dir == 'S') y -= int.Parse(bit[1..]);
            if (dir == 'W') x -= int.Parse(bit[1..]);
        }

        return Math.Abs(x) + Math.Abs(y);
    }

    public override object ExecutePart2()
    {
        string[] bits = File.ReadAllText(DataFile).Split(", ");
        var dirs = "NESW";
        var dir = 'N';
        int x = 0, y = 0;
        var visited = new List<(int x, int y)>();
        foreach (string bit in bits)
        {
            char rl = bit[0];
            int dist = int.Parse(bit[1..]);
            dir = rl == 'R' ? 
                dirs[Functions.Mod(dirs.IndexOf(dir) + 1, 4)] : dirs[Functions.Mod(dirs.IndexOf(dir) - 1, 4)];

            switch (dir)
            {
                case 'N':
                {
                    foreach (var step in Functions.Range(y + 1, y + dist).Select(ystep => (x, ystep)))
                    {
                        y++;
                        if (visited.Contains(step))
                            return Math.Abs(x) + Math.Abs(y);
                        visited.Add(step);
                    }

                    continue;
                }
                case 'E':
                {
                    foreach (var step in Functions.Range(x + 1, x + dist).Select(xstep => (xstep, y)))
                    {
                        x++;
                        if (visited.Contains(step))
                            return Math.Abs(x) + Math.Abs(y);
                        visited.Add(step);
                    }

                    continue;
                }
                case 'S':
                {
                    foreach (var step in Functions.Range(y - 1, y - dist).Select(ystep => (x, ystep)))
                    {
                        y--;
                        if (visited.Contains(step))
                            return Math.Abs(x) + Math.Abs(y);
                        visited.Add(step);
                    }

                    continue;
                }
                case 'W':
                {
                    foreach (var step in Functions.Range(x - 1, x - dist).Select(xstep => (xstep, y)))
                    {
                        x--;
                        if (visited.Contains(step))
                            return Math.Abs(x) + Math.Abs(y);
                        visited.Add(step);
                    }

                    continue;
                }
            }
        }

        return Math.Abs(x) + Math.Abs(y);
    }
}