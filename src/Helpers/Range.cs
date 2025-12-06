using System;
using System.Collections.Generic;

namespace AdventOfCode.Helpers;

public class Range : IEquatable<Range>
{
    public Range(long min, long max)
    {
        Min = min;
        Max = max;
    }

    public long Min { get; }
    public long Max { get; }

    public bool Equals(Range other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Min == other.Min && Max == other.Max;
    }


    public override string ToString()
    {
        return $"min: {Min} max: {Max}";
    }

    public bool Overlaps(Range range)
    {
        return !(Min > range.Max || Max < range.Min);
    }

    public Range CombineRanges(Range range)
    {
        return new Range(Math.Min(Min, range.Min), Math.Max(Max, range.Max));
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Range)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Min, Max);
    }

    public long Count()
    {
        return Max - Min + 1;        
    }

    public bool InRange(long value)
    {
        return value >= Min && value <= Max;
    }
}

public static class RangeExtensions
{
    public static List<Range> CombineRanges(this List<Range> ranges)
    {
        var newRanges = new List<Range>();
        foreach (var r in ranges)
        {
            var range = r;
            for (var i = 0; i < newRanges.Count; i++)
            {
                var range2 = newRanges[i];
                if (!range2.Overlaps(range)) continue;
                newRanges.RemoveAt(i);
                i--;
                range = range2.CombineRanges(range);
            }

            var inserted = false;
            for (var i = 0; i < newRanges.Count; i++)
            {
                if (range.Min >= newRanges[i].Min) continue;
                newRanges.Insert(i, range);
                inserted = true;
                break;
            }

            if (!inserted) newRanges.Add(range);
        }

        return newRanges;
    }
    
}