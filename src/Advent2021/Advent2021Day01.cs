using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day01 : Solution
{
    public Advent2021Day01()
    {
        Answer1 = 1676;
        Answer2 = 1706;
    }

    public override object ExecutePart1()
    {
        var arr = DataFile.ReadAll<int>();
        var count = 0;
        for (var i = 1; i < arr.Length; i++)
            if (arr[i] > arr[i - 1])
                count++;

        return count;
    }

    public override object ExecutePart2()
    {
        var arr = DataFile.ReadAll<int>();
        var count = 0;
        for (var i = 3; i < arr.Length; i++)
            if (arr[i] + arr[i - 1] + arr[i - 2] > arr[i - 1] + arr[i - 2] + arr[i - 3])
                count++;

        return count;
    }
}