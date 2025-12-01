using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day03 : Solution
{
    public Advent2021Day03()
    {
        Answer1 = 3895776;
        Answer2 = 7928162;
    }

    public override object ExecutePart1()
    {
        var strings = DataFile.ReadAll<string>();
        var gammaRate = 0;
        var epsilonRate = 0;
        var total = strings.Length;
        var counts = new int[strings[0].Length];
        foreach (var s in strings)
            for (var i = 0; i < strings[0].Length; i++)
                if (s[i] == '1')
                    counts[i]++;

        for (var i = 0; i < strings[0].Length; i++)
            if (counts[i] >= total / 2)
                gammaRate += 1 << (strings[0].Length - i - 1);
            else
                epsilonRate += 1 << (strings[0].Length - i - 1);

        return gammaRate * epsilonRate;
    }

    public override object ExecutePart2()
    {
        var origStrings = DataFile.ReadAll<string>().ToList();

        var stringLen = origStrings[0].Length;

        char MostCommon(int i, List<string> strings)
        {
            var count = 0;
            foreach (var s in strings)
                if (s[i] == '1')
                    count++;

            return count >= strings.Count - count ? '1' : '0';
        }

        ;

        List<string> Filter(int i, char val, IEnumerable<string> strings)
        {
            return strings.Where(s => s[i] == val).ToList();
        }

        var filteredStringsogr = origStrings.ToList();
        var filteredStringscsr = origStrings.ToList();
        for (var i = 0; i < stringLen; i++)
        {
            var mc = MostCommon(i, filteredStringsogr);
            if (filteredStringsogr.Count > 1)
                filteredStringsogr = Filter(i, mc, filteredStringsogr);

            var lc = MostCommon(i, filteredStringscsr) == '1' ? '0' : '1';
            if (filteredStringscsr.Count > 1)
                filteredStringscsr = Filter(i, lc, filteredStringscsr);
            if (filteredStringscsr.Count == 1 && filteredStringsogr.Count == 1) break;
        }

        var ogr = Convert.ToInt32(filteredStringsogr[0], 2);
        var csr = Convert.ToInt32(filteredStringscsr[0], 2);
        return ogr * csr;
    }
}