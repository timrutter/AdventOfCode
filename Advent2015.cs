using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Advent2015
    {
        
        #region Methods

        public static int Puzzle1Part1()
        {
            var data = File.ReadAllText("Data2015\\Day01.txt");
            return data.ToList().Count(c => c == '(') - data.ToList().Count(c => c == ')');
        }

        public static int Puzzle1Part2()
        {
            var data = File.ReadAllText("Data2015\\Day01.txt");
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
            var data = File.ReadAllLines("Data2015\\Day02.txt");
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
            var data = File.ReadAllLines("Data2015\\Day02.txt");
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
            var data = File.ReadAllText("Data2015\\Day03.txt");
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
           
            var data = File.ReadAllText("Data2015\\Day03.txt");
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
            
            var lines = File.ReadAllLines("Data2015\\Day05.txt").ToList();
            return lines.Count(line =>
            {
                return line.Count(c => c.IsVowel()) >= 3 &&
                       Characters.LowerAlphabet.Any(c=> Regex.IsMatch(line,$"{c}{c}")) &&
                       new[] {"ab", "cd", "pq", "xy"}.All(no => !line.Contains(no));
            });
        }

        public static int Puzzle5Part2()
        {
            var lines = File.ReadAllLines("Data2015\\Day05.txt").ToList();
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
            
            return int.MaxValue;
        }

        public static int Puzzle6Part2()
        {
           
            return int.MaxValue;
        }

        public static int Puzzle7Part1()
        {
            
            return int.MaxValue;
        }
        public static int Puzzle7Part2()
        {
          
            return int.MaxValue;
        }


        public static int Puzzle8Part1()
        {
          
            return int.MaxValue;
        }

        
        public static int Puzzle8Part2()
        {
           
            return int.MaxValue;
        }

        public static int Puzzle9Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle9Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle10Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle10Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle11Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle11Part2()
        {
            return int.MaxValue;
        }

        public static int Puzzle12Part1()
        {
            return int.MaxValue;
        }

        public static int Puzzle12Part2()
        {
            return int.MaxValue;
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