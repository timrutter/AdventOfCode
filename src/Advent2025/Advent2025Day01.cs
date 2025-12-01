using System;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day01 : Solution
{
    public Advent2025Day01()
    {
        Answer1 = null;
        Answer2 = null;
    }

    public override object ExecutePart1()
    {
        var pos = 50;
        var arr = DataFile.ReadAll<string>();
        var count = 0;
        foreach (var a in arr)
        {
            if (a[0] == 'R') pos = (pos + int.Parse(a[1..])) % 100;

            if (a[0] == 'L') pos = (pos - int.Parse(a[1..])) % 100;

            if (pos == 0) count++;
        }

        return count;
    }

    public static int GetPos(string rotStr, int pos, ref int count)
    {
        var rot = int.Parse(rotStr[1..]);
        var completeRots = rot / 100;
        count += completeRots;
        rot -= completeRots * 100;
        if (rotStr[0] == 'R' && rot > 0)
        {
            var b = pos + rot % 100;
            if (b >= 100) count++;

            return b % 100;
        }

        if (rotStr[0] != 'L' || rot <= 0) return pos;
        
        var newPos = pos - rot % 100;
        switch (newPos)
        {
            case < 0:
            {
                if (pos != 0) count++;
                return 100 + newPos;
            }
            case 0:
                count++;
                break;
        }

        return newPos ;
        

    }

    public override object ExecutePart2()
    {
        var pos = 50;
        var arr = DataFile.ReadAll<string>();
        var count = 0;
        foreach (var a in arr) pos = GetPos(a, pos, ref count);
        return count;
    }
}