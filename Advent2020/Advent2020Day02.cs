using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day02 : Solution
    {
        public Advent2020Day02()
        {
            Answer1 = 469;
            Answer2 = 267;
        }
        public override object ExecutePart1()
        {
            var kvps = DataFile.ReadAllKeyValuePairs();
            var count = 0;
            foreach (var (key, value) in kvps)
            {
                var pword = value.Trim();
                var bits = key.Split(" ");
                var letter = bits[1][0];
                bits = bits[0].Split("-");
                var min = int.Parse(bits[0]);
                var max = int.Parse(bits[1]);
                var letcount = pword.Count(c => c == letter);
                if (letcount >= min && letcount <= max)
                    count++;
            }

            return count;
        }

        public override object ExecutePart2()
        {
            var kvps = DataFile.ReadAllKeyValuePairs();
            var count = 0;
            foreach (var (key, value) in kvps)
            {
                var pword = value.Trim();
                var bits = key.Split(" ");
                var letter = bits[1][0];
                bits = bits[0].Split("-");
                var min = int.Parse(bits[0]);
                var max = int.Parse(bits[1]);

                var minChar = pword.Length >= min ? pword[min - 1] : (char)0;
                var maxChar = pword.Length >= max ? pword[max - 1] : (char)0;
                if ((minChar == letter) ^ (maxChar == letter))
                    count++;
            }

            return count;
        }
    }
}