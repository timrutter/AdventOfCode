using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Advent2021;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day08 : Solution
{
    private readonly List<Link> _links;
    private readonly List<Point3D> _points;

    public Advent2025Day08()
    {
        _points = DataFile.ReadAllFromFileAndSplitToType<int>(",").Select(s => new Point3D(s[0], s[1], s[2])).ToList();
        _links = [];
        for (int i = 0; i < _points.Count; i++)
            for (int j = i + 1; j < _points.Count; j++)
                _links.Add(new Link(_points[i],_points[j]));
        _links.Sort((p1, p2) => p1.Distance.CompareTo(p2.Distance));
        Answer1 = 102816;
        Answer2 = 100011612;
    }


    private class Link(Point3D point1, Point3D point2)
    {
        public Point3D Point1 { get; } = point1;
        public Point3D Point2 { get; } = point2;
        public double Distance { get; } = point1.DistanceTo(point2);

        public override string ToString()
        {
            return $"{Point1}, {Point2}, {Distance}";
        }
    }

    public override object ExecutePart1()
    {
        var circuits = new List< List<Link>>();
        int count = 1000;
        for (int i = 0; i < count; i++)
        {
            var link = _links[i];
            var p1 = link.Point1;
            var p2 = link.Point2;
            var circuit1 = circuits.Find(l => l.Find(p => p.Point1 == p1|| p.Point2 == p1) != null);
            var circuit2 = circuits.Find(l => l.Find(p => p.Point1 == p2|| p.Point2 == p2)!= null);
            if (circuit1 == null && circuit2 == null)
            { 
                circuits.Add([link]);
                continue;
            }

            if (circuit1 == null)
            {
                circuit2.Add(link);
                continue;
            }
            if (circuit2 == null)
            {
                circuit1.Add(link);
                continue;
            }
            if (circuit1 == circuit2) continue;
            circuits.Remove(circuit1);
            circuits.Remove(circuit2);
            circuit2.Add(link);
            circuits.Add(circuit1.Concat(circuit2).ToList());
        }
        circuits.Sort((l1,l2) => l2.Count.CompareTo(l1.Count));
        var d =  circuits.Take(3).Aggregate(1, (c, l) => c * (l.Count + 1));
        return d;
    }


    public override object ExecutePart2()
    {
        var pointsNotInCircuit = _points.ToList();
        var circuits = new List< List<Link>>();
        Link lastLink = null;
        var i = 0;
        while(circuits.Count > 1 || pointsNotInCircuit.Count > 0)
        {
            lastLink = _links[i++];
            var p1 = lastLink.Point1;
            var p2 = lastLink.Point2;
            pointsNotInCircuit.Remove(p1);
            pointsNotInCircuit.Remove(p2);
            var circuit1 = circuits.Find(l => l.Find(p => p.Point1 == p1|| p.Point2 == p1) != null);
            var circuit2 = circuits.Find(l => l.Find(p => p.Point1 == p2|| p.Point2 == p2)!= null);
            if (circuit1 == null && circuit2 == null)
            { 
                circuits.Add([lastLink]);
                
                continue;
            }
            if (circuit1 == null)
            {
                circuit2.Add(lastLink);
                continue;
            }
            if (circuit2 == null)
            {
                circuit1.Add(lastLink);
                continue;
            }
            if (circuit1 == circuit2) continue;
            circuits.Remove(circuit1);
            circuits.Remove(circuit2);
            circuit2.Add(lastLink);
            circuits.Add(circuit1.Concat(circuit2).ToList());
        }
        return lastLink!.Point1.X * lastLink.Point2.X;
    }
}