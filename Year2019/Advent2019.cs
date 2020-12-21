using System.IO;
using System.Linq;
using AdventOfCode.Functions;

namespace AdventOfCode.Year2019
{
    public static class Advent2019
    {
        
        #region Methods

        public static int Puzzle1Part1()
        {
            return "Year2019\\Data\\Day01.txt".ReadAll<int>().Select(i => i / 3 - 2).Sum();
        }

        public static int Puzzle1Part2()
        {
            return "Year2019\\Data\\Day01.txt".ReadAll<int>().Select(i =>
            {
                var sum = 0;
                var res = i;
                while (res > 0)
                {
                    var val = res / 3 - 2;
                    res = val;
                    sum += res > 0 ? res : 0;
                }
                return sum;
            }).Sum();

        }

        
        public static int Puzzle2Part1()
        {
            var ints =File.ReadAllText("Year2019\\Data\\Day02.txt").SplitToType<int>(",").ToList();
            ints[1] = 12;
            ints[2] = 2;
            return ints.CreateAndExecuteOpcodeComputer().Memory[0];
        }

        public static int Puzzle2Part2()
        {
            var intsStart =File.ReadAllText("Year2019\\Data\\Day02.txt").SplitToType<int>(",").ToList();
            for (int noun = 0; noun <= 99; noun++)
            for (int verb = 0; verb <= 99; verb++)
            {
                intsStart[1] = noun;
                intsStart[2] = verb;
                var ret = intsStart.CreateAndExecuteOpcodeComputer().Memory[0];
                if (ret == 19690720)
                    return 100 * noun + verb;
            }

            return -1;
        }

        public static int Puzzle3Part1()
        {
            
            return int.MaxValue;
        }

        private static int Puzzle3Part1Internal(string[] trees, int xInc, int yInc)
        {
           
            return int.MaxValue;
        }

        public static int Puzzle3Part2()
        {
           
            return int.MaxValue;
        }

        public static int Puzzle4Part1()
        {
            
            return int.MaxValue;
        }

        public static int Puzzle4Part2()
        {
            
            return int.MaxValue;
        }

        public static int Puzzle5Part1()
        {
            
            return int.MaxValue;
        }

        public static int Puzzle5Part2()
        {
            
            return int.MaxValue;
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