using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day11 : Solution
{
    public Advent2021Day11()
    {
        Answer1 = 1637;
        Answer2 = 242;
    }

    public override object ExecutePart1()
    {
        var octopuses = DataFile.LoadBoard<int>();
        var flashCount = 0;
        for (var i = 0; i < 100; i++)
        {
            foreach (var octopus in octopuses.ValuesAndPositions) octopuses.SetValueAt(octopus.pos, octopus.value + 1);

            var flashed = new List<Point>();
            var allDone = false;
            while (!allDone)
            {
                allDone = true;
                foreach (var octopus in octopuses.ValuesAndPositions)
                    if (ProcessFlash(octopuses, octopus, flashed))
                        allDone = false;
            }

            flashCount += flashed.Count;
        }

        return flashCount;
    }

    private bool ProcessFlash(Board<int> octopuses, (Point pos, int value) octopus, List<Point> flashed)
    {
        if (octopus.value <= 9) return false;
        var ret = false;
        octopuses.SetValueAt(octopus.pos, 0);
        flashed.Add(octopus.pos);
        var newPos = octopus.pos.Above();
        if (!flashed.Contains(newPos) && octopuses.TryGetValueAt(newPos, out var val))
        {
            ret = true;
            octopuses.SetValueAt(newPos, val + 1);
        }

        newPos = octopus.pos.Above().Right();
        if (!flashed.Contains(newPos) && octopuses.TryGetValueAt(newPos, out val))
        {
            ret = true;
            octopuses.SetValueAt(newPos, val + 1);
        }

        newPos = octopus.pos.Right();
        if (!flashed.Contains(newPos) && octopuses.TryGetValueAt(newPos, out val))
        {
            ret = true;
            octopuses.SetValueAt(newPos, val + 1);
        }

        newPos = octopus.pos.Below().Right();
        if (!flashed.Contains(newPos) && octopuses.TryGetValueAt(newPos, out val))
        {
            ret = true;
            octopuses.SetValueAt(newPos, val + 1);
        }

        newPos = octopus.pos.Below();
        if (!flashed.Contains(newPos) && octopuses.TryGetValueAt(newPos, out val))
        {
            ret = true;
            octopuses.SetValueAt(newPos, val + 1);
        }

        newPos = octopus.pos.Below().Left();
        if (!flashed.Contains(newPos) && octopuses.TryGetValueAt(newPos, out val))
        {
            ret = true;
            octopuses.SetValueAt(newPos, val + 1);
        }

        newPos = octopus.pos.Left();
        if (!flashed.Contains(newPos) && octopuses.TryGetValueAt(newPos, out val))
        {
            ret = true;
            octopuses.SetValueAt(newPos, val + 1);
        }

        newPos = octopus.pos.Above().Left();
        if (!flashed.Contains(newPos) && octopuses.TryGetValueAt(newPos, out val))
        {
            ret = true;
            octopuses.SetValueAt(newPos, val + 1);
        }

        return ret;
    }

    public override object ExecutePart2()
    {
        var octopuses = DataFile.LoadBoard<int>();
        var flashCount = 0;
        for (var i = 0; i < 500; i++)
        {
            foreach (var octopus in octopuses.ValuesAndPositions) octopuses.SetValueAt(octopus.pos, octopus.value + 1);

            var flashed = new List<Point>();
            var allDone = false;
            while (!allDone)
            {
                allDone = true;
                foreach (var octopus in octopuses.ValuesAndPositions)
                    if (ProcessFlash(octopuses, octopus, flashed))
                        allDone = false;
            }

            if (octopuses.Values.All(v => v == 0)) return i + 1;
            flashCount += flashed.Count;
        }

        return flashCount;
    }
}