using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Advent2020;

public class Bag
{
    #region Constructors

    public Bag(Bag parent, string color, List<(int, string)> children, int count = 1)
    {
        Parent = parent;
        Color = color;
        Count = count;
        Children = children?.Select(c => new Bag(this, c.Item2, null, c.Item1))
            .ToList() ?? [];
    }

    #endregion

    #region Properties

    public Bag Parent { get; }
    public string Color { get; }
    public int Count { get; }
    public List<Bag> Children { get; }

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"{Count} {Color}";
    }

    public List<Bag> FindBags(string color)
    {
        var ret = new List<Bag>();
        if (Color == color)
            ret.Add(this);
        ret.AddRange(Children.SelectMany(c => c.FindBags(color)).Where(b => b != null));
        return ret;
    }

    public List<Bag> GetAllParents()
    {
        var ret = new List<Bag>();

        var p = Parent;
        while (p != null)
        {
            ret.Add(p);
            p = p.Parent;
        }

        return ret;
    }

    public void Dump()
    {
        var ps = GetAllParents();
        var s = ToString();
        Console.WriteLine($"{s}".PadLeft(ps.Count * 2 + s.Length));
        foreach (var child in Children) child.Dump();
    }

    public int CountBags()
    {
        var count = 1;
        foreach (var child in Children) count += child.CountBags();

        return count;
    }

    #endregion
}