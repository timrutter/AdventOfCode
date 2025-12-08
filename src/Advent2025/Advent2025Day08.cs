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
        {
            for (int j = i + 1; j < _points.Count; j++)
            {
                if (i == j) continue;
                var p1 = _points[i];
                var p2 = _points[j];
                _links.Add(new Link(p1,p2));
            }
        }
        _links.Sort((p1, p2) => p1.Distance.CompareTo(p2.Distance));
        Answer1 = 102816;
        Answer2 = 100011612;
    }


    private class Link(Point3D point1, Point3D point2)
    {
        public Point3D Point1 { get;set; } = point1;
        public Point3D Point2 { get;set; } = point2;
        public double Distance { get; set; } = point1.DistanceTo(point2);

        public override string ToString()
        {
            return $"{Point1}, {Point2}, {Distance}";
        }
    }

    public override object ExecutePart1()
    {

        var linkedLinks = new List< List<Link>>();
        int count = 1000;
        for (int i = 0; i < count; i++)
        {
            var link = _links[i];
            var p1 = _links[i].Point1;
            var p2 = _links[i].Point2;
            var circuit1 = linkedLinks.Find(l => l.Find(p => p.Point1 == p1|| p.Point2 == p1) != null);
            var circuit2 = linkedLinks.Find(l => l.Find(p => p.Point1 == p2|| p.Point2 == p2)!= null);
            if (circuit1 == null && circuit2 == null)
            { 
                linkedLinks.Add([link]);
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
            linkedLinks.Remove(circuit1);
            linkedLinks.Remove(circuit2);
            circuit2.Add(link);
            linkedLinks.Add(circuit1.Concat(circuit2).ToList());
        }
        linkedLinks.Sort((l1,l2) => l2.Count.CompareTo(l1.Count));
        var d =  linkedLinks.Take(3).Aggregate(1, (c, l) => c * (l.Count + 1));
        return d;
    }


    public override object ExecutePart2()
    {
        var pointsNotInCircuit = _points.ToList();
        var linkedLinks = new List< List<Link>>();
        Link lastLink = null;
        bool moreThanOne = false;
        var i = 0;
        while(true)
        {
            if (linkedLinks.Count > 1) moreThanOne = true;
            if (linkedLinks.Count == 1 && moreThanOne && pointsNotInCircuit.Count == 0)
                break;
            lastLink = _links[i++];
            var p1 = lastLink.Point1;
            var p2 = lastLink.Point2;
            pointsNotInCircuit.Remove(p1);
            pointsNotInCircuit.Remove(p2);
            var circuit1 = linkedLinks.Find(l => l.Find(p => p.Point1 == p1|| p.Point2 == p1) != null);
            var circuit2 = linkedLinks.Find(l => l.Find(p => p.Point1 == p2|| p.Point2 == p2)!= null);
            if (circuit1 == null && circuit2 == null)
            { 
                linkedLinks.Add([lastLink]);
                
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
            linkedLinks.Remove(circuit1);
            linkedLinks.Remove(circuit2);
            circuit2.Add(lastLink);
            linkedLinks.Add(circuit1.Concat(circuit2).ToList());
        }
        return lastLink!.Point1.X * lastLink.Point2.X;
    }
}