using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

namespace AdventOfCode.Year2015
{
    public static class Advent2015
    {
        
        #region Methods

        public static int Puzzle1Part1()
        {
            var data = File.ReadAllText("Year2015\\Data\\Day01.txt");
            return data.ToList().Count(c => c == '(') - data.ToList().Count(c => c == ')');
        }

        public static int Puzzle1Part2()
        {
            var data = File.ReadAllText("Year2015\\Data\\Day01.txt");
            int count = 0;
            for (var index = 0; index < data.Length; index++)
            {
                var c = data[index];
                if (c == '(')
                    count++;
                else if (c == ')')
                    count--;
                if (count < 0)
                    return index + 1;
            }

            return int.MaxValue;
        }

        public static int Puzzle2Part1()
        {
            var data = File.ReadAllLines("Year2015\\Data\\Day02.txt");
            int count = 0;
            foreach (var t in data)
            {
                var bits = t.SplitToType<int>("x").ToList();
                var x = bits[0];
                var y = bits[1];
                var z = bits[2];
                var a1 = x * y  ;
                var a2 =  x * z  ;
                var a3 =  y * z ;
                count += a1 * 2 + a2 * 2 + a3 * 2 + new[] {a1, a2, a3}.Min();
            }

            return count;
        }

        public static int Puzzle2Part2()
        {
            var data = File.ReadAllLines("Year2015\\Data\\Day02.txt");
            int count = 0;
            foreach (var t in data)
            {
                var bits = t.SplitToType<int>("x").ToList();
                var x = bits[0];
                var y = bits[1];
                var z = bits[2];
                var a1 = 2*x + 2* y  ;
                var a2 =  2*x + 2*z  ;
                var a3 =  2*y + 2*z ;
                count +=  new[] {a1, a2, a3}.Min() + x*y*z;
            }

            return count;
        }

        public static int Puzzle3Part1()
        {
            var data = File.ReadAllText("Year2015\\Data\\Day03.txt");
            var hashSet = new HashSet<(int, int)> {(0, 0)};
            int x = 0, y = 0;
            foreach (var t in data.ToList())
            {
                switch (t)
                {
                    case '>': y++; break;
                    case '<': y--; break;
                    case '^': x++; break;
                    case 'v': x--; break;
                }

                if (!hashSet.Contains((x, y)))
                    hashSet.Add((x, y));
            }

            return hashSet.Count;
            
        }
        

        public static int Puzzle3Part2()
        {
           
            var data = File.ReadAllText("Year2015\\Data\\Day03.txt");
            var hashSet = new HashSet<(int, int)> {(0, 0)};
            int x = 0, y = 0;
            for (var index = 0; index < data.ToList().Count; index+=2)
            {
                var t = data.ToList()[index];
                switch (t)
                {
                    case '>':
                        y++;
                        break;
                    case '<':
                        y--;
                        break;
                    case '^':
                        x++;
                        break;
                    case 'v':
                        x--;
                        break;
                }

                if (!hashSet.Contains((x, y)))
                    hashSet.Add((x, y));
            }

            x = 0; y = 0;
            for (var index = 1; index < data.ToList().Count; index+=2)
            {
                var t = data.ToList()[index];
                switch (t)
                {
                    case '>':
                        y++;
                        break;
                    case '<':
                        y--;
                        break;
                    case '^':
                        x++;
                        break;
                    case 'v':
                        x--;
                        break;
                }

                if (!hashSet.Contains((x, y)))
                    hashSet.Add((x, y));
            }

            return hashSet.Count;
        }

        public static int Puzzle4Part1()
        {
            var input= "yzbqklnj";
            var md5 = System.Security.Cryptography.MD5.Create();
            int answer = 0;
            while (true)
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes($"{input}{answer.ToString()}");
                var hashBytes = md5.ComputeHash(inputBytes).ToHexString();
                bool areEqual = hashBytes.StartsWith("00000");

                if (areEqual)
                    return answer;
                answer++;
            }
        }

