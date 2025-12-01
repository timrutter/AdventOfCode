using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2015;

public class Advent2015Day12 : Solution
{
    public Advent2015Day12()
    {
        Answer1 = 111754;
        Answer2 = null;
    }

    private static int SumAllNumbers(string s)
    {
        var matches = Regex.Matches(s, "[0123456789-]+");
        return matches.Sum(m => int.Parse(m.Value));
    }

    public override object ExecutePart1()
    {
        return SumAllNumbers(File.ReadAllText(DataFile));
        var matches = Regex.Matches(File.ReadAllText(DataFile), "[0123456789-]+");
        return matches.Sum(m => int.Parse(m.Value));
    }

    public override object ExecutePart2()
    {
        var sum = 0;
        var s = File.ReadAllText(DataFile);

        int Populate(JSonTree jsonTree, int start)
        {
            for (var i = start; i < s.Length; i++)
                switch (s[i])
                {
                    case 'r' when i < s.Length - 3 && s[i + 1] == 'e' && s[i + 2] == 'd':
                        jsonTree.HasRed = true;
                        break;
                    case '[':
                    {
                        var c = new JSonTree(i, true);
                        jsonTree.AddChild(c);
                        i = Populate(c, i + 1);
                        break;
                    }
                    case '}':
                    case ']':
                    {
                        jsonTree.SubString = s.Substring(jsonTree.Start, i - jsonTree.Start + 1);
                        return i + 1;
                    }
                    case '{':
                    {
                        var c = new JSonTree(i, false);
                        jsonTree.AddChild(c);
                        i = Populate(c, i + 1);
                        break;
                    }
                }

            jsonTree.SubString = s.Substring(jsonTree.Start, s.Length - jsonTree.Start);
            return s.Length;
        }

        var root = new JSonTree(0, false) { SubString = s };
        Populate(root, 1);
        return root.Sum();
    }

    private class JSonTree : Tree<JSonTree>
    {
        public JSonTree(int start, bool isArray)
        {
            Start = start;
            IsArray = isArray;
        }

        public string SubString { get; set; }
        public int Start { get; }
        public bool IsArray { get; }
        public bool HasRed { get; set; }

        public int Sum()
        {
            if (HasRed && !IsArray)
                return 0;
            var sum = 0;
            var s = SubString;
            foreach (var c in Children)
            {
                sum += c.Sum();
                s = s.Replace(c.SubString, "");
            }

            sum += SumAllNumbers(s);
            return sum;
        }
    }
}