using System;
using AdventOfCode.Functions;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day12 : Solution
    {
        public Advent2020Day12()
        {
            Answer1 = 381;
            Answer2 = 28591;
        }
        public override object ExecutePart1()
        {
            var instructions = DataFile.ReadAll<string>();
            var epos = 0;
            var npos = 0;
            var direction = 90;
            string[] directions = { "N", "E", "S", "W" };

            void Move(string dir, int distance)
            {
                switch (dir)
                {
                    case "N":
                        npos += distance;
                        break;
                    case "S":
                        npos -= distance;
                        break;
                    case "E":
                        epos += distance;
                        break;
                    case "W":
                        epos -= distance;
                        break;
                }
            }

            foreach (var instruction in instructions)
            {
                var command = instruction.Substring(0, 1);
                var distance = int.Parse(instruction.Substring(1));
                switch (command)
                {
                    case "N":
                    case "S":
                    case "E":
                    case "W":
                        Move(command, distance);
                        //Console.WriteLine($"{instruction}: epos={epos} npos={npos}");
                        break;
                    case "F":
                        Move(directions[direction / 90], distance);
                        //Console.WriteLine($"{instruction}: epos={epos} npos={npos}");
                        break;
                    case "R":
                        direction = (direction + distance) % 360;
                        //Console.WriteLine($"{instruction}: direction={direction}");
                        break;
                    case "L":
                        direction = (direction - distance) % 360;
                        if (direction < 0) direction += 360;
                        //Console.WriteLine($"{instruction}: direction={direction}");
                        break;
                }
            }

            return Math.Abs(epos) + Math.Abs(npos);
        }

        public override object ExecutePart2()
        {
            var instructions = DataFile.ReadAll<string>();
            var epos = 0;
            var npos = 0;
            var wpepos = 10;
            var wpnpos = 1;

            void MoveWaypoint(string dir, int distance)
            {
                switch (dir)
                {
                    case "N":
                        wpnpos += distance;
                        break;
                    case "S":
                        wpnpos -= distance;
                        break;
                    case "E":
                        wpepos += distance;
                        break;
                    case "W":
                        wpepos -= distance;
                        break;
                }
            }

            void RotateWaypoint(int directionChange)
            {
                switch (directionChange)
                {
                    case 90:
                        {
                            var temp = wpnpos;
                            wpnpos = -wpepos;
                            wpepos = temp;
                            break;
                        }
                    case 180:
                        wpepos = -wpepos;
                        wpnpos = -wpnpos;
                        break;
                    case 270:
                        {
                            var temp = wpnpos;
                            wpnpos = wpepos;
                            wpepos = -temp;
                            break;
                        }
                }
            }

            foreach (var instruction in instructions)
            {
                var command = instruction.Substring(0, 1);
                var distance = int.Parse(instruction.Substring(1));
                switch (command)
                {
                    case "N":
                    case "S":
                    case "E":
                    case "W":
                        {
                            MoveWaypoint(command, distance);
                            //Console.WriteLine($"{instruction}: epos={epos} npos={npos}");
                            break;
                        }
                    case "F":
                        {
                            epos += wpepos * distance;
                            npos += wpnpos * distance;
                            //Console.WriteLine($"{instruction}: epos={epos} npos={npos}");
                            break;
                        }
                    case "R":
                        {
                            var directionChange = distance % 360;
                            RotateWaypoint(directionChange);
                            break;
                        }
                    case "L":
                        {
                            var directionChange = -distance % 360 + 360;
                            RotateWaypoint(directionChange);
                            break;
                        }
                }
            }

            return Math.Abs(epos) + Math.Abs(npos);
        }
    }
}