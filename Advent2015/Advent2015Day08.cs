using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Advent2015
{
    public class Advent2015Day08 : Solution
    {
        public Advent2015Day08()
        {
            Answer1 = 1342;
            Answer2 = 2074;
        }
        private static IEnumerable<char> Unescape(string str)
        {
            str = str.Substring(1, str.Length - 2);
            for (var index = 0; index < str.Length; index++)
            {
                var c = str[index];
                if (c == '\\')
                {
                    if (str[index + 1] == 'x')
                    {
                        yield return 'a';
                        index += 3;
                    }
                    else
                    {
                        yield return 'b';
                        index++;
                    }
                }
                else yield return str[index];
            }

        }
        private static IEnumerable<char> Escape(string str)
        {
            yield return '\"';
            yield return '\\';
            yield return '\"';
            for (var index = 1; index < str.Length - 1; index++)
            {
                var c = str[index];
                if (c == '\\')
                {
                    if (str[index + 1] == 'x')
                    {
                        yield return '\\';
                        yield return '\\';
                    }
                    else
                    {
                        yield return '\\';
                        yield return '\\';
                        yield return '\\';
                        yield return str[index + 1];
                        index++;
                    }
                }
                else yield return str[index];
            }

            yield return '\\';
            yield return '\"';
            yield return '\"';
        }


        public override object ExecutePart1()
        {
            var lines = File.ReadAllLines(DataFile);
            int total = 0;
            int lowerTotal = 0;
            foreach (var line in lines)
            {
                total += line.Length;
                var nLine = new string(Unescape(line).ToArray());
                lowerTotal += nLine.Length;
            }
            return total - lowerTotal;
        }

        public override object ExecutePart2()
        {
            var lines = File.ReadAllLines(DataFile);
            int total = 0;
            int lowerTotal = 0;
            foreach (var line in lines)
            {
                lowerTotal += line.Length;
                //Console.WriteLine(line);
                var nLine = new string(Escape(line).ToArray());
                // Console.WriteLine(nLine);
                total += nLine.Length;
            }
            return total - lowerTotal;
        }
    }
}