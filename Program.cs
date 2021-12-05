using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var cons = "";
            while (cons != "q")
            {
                //Execute(typeof(Advent2015));
                // Execute(typeof(Advent2016));
                // Execute(typeof(Advent2017));
                // Execute(typeof(Advent2018));
                //Execute(typeof(Advent2019));
                //Execute(new Advent2020.Advent2020(), GetDays(DayMode.Day, 
                //    new List<int>{ 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,21,22,23,24,25}));

                Execute(new Advent2021.Advent2021(), GetDays(DayMode.Today));


                cons = Console.ReadLine();
            }
        }

        private static List<int> GetDays(DayMode dayMode, List<int> days = null)
        {
            switch (dayMode)
            {
                case DayMode.All:
                    return Enumerable.Range(1, 25).ToList();
                case DayMode.Today:
                    return new List<int> {DateTime.Now.Day};
                case DayMode.Day:
                    return days ?? new List<int>();
            }

            return new List<int>();
        }

        private static void WriteAnswer(object answer, object expected, int day, int part, Stopwatch sw, bool showIncomplete)
        {
            switch (answer)
            {
                case int l when l != int.MaxValue:
                    Console.Write($"{day}.{part} {l} {sw.ElapsedMilliseconds}ms");
                    break;
                case long l when l != long.MaxValue:
                    Console.Write($"{day}.{part} {l} {sw.ElapsedMilliseconds}ms");
                    break;
                case ushort l when l != ushort.MaxValue:
                    Console.Write($"{day}.{part} {l} {sw.ElapsedMilliseconds}ms");
                    break;
                case string s:
                    Console.Write($"{day}.{part} {s} {sw.ElapsedMilliseconds}ms");
                    break;
                default:
                    if (showIncomplete)
                        Console.WriteLine($"{day}.{part} Not done");
                    return;
            }

            if (expected != null)
            {
                var passFail = answer.Equals(expected) ? "PASS" : $"=======FAIL=======: expected {expected} got {answer}";
                Console.WriteLine($" {passFail}"); 
            } else Console.WriteLine();
        }

        private static void Execute(Solutions solutions, List<int> days, bool showIncomplete = false)
        {
            Console.WriteLine($"=============={solutions.GetType().Name}===============");
            var methods = solutions.Solutions;
            foreach (var day in days)
            {
                var sw = Stopwatch.StartNew();
                var m = methods.FirstOrDefault(m => m.GetType().Name == $"{m.GetType().Name.Substring(0,10)}Day{day:00}");
                if (m == null)
                {
                    Console.WriteLine($"Day {day} is missing");
                    continue;
                }

                WriteAnswer(m.ExecutePart1(), m.Answer1, day, 1, sw, showIncomplete);
                WriteAnswer(m.ExecutePart2(), m.Answer2, day, 2, sw, showIncomplete);
            }
        }

        public static void Execute(Type t, bool showIncomplete = false)
        {
            Console.WriteLine($"=============={t.Name}===============");
            var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();
            for (var i = 1; i <= 25; i++)
                //for (var i = 4; i <= 4; i++)
                //int i = DateTime.Now.Day;
            {
                var sw = Stopwatch.StartNew();
                var m = methods.FirstOrDefault(m => m.Name == $"Puzzle{i}Part1");
                if (m != null)
                {
                    var answer = m.Invoke(null, null);

                    switch (answer)
                    {
                        case int l when l != int.MaxValue:
                            Console.WriteLine($"{i}.1 {l} {sw.ElapsedMilliseconds}ms");
                            break;
                        case long l when l != long.MaxValue:
                            Console.WriteLine($"{i}.1 {l} {sw.ElapsedMilliseconds}ms");
                            break;
                        case ushort l when l != ushort.MaxValue:
                            Console.WriteLine($"{i}.1 {l} {sw.ElapsedMilliseconds}ms");
                            break;
                        case string s:
                            Console.WriteLine($"{i}.1 {s} {sw.ElapsedMilliseconds}ms");
                            break;
                        default:
                            if (showIncomplete)
                                Console.WriteLine($"{i}.1 Not done");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"{i}.1 MISSING METHOD");
                }

                m = methods.FirstOrDefault(m => m.Name == $"Puzzle{i}Part2");
                if (m != null)
                {
                    var answer = m.Invoke(null, null);
                    switch (answer)
                    {
                        case int o when o != int.MaxValue:
                            Console.WriteLine($"{i}.2 {o} {sw.ElapsedMilliseconds}ms");
                            break;
                        case long l when l != long.MaxValue:
                            Console.WriteLine($"{i}.2 {l} {sw.ElapsedMilliseconds}ms");
                            break;
                        case string s:
                            Console.WriteLine($"{i}.1 {s} {sw.ElapsedMilliseconds}ms");
                            break;
                        default:
                            if (showIncomplete)
                                Console.WriteLine($"{i}.2 Not done");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"{i}.2 MISSING METHOD");
                }
            }
        }

        #endregion
    }

    internal enum DayMode
    {
        All, Today, Day
    }
}