using System.Collections.Generic;

namespace AdventOfCode;

public abstract class Solution
{
    protected string DataFile =>
        $"{GetType().Name[..10]}/Data/{GetType().Name.Substring(10, 5)}.txt";

    public object Answer1 { get; protected init; } = null;
    public object Answer2 { get; protected init; } = null;

    protected string GetDataFile(string suffix)
    {
        return $"{GetType().Name[..10]}/Data/{GetType().Name.Substring(10, 5)}_{suffix}.txt";
    }

    public abstract object ExecutePart1();
    public abstract object ExecutePart2();
}

public interface ISolutions
{
    List<Solution> Solutions { get; }
}