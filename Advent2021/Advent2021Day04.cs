using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Functions;

namespace AdventOfCode.Advent2021
{
    public class Advent2021Day04 : Solution
    {
        public Advent2021Day04()
        {
            Answer1 = 25023;
            Answer2 = 2634;
        }

        private static bool IsWinner(Board<int> board)
        {
            for (int y = 0; y < 5; y++)
            {
                bool isDone = true;
                for (int x = 0; x < 5; x++)
                {
                    if (board.ValueAt(x, y) != -1)
                    {
                        isDone = false;
                        break;
                    }
                }

                if (isDone) return true;
            }
            for (int x = 0; x < 5; x++)
            {
                bool isDone = true;
                for (int y = 0; y < 5; y++)
                {
                    if (board.ValueAt(x, y) != -1)
                    {
                        isDone = false;
                        break;
                    }
                }

                if (isDone) return true;
            }

            return false;
        }

        private static int CalcResult(Board<int> board)
        {
            int res = 0;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (board.ValueAt(x, y) != -1)
                    {
                        res += board.ValueAt(x, y);
                    }
                }
            }

            return res;
        }

        private static List<Board<int>> LoadBoards(List<string> origStrings)
        {
            var boards = new List<Board<int>>();
            for (int i = 1; i < origStrings.Count; i += 6)
            {
                boards.Add(new Board<int>(5, 5, int.MaxValue));
                boards[i / 6] = origStrings.Skip(i + 1).Take(5).LoadBoard(" ", int.MaxValue);
            }

            return boards;
        }

        private static void CrossOutNumIfPresent(Board<int> board, int num)
        {
            foreach (var _ in board.Traverse)
            {
                if (board.Value == num)
                    board.Value = -1;
            }
        }
        public override object ExecutePart1()
        {
            var origStrings = DataFile.ReadAll<string>().Select(s => s.Trim().Replace("  ", " ")).ToList();
            var nums = origStrings[0].SplitToType<int>(",").ToList();
            var boards = LoadBoards(origStrings);
            
            foreach (var t in nums)
            {
                foreach (var board in boards)
                {
                    CrossOutNumIfPresent(board, t);
                    if (IsWinner(board)) return CalcResult(board) * t;
                }
            }
            return int.MaxValue;
        }

        public override object ExecutePart2()
        {
            var origStrings = DataFile.ReadAll<string>().Select(s => s.Trim().Replace("  ", " ")).ToList();
            var nums = origStrings[0].SplitToType<int>(",").ToList();
            var boards = LoadBoards(origStrings);
           
            foreach (var t in nums)
            {
                foreach (var board in boards.ToList())
                {
                    CrossOutNumIfPresent(board, t);

                    if (!IsWinner(board)) continue;
                    boards.Remove(board);
                    if (boards.Count == 0)
                    {
                        return CalcResult(board) * t;
                    }
                }
            }
            return int.MaxValue;
        }
    }
}