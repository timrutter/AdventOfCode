using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;
using Microsoft.Z3;

namespace AdventOfCode.Advent2025;

public class Advent2025Day10 : Solution
{
    private static readonly Dictionary<int, List<ListOfInts>> Cache = new();

    public Advent2025Day10()
    {
        Answer1 = null;
        Answer2 = null;
    }

    public override object ExecutePart1()
    {
        return 0;
        var lines = DataFile.ReadAll<string>();
        var count = 0;
        foreach (var line in lines)
        {
            var bits = line.Split(" ");
            var lights = bits[0].TrimStart("[").TrimEnd("]").ToArray().Select((c, i) => c == '#' ? i : -1)
                .Where(i => i != -1).ToList();
            var bs = bits.Skip(1).Take(bits.Length - 2).ToList();
            var buttons = bs.Select(b => b.TrimStart("(").TrimEnd(")").ToString().SplitToType<int>(",").ToList())
                .ToList();
            for (var i = 1; i < buttons.Count; i++)
            {
                var sequences = GetAllSequences(buttons, i);
                if (sequences.Any(sequence => sequence.result.SequenceEqual(lights)))
                {
                    count += i;
                    break;
                }
            }
        }


        return count;
    }

    private List<(List<int> result, int count)> GetAllSequences(List<List<int>> buttons, int iterations)
    {
        List<(List<int> result, int count)> results = [];
        if (iterations == 1) return buttons.Select(b => (b, b.Count)).ToList();
        foreach (var button in buttons)
            results.AddRange(GetAllSequences((button, 0), buttons.Except([button]).ToList(), iterations - 1));

        return results;
    }

    private List<(List<int> result, int count)> GetAllSequences((List<int> buttons, int currentCount) current,
        List<List<int>> buttons, int iterations)
    {
        List<(List<int> result, int count)> results = [];
        foreach (var button in buttons)
        {
            var newButtons = Combine(current.buttons, button);

            if (iterations == 1) results.Add((newButtons, current.currentCount + button.Count));
            else
                results.AddRange(GetAllSequences((newButtons, current.currentCount + button.Count),
                    buttons.Except([button]).ToList(), iterations - 1));
        }

        return results;
    }

    public static List<int> Combine(List<int> b1, List<int> b2)
    {
        var combined = b1.Concat(b2).ToList();
        var ret = combined.Where(i => combined.Count(j => j == i) == 1).ToList();
        ret.Sort();
        return ret;
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>();
        var sum = 0;
        foreach (var machine in DataFile.ReadAll<string>())
        {
            var parsed = machine.Split(" ");
            var buttons = parsed[1..^1];
            var joltages = parsed.Last()[1..^1];
            sum += CalcutateJoltage(joltages, buttons);
        }

        return sum;
        var count = 0;
        // foreach (var line in lines)
        // {
        //     var bits = line.Split(" ");
        //     var joltages =
        //         new ListOfInts(bits.Last().TrimStart("{").TrimEnd("}").ToString().SplitToType<int>(",").ToList());
        //     var bs = bits.Skip(1).Take(bits.Length - 2).ToList();
        //     var buttons = bs.Select(b => b.TrimStart("(").TrimEnd(")").ToString().SplitToType<int>(",").ToList())
        //         .ToList();
        //     Console.WriteLine(line);
        //     for (var i = 1; i < 100; i++)
        //     {
        //         var currentJoltages = new ListOfInts(Enumerable.Repeat(0, joltages.Values.Count).ToList());
        //         var sequences = GetIncrements(joltages, currentJoltages,
        //             buttons.Select(b => new ListOfInts(b)).ToList(), i);
        //         Console.WriteLine(i);
        //         if (sequences is null || sequences.Any(sequence => sequence is null))
        //         {
        //             count += i;
        //             break;
        //         }
        //     }
        // }

        return count;
    }

