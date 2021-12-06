using System.IO;
using System.Linq;

namespace AdventOfCode.Advent2015
{
    public class Advent2015Day01 : Solution
    {
        public Advent2015Day01()
        {
            Answer1 = 232;
            Answer2 = 1783;
        }
        public override object ExecutePart1()
        {
            var data = File.ReadAllText(DataFile);
            return data.Count(c => c == '(') - data.Count(c => c == ')');
        }

        public override object ExecutePart2()
        {
            var data = File.ReadAllText(DataFile);
            int count = 0;
            for (var index = 0; index < data.Length; index++)
            {
                var c = data[index];
                switch (c)
                {
                    case '(':
                        count++;
                        break;
                    case ')':
                        count--;
                        break;
                }
                if (count < 0)
                    return index + 1;
            }

            return int.MaxValue;
        }
    }
}