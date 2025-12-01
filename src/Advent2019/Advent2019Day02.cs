using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2019;

public class Advent2019Day02 : Solution
{
    public Advent2019Day02()
    {
        Answer1 = 2890696;
        Answer2 = 8226;
    }

    public override object ExecutePart1()
    {
        var ints = File.ReadAllText(DataFile).SplitToType<int>(",").ToList();
        ints[1] = 12;
        ints[2] = 2;
        return ints.CreateAndExecuteOpcodeComputer().Memory[0];
    }

    public override object ExecutePart2()
    {
        var intsStart = File.ReadAllText(DataFile).SplitToType<int>(",").ToList();
        for (var noun = 0; noun <= 99; noun++)
        for (var verb = 0; verb <= 99; verb++)
        {
            intsStart[1] = noun;
            intsStart[2] = verb;
            var ret = intsStart.CreateAndExecuteOpcodeComputer().Memory[0];
            if (ret == 19690720)
                return 100 * noun + verb;
        }

        return -1;
    }
}