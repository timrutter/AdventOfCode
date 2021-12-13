using System.Collections.Generic;

namespace AdventOfCode
{
    public abstract class Solution
    {
        public string DataFile =>
            $"{GetType().Name.Substring(0, 10)}\\Data\\{GetType().Name.Substring(10, 5)}.txt";
        public string GetDataFile(string suffix) =>
            $"{GetType().Name.Substring(0, 10)}\\Data\\{GetType().Name.Substring(10, 5)}_{suffix}.txt";
        public abstract object ExecutePart1();
        public abstract object ExecutePart2();

        public object Answer1 { get; protected set; } = null;
        public object Answer2 { get; protected set; } = null;
    }
    public interface Solutions
    {
        List<Solution> Solutions { get; }
    }
}