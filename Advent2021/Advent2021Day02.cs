using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021
{
    public class Advent2021Day02 : Solution
    {
        public Advent2021Day02()
        {
            Answer1 = 2091984;
            Answer2 = 2086261056;
        }
        public override object ExecutePart1()
        {
            var arr = DataFile.ReadAllKeyValuePairs<string, int>(" ");
            int depth = 0;
            int pos = 0;
            foreach (var keyval in arr)
            {
                switch (keyval.key)
                {
                    case "forward":
                        pos += keyval.value;
                        break;
                    case "down":
                        depth += keyval.value;
                        break;
                    case "up":
                        depth -= keyval.value;
                        break;

                }
            }
            return depth * pos;
        }

        public override object ExecutePart2()
        {
            var arr = DataFile.ReadAllKeyValuePairs<string, int>(" ");
            int depth = 0;
            int pos = 0;
            int aim = 0;
            foreach (var keyval in arr)
            {
                switch (keyval.key)
                {
                    case "forward":
                        pos += keyval.value;
                        depth += aim * keyval.value;
                        break;
                    case "down":
                        aim += keyval.value;
                        break;
                    case "up":
                        aim -= keyval.value;
                        break;

                }
            }
            return depth * pos;
        }
    }
}