using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Helpers; 
namespace AdventOfCode.Advent2021
{
    public class Advent2021Day15 : Solution
    {
        public Advent2021Day15()
        {
            Answer1 = 707;
            Answer2 = 2942;
        }
        public override object ExecutePart1()
        {
            var board = DataFile.ReadBoard<int>();
             var ret = new Dijkstra(board, board.TopLeft) ;

             Console.WriteLine(string.Join("|", ret.GetRoute(board.BottomRight)));
            return ret.Weightings[board.BottomRight];
        }

       

        public override object ExecutePart2()
        {
            var board = DataFile.ReadBoard<int>();
            
            var board2 = new Board<int>(board.Width * 5, board.Height * 5, 
                (point, i) =>
                {
                    var tilex = point.X / board.Width;
                    var tiley = point.Y / board.Height;
                    var val = board.ValueAt(point.X % board.Width, point.Y % board.Height) + tilex + tiley;
                    return (val - 1) % 9 + 1;
                });

            var ret = new Dijkstra(board2, board2.TopLeft);
            return ret.Weightings[board2.BottomRight];
        }
    }
}