using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day04 : Solution
{
    public Advent2025Day04()
    {
        Answer1 = 1551;
        Answer2 = 9784;
    }

    public override object ExecutePart1()
    {
        var board = DataFile.LoadBoard<char>();
        var flcount = 0;
        foreach (var boardPosition in board.Traverse)
        {
            board.SetPosition(boardPosition.X, boardPosition.Y);
            if (board.Value == '.') continue;
            if (board.ValuesAround().Count(v => v == '@') >= 4) continue;
            flcount++;
        }

        return flcount;
    }

    public override object ExecutePart2()
    {
        var board = DataFile.LoadBoard<char>();
        while (true)
        {
            var changed = false;
            foreach (var _ in board.Traverse)
            {
                if (board.Value is '.' or 'x') continue;
                if (board.ValuesAround().Count(v => v == '@') >= 4) continue;
                board.SetValueAtCurrent('x');
                changed = true;
            }

            if (!changed) break;
        }

        return board.CountValues('x');
    }
}