using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020;

public class Advent2020Day17 : Solution
{
    public Advent2020Day17()
    {
        Answer1 = 317;
        Answer2 = 1692;
    }
    // private static void Dump(HashSet<(int x, int y, int z)> array)
    // {
    //     for (var z = array.Min(t => t.z); z <= array.Max(t => t.z); z++)
    //     {
    //         Console.WriteLine($"z = {z}");
    //         for (var y = array.Min(t => t.y); y <= array.Max(t => t.y); y++)
    //         {
    //             for (var x = array.Min(t => t.x); x <= array.Max(t => t.x); x++)
    //                 Console.Write(array.Contains((x, y, z)) ? '#' : '.');
    //             Console.WriteLine();
    //         }
    //
    //         Console.WriteLine();
    //     }
    // }

    // private static void Dump2(HashSet<(int x, int y, int z, int w)> array)
    // {
    //     for (var w = array.Min(t => t.w); w <= array.Max(t => t.w); w++)
    //     for (var z = array.Min(t => t.z); z <= array.Max(t => t.z); z++)
    //     {
    //         Console.WriteLine($"z = {z}, w={w}");
    //         for (var y = array.Min(t => t.y); y <= array.Max(t => t.y); y++)
    //         {
    //             for (var x = array.Min(t => t.x); x <= array.Max(t => t.x); x++)
    //                 Console.Write(array.Contains((x, y, z, w)) ? '#' : '.');
    //             Console.WriteLine();
    //         }
    //
    //         Console.WriteLine();
    //     }
    // }
    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>().ToList();
        var array = new HashSet<(int x, int y, int z)>();
        for (var y = lines.Count - 1; y >= 0; y--)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
                if (line[x] == '#')
                    array.Add((x, y, 0));
        }


        //Console.WriteLine($"======INITIAL======");
        //Dump(array);
        for (var cycle = 1; cycle <= 6; cycle++)
        {
            var newArray = new HashSet<(int x, int y, int z)>();
            //Console.WriteLine($"======CYCLE={cycle}======");
            var xmax = array.Max(t => t.x) + 1;
            var ymax = array.Max(t => t.y) + 1;
            var zmax = array.Max(t => t.z) + 1;
            for (var x = array.Min(t => t.x) - 1; x <= xmax; x++)
            for (var y = array.Min(t => t.y) - 1; y <= ymax; y++)
            for (var z = array.Min(t => t.z) - 1; z <= zmax; z++)
            {
                var neighbours = BoardExtensions.GetNeighbours(x, y, z);
                var activeCount = neighbours.Count(n => array.Contains((n.x, n.y, n.z)));
                if (array.Contains((x, y, z)) && (activeCount == 2 || activeCount == 3))
                    newArray.Add((x, y, z));
                else if (!array.Contains((x, y, z)) && activeCount == 3)
                    newArray.Add((x, y, z));
            }

            array = newArray;

            //Dump(array);
            //Console.WriteLine($"On={array.Count}");
        }

        return array.Count;
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>().ToList();
        var array = new HashSet<(int x, int y, int z, int w)>();
        for (var y = lines.Count - 1; y >= 0; y--)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
                if (line[x] == '#')
                    array.Add((x, y, 0, 0));
        }


        // Console.WriteLine($"======INITIAL======");
        // Dump2(array);
        for (var cycle = 1; cycle <= 6; cycle++)
        {
            var newArray = new HashSet<(int x, int y, int z, int w)>();
            //Console.WriteLine($"======CYCLE={cycle}======");
            var xmin = array.Min(t => t.x) - 1;
            var xmax = array.Max(t => t.x) + 1;
            var ymin = array.Min(t => t.y) - 1;
            var ymax = array.Max(t => t.y) + 1;
            var zmin = array.Min(t => t.z) - 1;
            var zmax = array.Max(t => t.z) + 1;
            var wmin = array.Min(t => t.w) - 1;
            var wmax = array.Max(t => t.w) + 1;
            for (var x = xmin; x <= xmax; x++)
            for (var y = ymin; y <= ymax; y++)
            for (var z = zmin; z <= zmax; z++)
            for (var w = wmin; w <= wmax; w++)
            {
                var neighbours = BoardExtensions.GetNeighbours(x, y, z, w);
                var activeCount = neighbours.Count(n => array.Contains((n.x, n.y, n.z, n.w)));
                var con = array.Contains((x, y, z, w));
                if (con && (activeCount == 2 || activeCount == 3))
                    newArray.Add((x, y, z, w));
                else if (!con && activeCount == 3)
                    newArray.Add((x, y, z, w));
            }

            array = newArray;

            //Dump2(array);
            //Console.WriteLine($"On={array.Count}");
        }

        return array.Count;
    }
}