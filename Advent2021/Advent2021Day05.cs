using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021
{
    public class Advent2021Day05 : Solution
    {
        public Advent2021Day05()
        {
            Answer1 = 6267;
            Answer2 = 20196;
        }

        public override object ExecutePart1()
        {
            var strings = DataFile.ReadAllKeyValuePairs<string, string>(" -> ");
            var vents = strings.Select(s => s.key.SplitToType<int>(",").Concat(s.value.SplitToType<int>(",")).ToList()).ToList();
            var xMax = vents.SelectMany(v => new[] {v[0], v[2]}).Max();
            var yMax = vents.SelectMany(v => new[] { v[1], v[3] }).Max();
            var board = new Board<int>(xMax + 1, yMax + 1);
            foreach (var vent in vents)
            {
                if (vent[0] == vent[2] || vent[1] == vent[3])
                {
                    PlotLine(vent, board);
                } 
            }

            return board.Count(v => v > 1);
        }
        
        private static void PlotLine(List<int> vent, Board<int> board)
        {
            var xs = Functions.Range(vent[0], vent[2]).ToList();
            var ys = Functions.Range(vent[1], vent[3]).ToList();
            if (xs.Count == 1)
            {
                foreach(var y in ys)
                {
                    board.SetValueAt(xs[0], y, board.ValueAt(xs[0], y) + 1);
                }
            } 
            else if (ys.Count == 1)
            {
                foreach(var x in xs)
                {
                    board.SetValueAt(x, ys[0], board.ValueAt(x, ys[0]) + 1);
                }
            }
            else
            {

                for (var i = 0; i < xs.Count; i++)
                {
                    board.SetValueAt(xs[i], ys[i], board.ValueAt(xs[i], ys[i]) + 1);
                }
            }
        }

        public override object ExecutePart2()
        {
            var strings = DataFile.ReadAllKeyValuePairs<string, string>(" -> ");
            var vents = strings.Select(s => s.key.SplitToType<int>(",").Concat(s.value.SplitToType<int>(",")).ToList()).ToList();
            var xMax = vents.SelectMany(v => new[] { v[0], v[2] }).Max();
            var yMax = vents.SelectMany(v => new[] { v[1], v[3] }).Max();
            var board = new Board<int>(xMax + 1, yMax + 1);
            foreach (var vent in vents)
            {
                PlotLine(vent, board);
            }

            return board.Count(v => v > 1);
        }
    }
}