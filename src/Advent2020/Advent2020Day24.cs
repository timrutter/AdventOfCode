using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020;

public class Advent2020Day24 : Solution
{
    public Advent2020Day24()
    {
        Answer1 = 373;
        Answer2 = 3917;
    }

    private Dictionary<Point, int> GetDictionary()
    {
        var lines = DataFile.ReadAll<string>();
        var dict = new Dictionary<Point, int>();
        foreach (var line in lines)
        {
            var l = line;
            var x = 0;
            var y = 0;
            while (l.Length > 0)
                if (l.StartsWith("e"))
                {
                    x++;
                    l = l[1..];
                }
                else if (l.StartsWith("se"))
                {
                    y--;
                    l = l[2..];
                }
                else if (l.StartsWith("sw"))
                {
                    x--;
                    y--;
                    l = l[2..];
                }
                else if (l.StartsWith("w"))
                {
                    x--;
                    l = l[1..];
                }
                else if (l.StartsWith("nw"))
                {
                    y++;
                    l = l[2..];
                }
                else if (l.StartsWith("ne"))
                {
                    y++;
                    x++;
                    l = l[2..];
                }

            if (!dict.ContainsKey(new Point(x, y)))
                dict[new Point(x, y)] = 0;
            dict[new Point(x, y)]++;
        }

        return dict;
    }

    public override object ExecutePart1()
    {
        var dict = GetDictionary();

        return dict.Values.Count(v => v % 2 == 1);
    }

    public override object ExecutePart2()
    {
        var hash = new HashSet<Point>(GetDictionary().Where(d => d.Value % 2 == 1).Select(k => k.Key));

        int GetBlackCount(Point adjTile)
        {
            var bc = 0;
            if (hash.Contains(new Point(adjTile.X + 1, adjTile.Y))) bc++;
            if (hash.Contains(new Point(adjTile.X, adjTile.Y - 1))) bc++;
            if (hash.Contains(new Point(adjTile.X - 1, adjTile.Y - 1))) bc++;
            if (hash.Contains(new Point(adjTile.X - 1, adjTile.Y))) bc++;
            if (hash.Contains(new Point(adjTile.X, adjTile.Y + 1))) bc++;
            if (hash.Contains(new Point(adjTile.X + 1, adjTile.Y + 1))) bc++;
            return bc;
        }

        for (var i = 0; i < 100; i++)
        {
            var newHash = new HashSet<Point>();
            foreach (var tile in hash)
            {
                var blackCount = GetBlackCount(tile);

                if (blackCount == 1 || blackCount == 2)
                    newHash.Add(tile);

                void CheckAdjacentWhite(Point adjTile)
                {
                    if (hash.Contains(adjTile)) return;
                    if (GetBlackCount(adjTile) == 2)
                        newHash.Add(adjTile);
                }

                CheckAdjacentWhite(new Point(tile.X + 1, tile.Y));
                CheckAdjacentWhite(new Point(tile.X, tile.Y - 1));
                CheckAdjacentWhite(new Point(tile.X - 1, tile.Y - 1));
                CheckAdjacentWhite(new Point(tile.X - 1, tile.Y));
                CheckAdjacentWhite(new Point(tile.X, tile.Y + 1));
                CheckAdjacentWhite(new Point(tile.X + 1, tile.Y + 1));
            }

            hash = newHash;
        }

        return hash.Count;
    }
}