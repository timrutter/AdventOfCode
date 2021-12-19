using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day19 : Solution
{
    private readonly List<Scanner> _scanners;
    private readonly List<Translation> _transformations;

    public Advent2021Day19()
    {
        Answer1 = 465;
        Answer2 = null;
        var records = DataFile.ReadAllBlankLineSeparatedRecords().Where(s => s.Length > 0).ToList();
        _scanners = records.Select(s => s[19..].ReadAllSplitToType<int>(",", StringSplitOptions.RemoveEmptyEntries))
            .Select(p => new Scanner(p.Select(ps => new Point3D(ps[0], ps[1], ps[2])).ToList())).ToList();
        _transformations = GetTranslations();
        _scanners[0].Position = new Point3D(0, 0, 0);
    }

    private List<Translation> GetTranslations()
    {
        var ret = new List<Translation>();
        var hashSet = new List<Point3D>();
        foreach (var x in new [] {SwapMode.X1, SwapMode.X2, SwapMode.X3, SwapMode.X4})
        {
            foreach (var y in new[] { SwapMode.Y1, SwapMode.Y2, SwapMode.Y3, SwapMode.Y4 })
            {
                foreach (var z in new[] { SwapMode.Z1, SwapMode.Z2, SwapMode.Z3, SwapMode.Z4 })
                {
                    var t = new Translation(new List<SwapMode> { x,y,z});
                    var p = t.Apply(new Point3D(1, 2, 3));
                    if (hashSet.Contains(p)) continue;
                    hashSet.Add(p);
                    ret.Add(t);
                }
            }
        }

        return ret;
    }
    public class Translation
    {
        public override string ToString()
        {
            return string.Join(",", _swapModes);
        }

        private readonly List<SwapMode> _swapModes;

        public Translation(List<SwapMode> swapModes)
        {
            _swapModes = swapModes;
        }
        
        private Point3D ApplySwapMode(Point3D p, SwapMode swapMode)
        {
            switch (swapMode)
            {
                case SwapMode.X1:
                    return p;
                case SwapMode.X2:
                    return new Point3D(p.Y, -p.X, p.Z);
                case SwapMode.X3:
                    return new Point3D(-p.X, -p.Y, p.Z);
                case SwapMode.X4:
                    return new Point3D(-p.Y, p.X, p.Z);
                case SwapMode.Y1:
                    return p;
                case SwapMode.Y2:
                    return new Point3D(p.Z, p.Y, -p.X);
                case SwapMode.Y3:
                    return new Point3D(-p.X, p.Y, -p.Z);
                case SwapMode.Y4:
                    return new Point3D(-p.Z, p.Y, p.X);
                case SwapMode.Z1:
                    return p;
                case SwapMode.Z2:
                    return new Point3D(p.X, p.Z, -p.Y);
                case SwapMode.Z3:
                    return new Point3D(p.X, -p.Y, -p.Z);
                case SwapMode.Z4:
                    return new Point3D(p.X, -p.Z, p.Y);
                default:
                    return p;
            }

        }

        public Point3D Apply(Point3D point3D)
        {
            var ret = point3D;
            foreach (var swapMode in _swapModes)
            {
                ret = ApplySwapMode(ret, swapMode);
            }

            return ret;
        }
        
    }

    public class Scanner
    {
        public Point3D Position { get; set; }
        public List<Point3D> Beacons { get; }

        public Scanner(List<Point3D> beacons = null)
        {
            Beacons = beacons ?? new List<Point3D>();
        }
        
        public void Apply(Translation translation)
        {
            for (int i = 0; i < this.Beacons.Count; i++)
            {
                Beacons[i] = translation.Apply(Beacons[i]);
            }
        }

        public Scanner Transform(Translation translation)
        {
            var ret = new Scanner();
            foreach (var point3D in Beacons)
            {
                ret.Beacons.Add(translation.Apply(point3D));
            }

            return ret;
        }
    }
    public class Point3D
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected bool Equals(Point3D other)
        {
            if (ReferenceEquals(other, null)) return false;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public static Point3D operator -(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point3D operator +(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static bool operator ==(Point3D p1, Point3D p2)
        {
            if (p1 is null && p2 is null) return true;
            return p2 is not null && p1 is not null && p1.Equals(p2);
        }

        public static bool operator !=(Point3D p1, Point3D p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Point3D) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Point3D(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }

        public static Point3D operator *(Point3D p1, Point3D p2)
        {
            return new Point3D(
                p1.X * p2.X,
                p1.Y * p2.Y,
                p1.Z * p2.Z);
        }
    }

    
    public override object ExecutePart1()
    {
        var unknownScanners = _scanners.Skip(1).ToList();
        while (unknownScanners.Count > 0)
        {
            foreach (var scanner in _scanners.Except(unknownScanners))
            {
                foreach (var unknownScanner in unknownScanners)
                {
                    if (!MatchTransformAndSetPosition(scanner, unknownScanner)) continue;
                    unknownScanners.Remove(unknownScanner);
                    Console.WriteLine(unknownScanners.Count);
                    break;
                }

            }
        }

        return _scanners.SelectMany(s => s.Beacons.Select(b => b )).Distinct().Count();
    }

    private bool MatchTransformAndSetPosition(Scanner scanner, Scanner scanner2)
    {
        foreach (var t in _transformations)
        {
            var scannerPos = CheckOverlap(scanner.Beacons, scanner2.Transform(t).Beacons);
            if (scannerPos == null) continue;
            scanner2.Apply(t);
                
            scanner2.Position = scannerPos;
            for (int i = 0; i < scanner2.Beacons.Count; i++)
            {
                scanner2.Beacons[i] += scannerPos;
            }
            return true;
        }

        return false;
    }

    private Point3D CheckOverlap(List<Point3D> p1, List<Point3D> p2)
    {
        foreach (var p1p in p1)
        {
            var points1 = p1.Select(p => p - p1p).ToHashSet();
            foreach (var p2p in p2)
            {
                var points2 = p2.Select(p => p - p2p).ToHashSet();
                var intersect = points1.Intersect(points2);
                if (intersect.Count() >= 12) return  p1p -p2p;
            }
        }

        return null;
    }
    
    public override object ExecutePart2()
    {
        var max = 0;
        for (int i = 0; i < _scanners.Count; i++)
        {
            for (int j = i + 1; j < _scanners.Count; j++)
            {

                var md = Math.Abs(_scanners[i].Position.X - _scanners[j].Position.X) +
                         Math.Abs(_scanners[i].Position.Y - _scanners[j].Position.Y) +
                         Math.Abs(_scanners[i].Position.Z - _scanners[j].Position.Z);
                if (md > max) max = md;
            }
        }

        return max;
    }
}

public enum SwapMode
{
    X1,X2,X3,X4,
    Y1,Y2,Y3,Y4,
    Z1,Z2,Z3,Z4,
}