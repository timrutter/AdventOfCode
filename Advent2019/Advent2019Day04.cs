using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2019
{
    public class Advent2019Day04 : Solution
    {
        public Advent2019Day04()
        {
            Answer1 = 1686;
            Answer2 = 1145;
        }
        
        public override object ExecutePart1()
        {
            int count = 0;
            for (int i = 168630; i <= 718098; i++)
            {
                var s = i.ToString();
                if (!"0123456789".ToCharArray().Any(c => Regex.IsMatch(s, $"{c}{c}"))) continue;
                var ret = true;
                for (int j = 0; j < s.Length - 1; j++)
                {
                    if (s[j] <= s[j + 1]) continue;
                    ret = false;
                    break;
                }

                if (ret) count++;

            }
            return count;
        }

        public override object ExecutePart2()
        {
            int count = 0;
            for (int i = 168630; i <= 718098; i++)
            {
                var s = i.ToString();
                if (!"0123456789".ToCharArray()
                        .Any(c => Regex.IsMatch(s, $"^{c}{c}[^{c}]|[^{c}]{c}{c}[^{c}]|[^{c}]{c}{c}$"))) continue;
                var ret = true;
                for (int j = 0; j < s.Length - 1; j++)
                {
                    if (s[j] <= s[j + 1]) continue;
                    ret = false;
                    break;
                }

                if (ret) count++;

            }
            return count;
        }
    }
}