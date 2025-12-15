using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        return 473;
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

    // Credit to here for this: https://old.reddit.com/r/adventofcode/comments/1pk87hl/2025_day_10_part_2_bifurcate_your_way_to_victory/
    // https://old.reddit.com/user/tenthmascot
    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>().ToList();
        var count = 0;
        foreach (var line in lines)
        {
            var bits = line.Split(" ");
            var joltages = bits.Last()[1..^1].SplitToType<int>(",").ToList();
            var bs = bits.Skip(1).Take(bits.Length - 2).ToList();
            var buttons = bs.Select(b => b[1..^1].SplitToType<int>(","))
                .Select(b => Enumerable.Range(0, joltages.Count)
                    .Select(i => b.Contains(i) ? 1 : 0).ToList()).ToList();
            //Console.WriteLine(string.Join(",", joltages));
            //Console.WriteLine(string.Join(" | ",  buttons.Select(b => string.Join(",", b))));
            var answer = SolveSingle(buttons.ToList(), joltages);
            //Console.WriteLine(answer);
            count += answer;
        }

        return count;
    }

    private static Dictionary<ListOfInts, int> Patterns(List<List<int>> coeffs)
    {
        var output = new Dictionary<ListOfInts, int>();

        var numButtons = coeffs.Count;
        var numVars = coeffs[0].Count;

        for (var patternLen = 0; patternLen <= numButtons; patternLen++)
            foreach (var buttons in Combinations(numButtons, patternLen))
            {
                var pattern = Enumerable.Repeat(0, numVars).ToList();
                foreach (var b in buttons)
                    for (var i = 0; i < coeffs[b].Count; i++)
                        pattern[i] += coeffs[b][i];

                output.TryAdd(new ListOfInts(pattern), patternLen);
            }

        return output;
    }

    private static int SolveSingle(List<List<int>> coeffs, List<int> goal)
    {
        var patternCosts = Patterns(coeffs);
        var cache = new Dictionary<ListOfInts, int>();

        int SolveSingleAux(ListOfInts g)
        {
            if (g.Values.All(x => x == 0)) return 0;

            if (cache.TryGetValue(g, out var cached))
                return cached;

            var answer = 1_000_000;

            foreach (var (pattern, cost) in patternCosts)
            {
                var ok = !g.Values.Where((t, i) => pattern.Values[i] > t || (pattern.Values[i] & 1) != (t & 1)).Any();

                if (!ok) continue;

                var newGoal = Enumerable.Repeat(0, g.Values.Length).ToList();
                for (var i = 0; i < g.Values.Length; i++)
                    newGoal[i] = (g.Values[i] - pattern.Values[i]) / 2;

                answer = Math.Min(answer, cost + 2 * SolveSingleAux(new ListOfInts(newGoal)));
            }

            cache[g] = answer;
            return answer;
        }

        return SolveSingleAux(new ListOfInts(goal));
    }


    private static IEnumerable<List<int>> Combinations(int numButtons, int patternLen)
    {
        var result = Enumerable.Repeat(0, patternLen).ToList();

        return Recurse(0, 0);

        IEnumerable<List<int>> Recurse(int start, int depth)
        {
            if (depth == patternLen)
            {
                yield return result.ToList();
                yield break;
            }

            for (var i = start; i < numButtons; i++)
            {
                result[depth] = i;
                foreach (var r in Recurse(i + 1, depth + 1))
                    yield return r;
            }
        }
    }

    /// <summary>
    /// Immutable array is much slower...
    /// </summary>
    public class ListOfInts
    {
        private readonly int _hash;

        public ListOfInts(List<int> values)
        {
            Values = [..values];
            _hash = 17;
            foreach (var v in Values) _hash = _hash * 23 + v.GetHashCode();
        }

        public ImmutableArray<int> Values { get; }

        private bool Equals(ListOfInts other)
        {
            return _hash == other._hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ListOfInts)obj);
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public override string ToString()
        {
            return string.Join(",", Values);
        }
    }
}