    private static List<ListOfInts> GetIncrements(ListOfInts targetVoltages, ListOfInts currentJoltages,
        List<ListOfInts> buttons, int iterations)
    {
        var buttonsTrimmed = buttons
            .Where(b => AllLess(b.Values, targetVoltages.Values, currentJoltages.Values)).ToList();
        var hash = GetHashCode(buttons, iterations);
        if (Cache.TryGetValue(hash, out var increments))
            return increments;
        List<ListOfInts> results = [];
        foreach (var button in buttonsTrimmed)
        {
            var newJoltages = PressButtons(currentJoltages, button);
            //if (Exceeds(newJoltages, targetJoltages)) continue;
            //if (targetJoltages.SequenceEqual(newJoltages)) return [newJoltages];

            if (iterations == 1)
            {
                results.Add(newJoltages);
            }
            else
            {
                var incs = GetIncrements(targetVoltages, newJoltages, buttonsTrimmed.ToList(), iterations - 1);
                results.AddRange(incs.Select(s => Increment(newJoltages, s)));
            }
        }

        if (results.Any(r => r is null || Increment(currentJoltages, r).Equals(targetVoltages))) return null;
        results = results.Distinct().ToList();
        results.Sort((i1, i2) => i1.GetHashCode().CompareTo(i2.GetHashCode()));
        Cache[hash] = results;
        return results;
    }

    public static int GetHashCode(List<ListOfInts> buttons, int iterations)
    {
        unchecked
        {
            var hash = 31;
            hash = hash * 23 + iterations.GetHashCode();

            foreach (var button in buttons)
                hash *= 37 + button.GetHashCode();

            return hash;
        }
    }

    private static bool AllLess(List<int> buttons, List<int> targetJoltages, List<int> currentJoltages)
    {
        for (var i = 0; i < targetJoltages.Count; i++)
            if (buttons.Contains(i) && currentJoltages[i] + 1 > targetJoltages[i])
                return false;

        return true;
    }

    private static bool Exceeds(ListOfInts newJoltages, ListOfInts targetJoltages)
    {
        for (var i = 0; i < newJoltages.Values.Count; i++)
            if (newJoltages.Values[i] > targetJoltages.Values[i])
                return true;

        return false;
    }

    public static ListOfInts PressButtons(ListOfInts currentJoltages, ListOfInts b2)
    {
        var cj = currentJoltages.Values.ToList();
        foreach (var i in b2.Values) cj[i]++;
        return new ListOfInts(cj);
    }

    public static ListOfInts Increment(ListOfInts currentJoltages, ListOfInts increments)
    {
        var nloi = new ListOfInts(currentJoltages.Values.ToList());
        for (var i = 0; i < nloi.Values.Count; i++) nloi.Values[i] += increments.Values[i];

        return nloi;
    }

    private static int CalcutateJoltage(string joltages, string[] buttons) // using Z3
    {
        var goalJoltages = joltages.Split(",").Select(int.Parse).ToArray();
        var btns = buttons.Select(x => x[1..^1].Split(",").Select(int.Parse)).ToList();
        using var ctx = new Context();
        var variables = btns
            .Select(btn => ctx.MkIntConst(string.Join(",",btn)))
            .ToList(); // creating all variables

        var opt = ctx.MkOptimize(); // creating an optimizer
        for (var i = 0; i < goalJoltages.Length; i++)
        {
            List<IntExpr> vars = [];
            for (var j = 0; j < btns.Count; j++)
                if (btns[j].Contains(i)) // check if index is in button basically all buttons that have the index
                    vars.Add(variables[j]); // of the current joltage
            var sum = ctx.MkAdd(vars); // sum of all variables
            var constraint = ctx.MkEq(sum, ctx.MkInt(goalJoltages[i])); // check if they equal the goal
            opt.Add(constraint); //add as a constraint
        }

        var total = ctx.MkAdd(variables);
        opt.MkMinimize(total); // what we want to minimize (sum of all variables)
        foreach (var v in variables)
            opt.Add(ctx.MkGe(v, ctx.MkInt(0))); // made sure all are >= 0

        var result = opt.Check();
        var presses = 0;
        if (result != Status.SATISFIABLE) return presses;

        var model = opt.Model;
        return variables.Sum(v => ((IntNum)model.Evaluate(v)).Int); // we sum up all the numbers
    }

    public class ListOfInts(List<int> values)
    {
        public List<int> Values { get; } = values;

        protected bool Equals(ListOfInts other)
        {
            return Values.SequenceEqual(other.Values);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ListOfInts)obj);
        }

        public override int GetHashCode()
        {
            var hash = 17;
            foreach (var v in Values) hash = hash * 23 + v.GetHashCode();

            return hash;
        }
    }
}