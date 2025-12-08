using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day07 : Solution
{
    private readonly Dictionary<BeamAndRow, long> _options = new();

    public Advent2025Day07()
    {
        Answer1 = 1698;
        Answer2 = 95408386769474L;
    }

    public override object ExecutePart1()
    {
        var board = DataFile.LoadBoard<char>();
        var beams = new HashSet<int>
        {
            board.GetRow(0).IndexOfFirst(r => r == 'S')
        };
        var count = 0;
        var rows = board.GetRows().Select(s => s.ToList()).Skip(1).ToList();
        for (var i = 0; i < rows.Count - 1; i++)
            foreach (var beam in beams.ToList().Where(beam => rows[i + 1][beam] == '^'))
            {
                count++;
                beams.Remove(beam);
                beams.Add(beam + 1);
                beams.Add(beam - 1);
            }

        return count;
    }

    private long CountRoutes(List<List<char>> rows, BeamAndRow current)
    {
        if (current.Row == rows.Count - 1) return 1;
        long count = 0;
        if (_options.TryGetValue(current, out var c)) return c;

        if (rows[current.Row + 1][current.Beam] == '^')
        {
            count += GetCount(new BeamAndRow(current.Beam + 1, current.Row + 1));
            count += GetCount(new BeamAndRow(current.Beam - 1, current.Row + 1));
        }
        else
        {
            count += GetCount(new BeamAndRow(current.Beam, current.Row + 1));
        }

        return count;

        long GetCount(BeamAndRow beamAndRow)
        {
            var cnt = CountRoutes(rows, beamAndRow);
            _options.TryAdd(beamAndRow, cnt);
            return cnt;
        }
    }

    public override object ExecutePart2()
    {
        var board = DataFile.LoadBoard<char>();
        var start = board.GetRow(0).IndexOfFirst(r => r == 'S');
        var rows = board.GetRows().Select(s => s.ToList()).Skip(1).ToList();
        return CountRoutes(rows, new BeamAndRow(start, 0));
    }

    private class BeamAndRow(int beam, int row)
    {
        public readonly int Beam = beam;
        public readonly int Row = row;


        private bool Equals(BeamAndRow other)
        {
            return Beam == other.Beam && Row == other.Row;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((BeamAndRow)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Beam, Row);
        }

        public override string ToString()
        {
            return Beam.ToString();
        }
    }
}