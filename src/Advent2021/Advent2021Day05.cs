using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day05 : Solution
{
    private Board<int> _board;
    private List<List<int>> _vents;
    private int _xMax;
    private int _yMax;

    public Advent2021Day05()
    {
        Answer1 = 6267;
        Answer2 = 20196;
        ReadData();
    }

    private void ReadData()
    {
        var strings = DataFile.ReadAllKeyValuePairs<string, string>(" -> ");
        _vents = strings.Select(s => s.key.SplitToType<int>(",").Concat(s.value.SplitToType<int>(",")).ToList())
            .ToList();
        _xMax = _vents.SelectMany(v => new[] { v[0], v[2] }).Max();
        _yMax = _vents.SelectMany(v => new[] { v[1], v[3] }).Max();
    }

    private static void PlotLine(List<int> vent, Board<int> board)
    {
        var xs = Functions.Range(vent[0], vent[2]).ToList();
        var ys = Functions.Range(vent[1], vent[3]).ToList();
        if (xs.Count == 1)
            foreach (var y in ys)
                board[xs[0], y]++;
        else if (ys.Count == 1)
            foreach (var x in xs)
                board[x, ys[0]]++;
        else
            for (var i = 0; i < Math.Max(xs.Count, ys.Count); i++)
                board[xs[i], ys[i]]++;
    }

    public override object ExecutePart1()
    {
        _board = new Board<int>(_xMax + 1, _yMax + 1);
        foreach (var vent in _vents.Where(vent => vent[0] == vent[2] || vent[1] == vent[3])) PlotLine(vent, _board);

        return _board.Count(v => v > 1);
    }

    public override object ExecutePart2()
    {
        _board = new Board<int>(_xMax + 1, _yMax + 1);
        foreach (var vent in _vents) PlotLine(vent, _board);

        return _board.Count(v => v > 1);
    }
}