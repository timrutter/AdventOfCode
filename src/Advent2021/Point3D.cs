using System;
using System.Collections.Generic;

namespace AdventOfCode.Advent2021;

public class Point3D(int x, int y, int z)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Z { get; } = z;

    public double DistanceTo(Point3D p2)
    {
        var dx = X - p2.X;
        var dy = Y - p2.Y;
        var dz = Z - p2.Z;
        var dx2 = Math.Pow(dx, 2);
        var dy2 = Math.Pow(dy, 2);
        var dz2 = Math.Pow(dz, 2);
        var d = Math.Sqrt(dx2 + dy2 + dz2);
        
        return Math.Abs(d );
    }
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
        return obj.GetType() == GetType() && Equals((Point3D)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
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