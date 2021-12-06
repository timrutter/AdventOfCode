using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Advent2015
{
    public class Advent2015Day06 : Solution
    {
        public Advent2015Day06()
        {
            Answer1 = 400410;
            Answer2 = 15343601;
        }
        public override object ExecutePart1()
        {
            var lines = File.ReadAllLines(DataFile).Select(l =>
                Regex.Match(l, @"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)"));
            var lights = new bool[1000, 1000];
            foreach (var match in lines)
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

        public override object ExecutePart2()
        {
            var lines = File.ReadAllLines(DataFile).Select(l =>
                  Regex.Match(l, @"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)"));
            var lights = new int[1000, 1000];
            foreach (var match in lines)
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
                                lights[x, y]++;
                            }
                        break;
                    case "turn off":
                        for (int x = startx; x <= endx; x++)
                            for (int y = starty; y <= endy; y++)
                            {
                                if (lights[x, y] > 0)
                                    lights[x, y]--;
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
    }
}