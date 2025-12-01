using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day13 : Solution
{
    public Advent2021Day13()
    {
        Answer1 = 802;
        Answer2 = @"###..#..#.#..#.####.####..##..#..#.###..
#..#.#.#..#..#.#.......#.#..#.#..#.#..#.
#..#.##...####.###....#..#....#..#.###..
###..#.#..#..#.#.....#...#.##.#..#.#..#.
#.#..#.#..#..#.#....#....#..#.#..#.#..#.
#..#.#..#.#..#.#....####..###..##..###..";
    }

    public override object ExecutePart1()
    {
        var (board, folds) = ReadBoard();

        foreach (var fold in folds.Take(1)) board = Fold(board, fold);
        return board.ValuesAndPositions.Count(v => v.value == '#');
    }

    private (Board<char> board, List<(string dir, int line)> folds) ReadBoard()
    {
        var points = DataFile.ReadAllFromFileAndSplitToType<int>(",").Select(p => new Point(p[0], p[1])).ToList();
        var folds = GetDataFile("1").ReadAll<string>().Select(p =>
        {
            var s = p.Replace("fold along ", "").Split("=");
            return (dir: s[0], line: int.Parse(s[1]));
        }).ToList();
        var board = new Board<char>(
            points.Select(p => p.X).Max() + 1,
            points.Select(p => p.Y).Max() + 1,
            (p, _) => points.Contains(p) ? '#' : '.');

        return (board, folds);
    }

    private static Board<char> Fold(Board<char> board, (string dir, int line) fold)
    {
        if (fold.dir == "x")
        {
            var (board1, board2) = board.SplitAtXLine(fold.line);

            board2 = board2.FlipX();

            foreach (var (pos, value) in board2.ValuesAndPositions)
            {
                var alignedPos = new Point(pos.X + (board1.Width - board2.Width), pos.Y);
                var curval = board.ValueAt(alignedPos);
                board1.SetValueAt(pos.X, pos.Y, curval == '#' || value == '#' ? '#' : '.');
            }

            return board1;
        }
        else
        {
            var (board1, board2) = board.SplitAtYLine(fold.line);

            board2 = board2.FlipY();

            foreach (var (pos, value) in board2.ValuesAndPositions)
            {
                var alignedPos = new Point(pos.X, pos.Y + (board1.Height - board2.Height));
                var curval = board.ValueAt(alignedPos);
                board1.SetValueAt(pos.X, pos.Y, curval == '#' || value == '#' ? '#' : '.');
            }

            return board1;
        }
    }

    public override object ExecutePart2()
    {
        var (board, folds) = ReadBoard();

        foreach (var fold in folds) board = Fold(board, fold);
        return board.WriteToString();
    }
}