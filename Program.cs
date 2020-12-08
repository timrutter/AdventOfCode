using System;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string cons = "";
            while (cons != "q")
            {
                var methods = typeof(Advent2020).GetMethods( BindingFlags.Public | BindingFlags.Static).ToList();
                // for (int i = 1; i <= 24; i++)
                int i = DateTime.Now.Day;
                {
                    var m = methods.FirstOrDefault(m => m.Name == $"Puzzle{i}Part1");
                    if (m != null)
                    {
                        var answer = m.Invoke(null, null);
                        if (answer != null) 
                            Console.WriteLine($"{i}.1 {answer}");
                    }
                    else Console.WriteLine($"{i}.1 MISSING METHOD");
                    m = methods.FirstOrDefault(m => m.Name == $"Puzzle{i}Part2");
                    if (m != null) 
                    {
                        var answer = m.Invoke(null, null);
                        if (answer != null)
                            Console.WriteLine($"{i}.2 {answer}");
                    }
                    else Console.WriteLine($"{i}.2 MISSING METHOD");


                }
                
                cons =   Console.ReadLine();

            }
        }


    }
}