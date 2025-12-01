using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day06 : Solution
{
    public Advent2021Day06()
    {
        Answer1 = 352151;
        Answer2 = 1601616884019;
    }

    public override object ExecutePart1()
    {
        var fish = File.ReadAllText(DataFile).SplitToType<int>(",").ToList();
        const int days = 80;
        for (var i = 0; i < days; i++)
        {
            var c = fish.Count;
            for (var j = 0; j < c; j++)
            {
                fish[j]--;
                if (fish[j] != -1) continue;
                fish.Add(8);
                fish[j] = 6;
            }
        }

        return fish.Count;
    }

    public override object ExecutePart2()
    {
        var fish = File.ReadAllText(DataFile).SplitToType<int>(",").ToList();
        var fishDictionary = Functions.Range(-1, 8).ToDictionary(i => i, i => (long)fish.Count(j => i == j));

        for (var i = 0; i < 256; i++)
        {
            for (var j = 0; j <= 8; j++)
                fishDictionary[j - 1] = fishDictionary[j];

            fishDictionary[8] = fishDictionary[-1];
            fishDictionary[6] += fishDictionary[-1];
            fishDictionary[-1] = 0;
        }

        return fishDictionary.Values.Sum();
    }
}