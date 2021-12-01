using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Functions;
// ReSharper disable UnusedMember.Global

namespace AdventOfCode.Year2021
{
    public static class Advent2021
    {

        #region Methods

        public static int Puzzle1Part1()
        {
            var arr = "Year2021\\Data\\Day01.txt".ReadAll<int>();
            int count = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > arr[i - 1]) count++;
            }

            return count;
        }


        public static int Puzzle1Part2()
        {
            var arr = "Year2021\\Data\\Day01.txt".ReadAll<int>();
            int count = 0;
            for (int i = 3; i < arr.Length; i++)
            {
                if (arr[i] + arr[i - 1] + arr[i - 2] > arr[i - 1] + arr[i - 2] + arr[i - 3]) count++;
            }

            return count;
        }

        public static int Puzzle2Part1()
        {

            return int.MaxValue;
        }

        public static int Puzzle2Part2()
        {

            return int.MaxValue;
        }

        public static int Puzzle3Part1()
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