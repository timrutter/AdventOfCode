using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020;

public class Advent2020Day11 : Solution
{
    public Advent2020Day11()
    {
        Answer1 = 2265;
        Answer2 = 2045;
    }

    public override object ExecutePart1()
    {
        var board2 = DataFile.ReadBoard<char>();
        //board2.Dump();

        int CountOccupied(Board<char> board, int x, int y)
        {
            var count = 0;
            if (board.Jump(x, y).Left().Value == '#') count++;
            if (board.Jump(x, y).Right().Value == '#') count++;
            if (board.Jump(x, y).Up().Value == '#') count++;
            if (board.Jump(x, y).Down().Value == '#') count++;
            if (board.Jump(x, y).UpLeft().Value == '#') count++;
            if (board.Jump(x, y).UpRight().Value == '#') count++;
            if (board.Jump(x, y).DownLeft().Value == '#') count++;
            if (board.Jump(x, y).DownRight().Value == '#') count++;
            return count;
        }

        var changeCount = int.MaxValue;
        while (changeCount > 0)
        {
            changeCount = 0;
            var board1 = new Board<char>(board2);

            for (var x = 0; x < board1.Width; x++)
            for (var y = 0; y < board1.Height; y++)
                switch (board1.Jump(x, y).Value)
                {
                    case '.':
                        continue;
                    case 'L' when CountOccupied(board1, x, y) == 0:
                        board2.SetValueAt(x, y, '#');
                        changeCount++;
                        break;
                    case '#' when CountOccupied(board1, x, y) >= 4:
                        board2.SetValueAt(x, y, 'L');
                        changeCount++;
                        break;
                }

            //board2.Dump();
        }

        return board2.CountValues('#');
    }

    public override object ExecutePart2()
    {
        var board2 = DataFile.ReadBoard<char>();

        int CountOccupied(Board<char> board, int x, int y)
        {
            var count = 0;

            if (board.Jump(x, y).Left().LeftWhile(c => c == '.').Value == '#') count++;
            if (board.Jump(x, y).Right().RightWhile(c => c == '.').Value == '#') count++;
            if (board.Jump(x, y).Up().UpWhile(c => c == '.').Value == '#') count++;
            if (board.Jump(x, y).Down().DownWhile(c => c == '.').Value == '#') count++;
            if (board.Jump(x, y).UpLeft().UpLeftWhile(c => c == '.').Value == '#') count++;
            if (board.Jump(x, y).UpRight().UpRightWhile(c => c == '.').Value == '#') count++;
            if (board.Jump(x, y).DownLeft().DownLeftWhile(c => c == '.').Value == '#') count++;
            if (board.Jump(x, y).DownRight().DownRightWhile(c => c == '.').Value == '#') count++;

            return count;
        }

        var changeCount = int.MaxValue;
        while (changeCount > 0)
        {
            changeCount = 0;
            var board1 = new Board<char>(board2);
            for (var x = 0; x < board1.Width; x++)
            for (var y = 0; y < board1.Height; y++)
                switch (board1.Jump(x, y).Value)
                {
                    case '.':
                        continue;
                    case 'L' when CountOccupied(board1, x, y) == 0:
                        board2.SetValueAt(x, y, '#');
                        changeCount++;
                        break;
                    case '#' when CountOccupied(board1, x, y) >= 5:
                        board2.SetValueAt(x, y, 'L');
                        changeCount++;
                        break;
                }

            //board2.Dump();
        }

        return board2.CountValues('#');
    }
}