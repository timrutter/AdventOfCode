using System.Collections.Generic;

namespace AdventOfCode.Advent2025;

public class Advent2025 : ISolutions
{
    public Advent2025()
    {
        Solutions.Add(new Advent2025Day01());
        Solutions.Add(new Advent2025Day02());
        Solutions.Add(new Advent2025Day03());
        Solutions.Add(new Advent2025Day04());
        Solutions.Add(new Advent2025Day05());
        Solutions.Add(new Advent2025Day06());
        Solutions.Add(new Advent2025Day07());
        Solutions.Add(new Advent2025Day08());
        Solutions.Add(new Advent2025Day09());
        Solutions.Add(new Advent2025Day10());
        Solutions.Add(new Advent2025Day11());
        Solutions.Add(new Advent2025Day12());
    }

    public List<Solution> Solutions { get; } = [];
}