using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day04 : Solution
{
    public Advent2021Day04()
    {
        Answer1 = 25023;
        Answer2 = 2634;
    }

    private static bool IsWinner(Board<int> board)
    {
        return board.GetRows().Any(r => r.All(v => v == -1)) ||
               board.GetColumns().Any(c => c.All(v => v == -1));
    }

    private static int CalcResult(Board<int> board)
    {
        return board.Values.Where(v => v != -1).Sum();
    }

    private static List<Board<int>> LoadBoards(List<string> origStrings)
    {
        var boards = new List<Board<int>>();
        for (var i = 1; i < origStrings.Count; i += 6)
        {
            boards.Add(new Board<int>(5, 5));
            boards[i / 6] = origStrings.Skip(i + 1).Take(5).LoadBoard<int>(" ");
        }

        return boards;
    }

    private static void CrossOutNumIfPresent(Board<int> board, int num)
    {
        foreach (var (pos, _) in board.ValuesAndPositions.Where(v => v.value == num)) board.SetValueAt(pos, -1);
    }

    public override object ExecutePart1()
    {
        var origStrings = DataFile.ReadAll<string>().Select(s => s.Trim().Replace("  ", " ")).ToList();
        var nums = origStrings[0].SplitToType<int>(",").ToList();
        var boards = LoadBoards(origStrings);

        foreach (var t in nums)
        foreach (var board in boards)
        {
            CrossOutNumIfPresent(board, t);
            if (IsWinner(board)) return CalcResult(board) * t;
        }

        return int.MaxValue;
    }

    public override object ExecutePart2()
    {
        var origStrings = DataFile.ReadAll<string>().Select(s => s.Trim().Replace("  ", " ")).ToList();
        var nums = origStrings[0].SplitToType<int>(",").ToList();
        var boards = LoadBoards(origStrings);

        foreach (var t in nums)
        foreach (var board in boards.ToList())
        {
            CrossOutNumIfPresent(board, t);

            if (!IsWinner(board)) continue;
            boards.Remove(board);
            if (boards.Count == 0) return CalcResult(board) * t;
        }

        return int.MaxValue;
    }
}