using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Advent2015;

public class Advent2015Day03 : Solution
{
    public Advent2015Day03()
    {
        Answer1 = 2592;
        Answer2 = 2360;
    }

    public override object ExecutePart1()
    {
        var data = File.ReadAllText(DataFile);
        var hashSet = new HashSet<(int, int)> { (0, 0) };
        int x = 0, y = 0;
        foreach (var t in data.ToList())
        {
            switch (t)
            {
                case '>': y++; break;
                case '<': y--; break;
                case '^': x++; break;
                case 'v': x--; break;
            }

            if (!hashSet.Contains((x, y)))
                hashSet.Add((x, y));
        }

        return hashSet.Count;
    }

    public override object ExecutePart2()
    {
        var data = File.ReadAllText(DataFile);
        var hashSet = new HashSet<(int, int)> { (0, 0) };
        int x = 0, y = 0;
        for (var index = 0; index < data.ToList().Count; index += 2)
        {
            var t = data.ToList()[index];
            switch (t)
            {
                case '>':
                    y++;
                    break;
                case '<':
                    y--;
                    break;
                case '^':
                    x++;
                    break;
                case 'v':
                    x--;
                    break;
            }

            if (!hashSet.Contains((x, y)))
                hashSet.Add((x, y));
        }

        x = 0;
        y = 0;
        for (var index = 1; index < data.ToList().Count; index += 2)
        {
            var t = data.ToList()[index];
            switch (t)
            {
                case '>':
                    y++;
                    break;
                case '<':
                    y--;
                    break;
                case '^':
                    x++;
                    break;
                case 'v':
                    x--;
                    break;
            }

            if (!hashSet.Contains((x, y)))
                hashSet.Add((x, y));
        }

        return hashSet.Count;
    }
}