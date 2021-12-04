using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Functions;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day18 : Solution
    {
        public Advent2020Day18()
        {
            Answer1 = 7293529867931;
            Answer2 = 60807587180737;
        }
        public override object ExecutePart1()
        {
            var sums = DataFile.ReadAll<string>().ToList();
            long ret = 0;
            int i;

            long DoEvaluate(string s)
            {
                long total = 0;
                var add = -1;
                for (; i < s.Length; i++)
                {
                    var c = s[i];
                    if (char.IsWhiteSpace(c)) continue;
                    if (char.IsDigit(c) && add == -1)
                    {
                        total = int.Parse(c.ToString());
                    }
                    else if (char.IsDigit(c) && add > -1)
                    {
                        if (add == 0)
                            total += int.Parse(c.ToString());
                        else total *= int.Parse(c.ToString());
                    }
                    else if (c == '+')
                    {
                        add = 0;
                    }
                    else if (c == '*')
                    {
                        add = 1;
                    }
                    else if (c == '(')
                    {
                        switch (add)
                        {
                            case -1:
                                i++;
                                total = DoEvaluate(s);
                                break;
                            case 0:
                                i++;
                                total += DoEvaluate(s);
                                break;
                            case 1:
                                i++;
                                total *= DoEvaluate(s);
                                break;
                        }
                    }
                    else if (c == ')')
                    {
                        return total;
                    }
                }

                return total;
            }

            foreach (var s in sums)
            {
                i = 0;
                var t = DoEvaluate(s);
                //Console.WriteLine(t);
                ret += t;
            }

            return ret;
        }

        public override object ExecutePart2()
        {
            var sums = DataFile.ReadAll<string>().ToList();

            static string ReplaceAdds(string s1)
            {
                return Regex.Replace(s1, @"[0-9]+( \+ [0-9]+)+",
                    match => match.Value.SplitToType<long>("+").Sum().ToString());
            }

            static long EvaluateRegex(string s1)
            {
                var s2 = s1;
                while (s2.Contains('('))
                    s2 = Regex.Replace(s2, @"\([0-9 +*]+\)",
                        match => EvaluateNoBrackets(match.Value.TrimStart('(').TrimEnd(')')).ToString());

                return EvaluateNoBrackets(s2);
            }

            static long EvaluateNoBrackets(string s)
            {
                return ReplaceAdds(s).SplitToType<long>(" * ").Product();
            }

            return sums.Sum(EvaluateRegex);
        }
    }
}