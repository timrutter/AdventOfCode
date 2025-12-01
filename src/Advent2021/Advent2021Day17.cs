using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day17 : Solution
{
    public Advent2021Day17()
    {
        Answer1 = 6555;
        Answer2 = 4973;
    }

    public (int x, int y, int maxHeight)? Launch(int xSpeed, int ySpeed, int xmin, int xmax, int ymin, int ymax)
    {
        int x = 0, y = 0;
        var maxHeight = int.MinValue;
        while (true)
        {
            x += xSpeed;
            y += ySpeed;
            if (y > maxHeight) maxHeight = y;
            if (xSpeed > 0) xSpeed--;
            if (xSpeed < 0) xSpeed++;
            ySpeed--;
            if (x >= xmin && x <= xmax && y >= ymin && y <= ymax) return (x, y, maxHeight);

            if (x > xmax || y < ymin) return null;
        }
    }

    public override object ExecutePart1()
    {
        const int xmin = 207;
        const int xmax = 263;
        const int ymin = -115;
        const int ymax = -63;
        var maxHeight = int.MinValue;
        foreach (var xspeed in Functions.Range(xmax + 1, 1))
        foreach (var yspeed in Functions.Range(1, 300))
        {
            var ret = Launch(xspeed, yspeed, xmin, xmax, ymin, ymax);
            if (ret != null && ret.Value.maxHeight > maxHeight)
                maxHeight = ret.Value.maxHeight;
        }

        return maxHeight;
    }

    public override object ExecutePart2()
    {
        const int xmin = 207;
        const int xmax = 263;
        const int ymin = -115;
        const int ymax = -63;
        return Functions.Range(xmax + 1, 1).Sum(xspeed =>
            Functions.Range(-2000, 2000).Count(yspeed => Launch(xspeed, yspeed, xmin, xmax, ymin, ymax) != null));
    }
}