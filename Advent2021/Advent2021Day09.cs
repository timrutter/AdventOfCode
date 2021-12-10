using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers; 
namespace AdventOfCode.Advent2021
{
    public class Advent2021Day09 : Solution
    {
        private readonly Board<int> _board;
        private readonly IEnumerable<(int x, int y)> _lowPoints;

        public Advent2021Day09()
        {
            Answer1 = 572;
            Answer2 = 847044;
            _board = DataFile.LoadBoard<int>();
            _lowPoints = _board.Positions.Where(p => (!_board.TryGetValueAt(p.Above(), out var val) ||
                                                      val > _board.ValueAt(p)) &&
                                                     (!_board.TryGetValueAt(p.Below(), out val) ||
                                                      val > _board.ValueAt(p)) &&
                                                     (!_board.TryGetValueAt(p.Left(), out val) ||
                                                      val > _board.ValueAt(p)) &&
                                                     (!_board.TryGetValueAt(p.Right(), out val) ||
                                                      val > _board.ValueAt(p)));
        }
        
        public override object ExecutePart1()
        { 
            return _lowPoints.Sum(p => _board.ValueAt(p.x, p.y) + 1);
        }

        public override object ExecutePart2()
        {
            var basin = _lowPoints.Select(pos =>
            {
                var basin = new List<(int x, int y)>();
                FindBasin(_board, pos, basin);
                return basin.Count;
            }).ToList();
            basin.Sort();
            basin.Reverse();
            return basin[0] * basin[1] * basin[2];
        }

        private static void FindBasin(Board<int> board, (int x, int y) p, List<(int x, int y)> positions)
        {
            var newPoses = new []{p.Above(), p.Below(), p.Left(), p.Right()};
            foreach (var newPos in newPoses)
            {
                if (!board.TryGetValueAt(newPos, out var val) || val == 9 || positions.Contains(newPos)) continue;
                positions.Add(newPos);
                FindBasin(board, newPos, positions);
            }
        }
    }
}