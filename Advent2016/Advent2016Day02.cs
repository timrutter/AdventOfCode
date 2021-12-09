using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2016;

public class Advent2016Day02 : Solution
{
    public Advent2016Day02()
    {
        Answer1 = 52981;
        Answer2 = "74CD2";
    }

    public override object ExecutePart1()
    {
        var code = "";
        string[] lines = File.ReadAllLines(DataFile);
        var board = new Board<int>(3, 3);
        board.SetValues(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9}.ToList());
        board.SetPosition(1, 1);
        foreach (string line in lines)
        {
            foreach (char c in line)
                switch (c)
                {
                    case 'U':
                        board.MoveUp();
                        if (!board.PositionIsValid) board.MoveDown();
                        break;
                    case 'D':
                        board.MoveDown();
                        if (!board.PositionIsValid) board.MoveUp();
                        break;
                    case 'R':
                        board.MoveRight();
                        if (!board.PositionIsValid) board.MoveLeft();
                        break;
                    case 'L':
                        board.MoveLeft();
                        if (!board.PositionIsValid) board.MoveRight();
                        break;
                }

            code += board.Value.ToString();
        }

        return int.Parse(code);
    }

    public override object ExecutePart2()
    {
        var code = "";
        string[] lines = File.ReadAllLines(DataFile);
        var board = new Board<char>(5,5);
        board.SetValues(new[]
        {
            '0', '0', '1', '0', '0',
            '0', '2', '3', '4', '0',
            '5', '6', '7', '8', '9',
            '0', 'A', 'B', 'C', '0',
            '0', '0', 'D', '0', '0'
        }.ToList());
        board.SetPosition(0, 2);
        foreach (string line in lines)
        {
            foreach (char c in line)
                switch (c)
                {
                    case 'U':
                        board.MoveUp();
                        if (!board.PositionIsValid || board.Value == '0') board.MoveDown();
                        break;
                    case 'D':
                        board.MoveDown();
                        if (!board.PositionIsValid || board.Value == '0') board.MoveUp();
                        break;
                    case 'R':
                        board.MoveRight();
                        if (!board.PositionIsValid || board.Value == '0') board.MoveLeft();
                        break;
                    case 'L':
                        board.MoveLeft();
                        if (!board.PositionIsValid || board.Value == '0') board.MoveRight();
                        break;
                }

            code += board.Value;
        }

        return code;
    }
}