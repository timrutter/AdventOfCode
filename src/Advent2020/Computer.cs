using System.Collections.Generic;

namespace AdventOfCode.Advent2020;

public static class Computer
{
    #region Fields

    private const int InfiniteLoopDetected = 1;

    #endregion

    #region Methods

    public static (int acc, int err) RunProgram(List<(string key, string value)> kvps)
    {
        var acc = 0;
        var ind = 0;
        var visited = new HashSet<int>();
        while (true)
        {
            if (ind >= kvps.Count) return (acc, 0);
            if (visited.Contains(ind))
                return (acc, InfiniteLoopDetected);
            visited.Add(ind);
            switch (kvps[ind].key)
            {
                case "acc":
                    acc += int.Parse(kvps[ind].value);
                    ind++;
                    break;
                case "jmp":
                    ind += int.Parse(kvps[ind].value);
                    break;
                case "nop":
                    ind++;
                    break;
            }
        }
    }

    #endregion
}