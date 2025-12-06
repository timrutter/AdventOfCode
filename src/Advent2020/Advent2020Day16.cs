using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020;

public class Advent2020Day16 : Solution
{
    public Advent2020Day16()
    {
        Answer1 = 21071;
        Answer2 = 3429967441937;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>().ToList();
        var rules = lines.TakeWhile(s => !string.IsNullOrEmpty(s)).ToList();
        var nearbyTickets = lines.Skip(rules.Count + 6).Select(s => s.SplitToType<int>(",")).ToList();

        Dictionary<string, List<(int min, int max)>> dict = rules.ToDictionary(s => s.Split(":")[0], s =>
        {
            return s.Split(": ")[1].Split(" or ")
                .Select(b =>
                {
                    var bits2 = b.Split("-");
                    return (int.Parse(bits2[0]), int.Parse(bits2[1]));
                }).ToList();
        });
        return nearbyTickets.SelectMany(nt =>
            nt.Where(n => dict.All(r => r.Value.All(range => n < range.min || n > range.max)))).Sum();
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>().ToList();
        var rules = lines.TakeWhile(s => !string.IsNullOrEmpty(s)).ToList();
        var myTicket = lines[rules.Count + 2].SplitToType<int>(",").ToList();
        var nearbyTickets = lines.Skip(rules.Count + 5).Select(s => s.SplitToType<int>(",").ToList()).ToList();
        nearbyTickets.Insert(0, myTicket);
        Dictionary<string, List<Range>> dict = rules.ToDictionary(s => s.Split(":")[0], s =>
        {
            return s.Split(": ")[1].Split(" or ")
                .Select(b =>
                {
                    var bits2 = b.SplitToType<int>("-").ToList();
                    return new Range (bits2[0], bits2[1]);
                }).ToList();
        });
        var validNearbyTickets = nearbyTickets.Where(nt =>
            nt.All(n => dict.Any(r => r.Value.Any(range => range.InRange(n))))).ToList();
        var possibleFields = new List<List<string>>();
        for (var i = 0; i < rules.Count; i++)
        {
            var fields = validNearbyTickets.Select(v => v[i]).ToList();
            fields.Sort();
            possibleFields.Add(dict.Where(d =>
                    fields.All(f => d.Value.Any(range => range.InRange(f)))).Select(d => d.Key)
                .ToList());
        }

        var orderedFields = new Dictionary<int, string>();
        for (var i = 0; i < rules.Count; i++)
        {
            var pf = possibleFields.First(p => p.Count == i + 1);
            orderedFields.Add(possibleFields.IndexOf(pf), pf.Except(orderedFields.Values).First());
        }

        long sum = 1;
        for (var i = 0; i < orderedFields.Count; i++)
            if (orderedFields[i].StartsWith("departure"))
                sum *= myTicket[i];

        return sum;
    }
}