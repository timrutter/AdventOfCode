using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020;

public class Advent2020Day07 : Solution
{
    public Advent2020Day07()
    {
        Answer1 = 101;
        Answer2 = 108636;
    }

    private static void GetAllParents(HashSet<string> hashset, List<Bag> dict, string name)
    {
        var bags = FindBags(dict, name).ToList();
        foreach (var bag in bags)
        foreach (var allParent in bag.GetAllParents())
        {
            if (hashset.Contains(allParent.Color)) continue;
            hashset.Add(allParent.Color);
            GetAllParents(hashset, dict, allParent.Color);
        }
    }

    private static List<Bag> FindBags(List<Bag> dict, string color)
    {
        return dict.SelectMany(d => d.FindBags(color)).Where(b => b != null).ToList();
    }

    private static string CleanColour(string str)
    {
        return Regex.Replace(str, " bag.*", "").Trim();
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAllKeyValuePairs("contain");
        var dict = new List<Bag>();
        foreach (var line in lines)
        {
            var list = new List<(int, string)>();

            if (line.value != " no other bags.")
            {
                var bits = line.value.Split(",");
                foreach (var bit in bits)
                    list.Add((int.Parse(bit.Substring(1, 2)), CleanColour(bit[2..])));
            }

            var color = CleanColour(line.key);
            var b = FindBags(dict, color);
            if (b.Count == 0)
                dict.Add(new Bag(null, CleanColour(line.key), list));
            else
                foreach (var bag in b)
                    bag.Children.AddRange(list.Select(l => new Bag(bag, l.Item2, null, l.Item1)));
        }


        var hashset = new HashSet<string>();
        GetAllParents(hashset, dict, "shiny gold");
        return hashset.Count;
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAllKeyValuePairs("contain");
        var dict = new Dictionary<string, Bag>();
        foreach (var line in lines)
        {
            var list = new List<(int, string)>();

            if (line.value != " no other bags.")
            {
                var bits = line.value.Split(",");
                foreach (var bit in bits)
                    list.Add((int.Parse(bit.Substring(1, 2)), CleanColour(bit[2..])));
            }

            var bagName = CleanColour(line.key);
            var bag = dict.GetOrAdd(bagName, () => new Bag(null, bagName, null));

            foreach (var l in list)
            {
                Bag childBag;
                if (dict.ContainsKey(l.Item2))
                {
                    childBag = dict[l.Item2];
                }
                else
                {
                    childBag = new Bag(bag, l.Item2, null, l.Item1);
                    dict.Add(l.Item2, childBag);
                }

                for (var i = 0; i < l.Item1; i++)
                    bag.Children.Add(childBag);
            }
        }


        return dict["shiny gold"].CountBags() - 1;
    }
}