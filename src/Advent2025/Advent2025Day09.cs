using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day09 : Solution
{
    private readonly List<Point> _coords;

    public Advent2025Day09()
    {
        _coords = DataFile.ReadAllFromFileAndSplitToType<int>(",").Select(p => new Point(p[0], p[1])).ToList();
        Answer1 = 4773451098;
        Answer2 = 1429075575L;
    }

    public override object ExecutePart1()
    {
        long max = 0;
        for (var i = 0; i < _coords.Count; i++)
        for (var j = i + 1; j < _coords.Count; j++)
            max = Math.Max(max, GetArea(_coords[i], _coords[j]));

        return max;
    }

    private static long GetArea(Point p1, Point p2)
    {
        long xl = Math.Abs(p2.X - p1.X) + 1;
        long yl = Math.Abs(p2.Y - p1.Y) + 1;
        return xl * yl;
    }

    public override object ExecutePart2()
    {
        var edges = new List<(Point p1, Point p2)>();
        for (var i = 0; i < _coords.Count; i++)
        {
            var p1 = _coords[i];
            var nextPoint = i == _coords.Count - 1 ? _coords[0] : _coords[i + 1];
            edges.Add((p1, nextPoint));
        }

        long tmax = 0;
        for (var i = 0; i < _coords.Count; i++)
            for (var j = i + 1; j < _coords.Count; j++)
                if (AllAreGreen(_coords[i], _coords[j], edges))
                    tmax = Math.Max(tmax, GetArea(_coords[i], _coords[j]));

        return tmax;
    }

    private static bool AllAreGreen(Point p1, Point p2, List<(Point p1, Point p2)> edges)
    {
        var minX = Math.Min(p1.X, p2.X) + 1;
        var minY = Math.Min(p1.Y, p2.Y) + 1;
        var maxX = Math.Max(p1.X, p2.X) - 1;
        var maxY = Math.Max(p1.Y, p2.Y) - 1;
        var corners = new List<Point>
        {
            new(minX, minY),
            new(maxX, minY),
            new(maxX, maxY),
            new(minX, maxY)
        };
        if (CountIntersects(edges, (corners[0], corners[1])) > 0) return false;
        if (CountIntersects(edges, (corners[1], corners[2])) > 0) return false;
        if (CountIntersects(edges, (corners[2], corners[3])) > 0) return false;
        if (CountIntersects(edges, (corners[3], corners[0])) > 0) return false;
        return true;
    }

    private static int CountIntersects(List<(Point p1, Point p2)> edges, (Point p1, Point p2) line)
    {
        return edges.Count(edge => LinesIntersect(edge.p1, edge.p2, line.p1, line.p2));
    }

    public static bool LinesIntersect(Point a1, Point a2, Point b1, Point b2)
    {
        var aVertical = a1.X == a2.X;
        var bVertical = b1.X == b2.X;

        if (aVertical && bVertical) return false;

        if (!aVertical && !bVertical) return false;

        var v1 = aVertical ? a1 : b1;
        var v2 = aVertical ? a2 : b2;
        var h1 = aVertical ? b1 : a1;
        var h2 = aVertical ? b2 : a2;

        var vX = v1.X;
        var vMinY = Math.Min(v1.Y, v2.Y);
        var vMaxY = Math.Max(v1.Y, v2.Y);

        var hY = h1.Y;
        var hMinX = Math.Min(h1.X, h2.X);
        var hMaxX = Math.Max(h1.X, h2.X);

        return vX > hMinX && vX < hMaxX &&
               hY > vMinY && hY < vMaxY;
    }
}