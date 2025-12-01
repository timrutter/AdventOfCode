using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Helpers;

public class Dijkstra
{
    private readonly Dictionary<Point, Point> _previousPointsPerPoint;
    private readonly Point _start;

    public Dijkstra(Board<int> startBoard, Point start)
    {
        _start = start;
        var dict = new Dictionary<Point, int>();
        var prev = new Dictionary<Point, Point>();
        var q = new HashSet<Point>();
        foreach (var pos in startBoard.Positions)
        {
            dict.Add(pos, int.MaxValue);
            prev.Add(pos, null);
            q.Add(pos);
        }

        dict[start] = 0;
        // this dictionary significantly speeds up finding the min each iteration 
        var distanceDict = new Dictionary<int, HashSet<Point>> { { 0, [start] } };
        while (distanceDict.Any())
        {
            var min = distanceDict.Keys.Min();
            var u = distanceDict[min].First();
            distanceDict[min].Remove(u);
            if (distanceDict[min].Count == 0) distanceDict.Remove(min);
            q.Remove(u);
            var options = new[] { u.Right(), u.Below(), u.Above(), u.Left() }.Where(p =>
                startBoard.PositionInRange(p) && q.Contains(p)).ToList();
            foreach (var option in options)
            {
                var alt = min + startBoard.ValueAt(option);
                if (alt >= dict[option]) continue;
                dict[option] = alt;
                if (!distanceDict.ContainsKey(alt)) distanceDict.Add(alt, []);
                distanceDict[alt].Add(option);
                prev[option] = u;
            }
        }

        Weightings = dict;
        _previousPointsPerPoint = prev;
    }

    public IReadOnlyDictionary<Point, int> Weightings { get; }

    public IEnumerable<Point> GetRoute(Point end)
    {
        var route = new List<Point>();
        var current = end;
        while (current != _start)
        {
            route.Add(current);
            current = _previousPointsPerPoint[current];
        }

        route.Add(_start);
        route.Reverse();
        return route;
    }
}