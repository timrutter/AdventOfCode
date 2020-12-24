using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AdventOfCode.Year2015;
using AdventOfCode.Year2019;
using AdventOfCode.Year2020;

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
                 Execute(typeof(Advent2020));


                cons = Console.ReadLine();
            }
        }

        public static void Execute(Type t, bool showIncomplete = false)
        {
            Console.WriteLine($"=============={t.Name}===============");
            var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();
            //for (var i = 1; i <= 25; i++)
            //for (var i = 4; i <= 4; i++)
               int i = DateTime.Now.Day;
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
}