        public static int Puzzle4Part2()
        {
            return int.MaxValue;
            var input= "yzbqklnj";
            var md5 = System.Security.Cryptography.MD5.Create();
            int answer = 0;
            while (true)
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes($"{input}{answer.ToString()}");
                var hashBytes = md5.ComputeHash(inputBytes).ToHexString();
                bool areEqual = hashBytes.StartsWith("000000");

                if (areEqual)
                    return answer;
                answer++;
            }
        }

        public static int Puzzle5Part1()
        {
            
            var lines = File.ReadAllLines("Year2015\\Data\\Day05.txt").ToList();
            return lines.Count(line =>
            {
                return line.Count(c => c.IsVowel()) >= 3 &&
                       Characters.LowerAlphabet.Any(c=> Regex.IsMatch(line,$"{c}{c}")) &&
                       new[] {"ab", "cd", "pq", "xy"}.All(no => !line.Contains(no));
            });
        }

        public static int Puzzle5Part2()
        {
            var lines = File.ReadAllLines("Year2015\\Data\\Day05.txt").ToList();
            return lines.Count(line =>
            {
                var match = false;
                foreach (var c1 in Characters.LowerAlphabet)
                {
                    match = Characters.LowerAlphabet.Any(c2 => Regex.IsMatch(line, $"{c1}{c2}.*{c1}{c2}"));
                    if (match) break;
                }

                return match && Characters.LowerAlphabet.Any(c1 => Regex.IsMatch(line, $"{c1}.{c1}"));
            });
        }

        public static int Puzzle6Part1()
        {
            var lines = File.ReadAllLines("Year2015\\Data\\Day06.txt").Select(l =>
                Regex.Match(l, @"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)"));
            var lights =new  bool[1000, 1000];
            foreach(var match in lines)
            {
                var startx = int.Parse(match.Groups[2].Value);
                var starty = int.Parse(match.Groups[3].Value);
                var endx = int.Parse(match.Groups[4].Value);
                var endy = int.Parse(match.Groups[5].Value);
                switch (match.Groups[1].Value)
                {
                    case "turn on":
                        for (int x = startx; x <= endx; x++)
                        for (int y = starty; y <= endy; y++)
                        {
                            lights[x, y] = true;
                        }
                        break;
                    case "turn off":
                        for (int x = startx; x <= endx; x++)
                        for (int y = starty; y <= endy; y++)
                        {
                            lights[x, y] = false;
                        }
                        break;
                    case "toggle":
                        for (int x = startx; x <= endx; x++)
                        for (int y = starty; y <= endy; y++)
                        {
                            lights[x, y] = !lights[x, y];
                        }
                        break;
                }
            }

            var count = 0;
            for (int x = 0; x < lights.GetLength(0); x++)
            for (int y = 0; y < lights.GetLength(1); y++)
                if (lights[x, y])
                    count++;
            return count;
        }

        public static int Puzzle6Part2()
        {
            var lines = File.ReadAllLines("Year2015\\Data\\Day06.txt").Select(l =>
                Regex.Match(l, @"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)"));
            var lights =new  int[1000, 1000];
            foreach(var match in lines)
            {
                var startx = int.Parse(match.Groups[2].Value);
                var starty = int.Parse(match.Groups[3].Value);
                var endx = int.Parse(match.Groups[4].Value);
                var endy = int.Parse(match.Groups[5].Value);
                switch (match.Groups[1].Value)
                {
                    case "turn on":
                        for (int x = startx; x <= endx; x++)
                        for (int y = starty; y <= endy; y++)
                        {
                            lights[x, y] ++;
                        }
                        break;
                    case "turn off":
                        for (int x = startx; x <= endx; x++)
                        for (int y = starty; y <= endy; y++)
                        {
                            if (lights[x,y] > 0)
                                lights[x, y] --;
                        }
                        break;
                    case "toggle":
                        for (int x = startx; x <= endx; x++)
                        for (int y = starty; y <= endy; y++)
                        {
                            lights[x, y] += 2;
                        }
                        break;
                }
            }

            var count = 0;
            for (int x = 0; x < lights.GetLength(0); x++)
            for (int y = 0; y < lights.GetLength(1); y++)
               count += lights[x, y];
            return count;
        }

        public static int Puzzle7Part1()
        {
            var lines = File.ReadAllLines("Year2015\\Data\\Day07.txt").Select(l => (
                Regex.Match(l, @"([a-z0-9]*)? ?(AND|OR|NOT|LSHIFT|RSHIFT) ([a-z0-9]*) -> ([a-z]*)"), l)).ToList();

            var instructions = new List<(string operation, string in1, string in2, string out1)>();
            var dict = new Dictionary<string, ushort>();
            foreach (var match in lines)
            {
                if (match.Item1.Success)
                    instructions.Add((match.Item1.Groups[2].Value,match.Item1.Groups[1].Value,match.Item1.Groups[3].Value,match.Item1.Groups[4].Value));
                else
                {
                    var bits = match.l.Split(" -> ");
                    instructions.Add(("ASIGN",bits[0],"",bits[1]));

                }
            }

            var inst2 = instructions.ToList();

            void Trace()
            {
                
                while (true)
                {
                    for (var index = 0; index < instructions.Count; index++)
                    {
                        var line = instructions[index];
                        switch (line.operation)
                        {
                            case "NOT":
                            {
                                if (!dict.Keys.Contains(line.in2)) continue;
                                //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                var val = dict[line.in2];
                                    val = (ushort)~val;

                                dict[line.out1] = val;
                                instructions.RemoveAt(index);
                                index--;
                                break;
                            }
                            case "AND":
                            {

                                if ((!int.TryParse(line.in1,out var inNum) && !dict.Keys.Contains(line.in1)) || !dict.Keys.Contains(line.in2)) continue;
                                //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                var val1 = int.TryParse(line.in1,out inNum) ? inNum : dict[line.in1];
                                var val3 = dict[line.in2];
                                dict[line.out1] = (ushort)(val1 & val3);
                                instructions.RemoveAt(index);
                                index--;
                                break;
                            }
                            case "OR":
                            {
                                if (!dict.Keys.Contains(line.in1) || !dict.Keys.Contains(line.in2)) continue;
                                //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                var val1 = dict[line.in1];
                                var val3 = dict[line.in2];
                                dict[line.out1] = (ushort)(val1 | val3);
                                instructions.RemoveAt(index);
                                index--;
                                break;
                            }
                            case "LSHIFT":
                            {
                                if (!dict.Keys.Contains(line.in1)) continue;
                                //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                var val1 = dict[line.in1];
                                var val3 = int.Parse(line.in2);
                                dict[line.out1] = (ushort)(val1 << val3);
                                instructions.RemoveAt(index);
                                index--;
                                break;
                            }
                            case "RSHIFT":
                            {
                                if (!dict.Keys.Contains(line.in1)) continue;
                                //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                var val1 = dict[line.in1];
                                var val3 = int.Parse(line.in2);
                                dict[line.out1] = (ushort)(val1 >> val3);
                                instructions.RemoveAt(index);
                                index--;
                                break;
                            }
                            case "ASIGN":
                            {
                                if (ushort.TryParse(line.in1, out var val))
                                {
                                    //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                    dict.Add(line.out1, val);
                                    instructions.RemoveAt(index);
                                    index--;
                                    break;
                                }
                                if (!dict.Keys.Contains(line.in1) ) continue;
                                //Console.WriteLine($"{line.in1} {line.operation} {line.in2} -> {line.out1}");
                                dict[line.out1] = dict[line.in1];
                                instructions.RemoveAt(index);
                                index--;
                                break;
                            }
                        }
                    }

                    if (dict.ContainsKey("a"))
                        return;
                    Trace();
                }
            }
            
             Trace();

             dict.Clear();
             instructions = inst2.ToList();
             var index =instructions.FindIndex(i => i.operation == "ASIGN" && i.out1 == "b");
             instructions[index] = ("ASIGN", "16076", "", "b");
             Trace();
            return -1;
        }
        public static int Puzzle7Part2()
        {
          
            return int.MaxValue;
        }


        public static int Puzzle8Part1()
        {
            var lines = File.ReadAllLines("Year2015\\Data\\Day08.txt");
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
            for (var index = 1; index < str.Length -1; index++)
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


        public static int Puzzle8Part2()
        {
            var lines = File.ReadAllLines("Year2015\\Data\\Day08.txt");
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

        public static int Puzzle9Part1()
        {
            var kvps = "Year2015\\Data\\Day09.txt".ReadAllKeyValuePairs<string, int>(" = ").ToList();
            var distances = kvps.Select(k =>
            {
                var bits = k.key.Split(" to ");
                return (bits[0], bits[1], k.value);
            }).ToList();
            distances.AddRange(kvps.Select(k =>
            {
                var bits = k.key.Split(" to ");
                return (bits[1], bits[0], k.value);
            }));

            var uNames = distances.Select(r => r.Item1).Distinct().ToList();

            var routes = new List<int>();
            void GetRouteOptions(List<string> visited, string start, int distanceTravelled)
            {
                if (visited.Contains(start)) return ;
                visited.Add(start);
                if (visited.Count == uNames.Count())
                {
                    routes.Add(distanceTravelled);
                    return;
                }
                foreach (var route in distances.Where(d => d.Item1 == start))
                {
                    GetRouteOptions(visited.ToList(),route.Item2,distanceTravelled + route.value);
                }
            }
            foreach(var d in uNames)
                GetRouteOptions(new List<string>(),d,0);

            return routes.Max();

        }

        public static int Puzzle9Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle10Part1()
        {
            var text = File.ReadAllText("Year2015\\Data\\Day10.txt");

            string LookAndSay(string s)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < s.Length; i++)
                {
                    var startChar = s[i];
                    var startI = i ;
                    while ( i + 1 < s.Length && s[i + 1] == startChar)
                        i++;
                    sb.Append($"{i - startI + 1}{startChar}");
                }

                return sb.ToString();
            }

            for (int i = 0; i < 50; i++)
            {
                text = LookAndSay(text);
            }

            return text.Length;
        }

        public static int Puzzle10Part2()
        {
            return int.MaxValue;
        }

        public static string Puzzle11Part1()
        {
            var input = "cqjxjnds";

            char IncChar(char ch)
            {
                var letNum = ch - 97;
                letNum = (letNum + 1) % 26;
                return (char) (97 + letNum);
            }
            string Increment(string str)
            {
                var chars = str.ToCharArray();
                for (int i = str.Length - 1; i >= 0; i--)
                {
                    var newCh = IncChar(chars[i]);
                    chars[i] = newCh;
                    if (newCh != 'a')
                        return new string(chars);
                }

                return $"a{new string(chars)}";
            }

            bool IsValid(string password)
            {
                if (password.Contains("i") || password.Contains("o") || password.Contains("l")) return false;

                if (Characters.LowerAlphabet.Select(a => password.Contains($"{a}{a}")).Count(c => c) < 2) return false;

                if (Characters.LowerAlphabet.All(a => !password.Contains($"{a}{(char)(a + 1)}{(char)(a + 2)}"))) return false;

                return true;
            }
            var newPass = Increment(input);
            while (!IsValid(newPass))
                newPass =Increment(newPass);

            newPass = Increment(newPass);
            while (!IsValid(newPass))
                newPass =Increment(newPass);
            return newPass;
        }

        public static int Puzzle11Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle12Part1()
        {
            return SumAllNumbers(File.ReadAllText("Year2015\\Data\\Day12.txt"));
            var matches = Regex.Matches(File.ReadAllText("Year2015\\Data\\Day12.txt"),"[0123456789-]+");
            return matches.Sum(m => int.Parse(m.Value));
        }

        private static int SumAllNumbers(string s)
        {
            var matches = Regex.Matches(s,"[0123456789-]+");
            return matches.Sum(m => int.Parse(m.Value));
        }

        public static int Puzzle12Part2()
        {
            var sum = 0;
            var s = File.ReadAllText("Year2015\\Data\\Day12.txt");
            
            int Populate(JSonTree jsonTree, int start)
            {
                for (int i = start; i < s.Length ; i++)
                {
                    switch (s[i])
                    {
                        case 'r' when i < s.Length -3 && s[i+1] == 'e' && s[i+2] == 'd':
                            jsonTree.HasRed = true;
                            break;
                        case '[':
                        {
                            var c = new JSonTree(i, true);
                            jsonTree.AddChild(c);
                            i = Populate(c,i + 1);
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
                            i = Populate(c,i + 1);
                            break;
                        }
                    }
                }
                jsonTree.SubString = s.Substring(jsonTree.Start, s.Length - jsonTree.Start );
                return s.Length ;
            }

            var root = new JSonTree(0, false) {SubString = s};
            Populate(root, 1);
            return root.Sum();
        }
        private class JSonTree : Tree<JSonTree>
        {
            public string SubString { get; set; }
            public int Start { get; }
            public bool IsArray { get; }
            public bool HasRed { get; set; }

            public JSonTree(int start, bool isArray) 
            {
                Start = start;
                IsArray = isArray;
            }

            public int Sum()
            {
                if (HasRed && !IsArray)
                    return 0;
                var sum = 0;
                var s = SubString;
                foreach (var c in Children)
                {
                    sum += c.Sum();
                    s = s.Replace(c.SubString,"");
                }
                sum += SumAllNumbers(s);
                return sum;
            }
        }
        public static int Puzzle13Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle13Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle14Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle14Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle15Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle15Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle16Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle16Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle17Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle17Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle18Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle18Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle19Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle19Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle20Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle20Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle21Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle21Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle22Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle22Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle23Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle23Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle24Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle24Part2()
        {
            return int.MaxValue;
        }
        public static int Puzzle25Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle25Part2()
        {
            return int.MaxValue;
        }
        #endregion
    }

  }