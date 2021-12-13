using System;

namespace AdventOfCode.Helpers;

public class Point
{
    protected bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }
    public static bool operator ==(Point obj1, Point obj2)
    {
        if (ReferenceEquals(obj1, obj2)) return true;
        if (obj1 is null) return false;
        return obj2 is not null && obj1.Equals(obj2);
    }

    public static bool operator != (Point obj1, Point obj2)
    {
        return !(obj1 == obj2);
    }
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Point) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; }
    public int Y { get; }
    public Point Above( int distance = 1)
    {
        return new Point(X, Y - distance);
    }
    public Point Below(int distance = 1)
    {
        return new Point(X, Y + distance);
    }
    public Point Left( int distance = 1)
    {
        return new Point(X - distance, Y);
    }
    public Point Right( int distance = 1)
    {
        return new Point(X + distance, Y);
    }

}