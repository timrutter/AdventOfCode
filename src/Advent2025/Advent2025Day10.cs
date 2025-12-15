using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day10 : Solution
{
    private static readonly Dictionary<int, List<ListOfInts>> Cache = new();

    public Advent2025Day10()
    {
        Answer1 = 473;
        Answer2 = 18681;
    }

    public override object ExecutePart1()
    {
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
        var lines = DataFile.ReadAll<string>().ToList();
        var count = 0;
        foreach (var line in lines)
        {
            var bits = line.Split(" ");
            var joltages =
                new ListOfInts(bits.Last()[1..^1].SplitToType<int>(",").ToList());
            var bs = bits.Skip(1).Take(bits.Length - 2).ToList();
            var buttons = bs.Select(b => b[1..^1].SplitToType<int>(","))
                .Select(b => Enumerable.Range(0, joltages.Values.Count)
                    .Select(i =>b.Contains(i) ? 1 : 0).ToList()).ToList();
            //Console.WriteLine(string.Join(",", joltages));
            //Console.WriteLine(string.Join(" | ",  buttons.Select(b => string.Join(",", b))));
            var answer = Solver.SolveSingle(buttons.Select(b => new ListOfInts(b)).ToList(), joltages);
            //Console.WriteLine(answer);
            count += answer;
        }

        return count;
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

        public ListOfInts Clone()
        {
            return new ListOfInts(Values.ToList());

        }

        public override string ToString()
        {
            return string.Join(",", Values);
        }
    }


    public static class Solver
    {
        private static Dictionary<ListOfInts, int> Patterns(List<ListOfInts> coeffs)
        {
            var output = new Dictionary<ListOfInts, int>();

            var numButtons = coeffs.Count;
            var numVars = coeffs[0].Values.Count;

            for (var patternLen = 0; patternLen <= numButtons; patternLen++)
            {
                foreach (var buttons in Combinations(numButtons, patternLen))
                {
                    var pattern = new ListOfInts(Enumerable.Repeat(0,numVars).ToList());
                    foreach (var b in buttons.Values)
                    {
                        for (var i = 0; i < coeffs[b].Values.Count; i++)
                            pattern.Values[i] += coeffs[b].Values[i];
                    }

                    output.TryAdd(pattern, patternLen);
                }
            }

            return output;
        }

        public static int SolveSingle(List<ListOfInts> coeffs, ListOfInts goal)
        {
            var patternCosts = Patterns(coeffs);
            var cache = new Dictionary<ListOfInts, int>();

            int SolveSingleAux(ListOfInts g)
            {
                if (g.Values.All(x => x == 0)) return 0;

                if (cache.TryGetValue(g, out var cached))
                    return cached;

                var answer = 1_000_000;

                foreach (var kv in patternCosts)
                {
                    var pattern = kv.Key;
                    var cost = kv.Value;

                    var ok = !g.Values.Where((t, i) => pattern.Values[i] > t || (pattern.Values[i] & 1) != (t & 1)).Any();

                    if (!ok) continue;

                    var newGoal = new ListOfInts( Enumerable.Repeat(0,g.Values.Count).ToList());
                    for (var i = 0; i < g.Values.Count; i++)
                        newGoal.Values[i] = (g.Values[i] - pattern.Values[i]) / 2;

                    answer = Math.Min(answer, cost + 2 * SolveSingleAux(newGoal));
                }

                cache[g] = answer;
                return answer;
            }

            return SolveSingleAux(goal);
        }


        static IEnumerable<ListOfInts> Combinations(int numButtons, int patternLen)
        {
            var result = new ListOfInts(Enumerable.Repeat(0,patternLen).ToList());

            IEnumerable<ListOfInts> Recurse(int start, int depth)
            {
                if (depth == patternLen)
                {
                    yield return result.Clone();
                    yield break;
                }

                for (var i = start; i < numButtons; i++)
                {
                    result.Values[depth] = i;
                    foreach (var r in Recurse(i + 1, depth + 1))
                        yield return r;
                }
            }

            return Recurse(0, 0);
        }
    }
}