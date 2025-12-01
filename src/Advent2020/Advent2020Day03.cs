using System.IO;

namespace AdventOfCode.Advent2020;

public class Advent2020Day03 : Solution
{
    public Advent2020Day03()
    {
        Answer1 = 189;
        Answer2 = 1718180100;
    }

    private static int Puzzle3Part1Internal(string[] trees, int xInc, int yInc)
    {
        var count = 0;
        var x = 0;
        for (var y = 0; y < trees.Length; y += yInc)
        {
            if (trees[y][x] == '#') count++;
            x = (x + xInc) % trees[0].Length;
        }

        return count;
    }

    public override object ExecutePart1()
    {
        var trees = File.ReadAllLines(DataFile);
        return Puzzle3Part1Internal(trees, 3, 1);
    }

    public override object ExecutePart2()
    {
        var trees = File.ReadAllLines(DataFile);
        return Puzzle3Part1Internal(trees, 1, 1) *
               Puzzle3Part1Internal(trees, 3, 1) *
               Puzzle3Part1Internal(trees, 5, 1) *
               Puzzle3Part1Internal(trees, 7, 1) *
               Puzzle3Part1Internal(trees, 1, 2);
    }
}