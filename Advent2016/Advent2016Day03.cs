using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2016
{
    public class Advent2016Day03 : Solution
    {
        public Advent2016Day03()
        {
            Answer1 = 917;
            Answer2 = 1649;
        }
        public override object ExecutePart1()
        {
            var triangles = DataFile.ReadAllFromFileAndSplitToType<int>(" ", StringSplitOptions.RemoveEmptyEntries );
            return triangles.Count(t => 
                t[0] + t[1] > t[2] &&
                t[0] + t[2] > t[1] &&
                t[1] + t[2] > t[0]);
        }

        public override object ExecutePart2()
        {
            var triangles = new List<List<int>>();
            var board = DataFile.ReadBoard<int>(" ", StringSplitOptions.RemoveEmptyEntries);
            var columns = board.GetColumns().ToList();
            foreach (var t in columns)
            {
                triangles.AddRange(t.TakeBlocks(3));
            }
            return triangles.Count(t =>
                t[0] + t[1] > t[2] &&
                t[0] + t[2] > t[1] &&
                t[1] + t[2] > t[0]);
        }
    }
}