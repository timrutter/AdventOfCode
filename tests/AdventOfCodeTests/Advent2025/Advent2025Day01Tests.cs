using AdventOfCode.Advent2025;

namespace AdventOfCodeTests.Advent2025;

public class Advent2025Day01Tests
{
    [Fact]
    public void Should()
    {
        int count = 0;
        Assert.Equal(50, Advent2025Day01.GetPos("L50", 0, ref count));
        Assert.Equal(0, count);
        Assert.Equal(50, Advent2025Day01.GetPos("R50", 0, ref count));
        Assert.Equal(0, count);
        Assert.Equal(0, Advent2025Day01.GetPos("L100", 0, ref count));
        Assert.Equal(1, count);
        Assert.Equal(0, Advent2025Day01.GetPos("R100", 0, ref count));
        Assert.Equal(2, count);
        Assert.Equal(30, Advent2025Day01.GetPos("R20", 10, ref count));
        Assert.Equal(2, count);
        Assert.Equal(70, Advent2025Day01.GetPos("L20", 90, ref count));
        Assert.Equal(2, count);
        Assert.Equal(90, Advent2025Day01.GetPos("L20", 10, ref count));
        Assert.Equal(3, count);
        Assert.Equal(10, Advent2025Day01.GetPos("R20", 90, ref count));
        Assert.Equal(4, count);
        Assert.Equal(90, Advent2025Day01.GetPos("L220", 10, ref count));
        Assert.Equal(7, count);
        Assert.Equal(10, Advent2025Day01.GetPos("R220", 90, ref count));
        Assert.Equal(10, count);
        Assert.Equal(80, Advent2025Day01.GetPos("L220", 0, ref count));
        Assert.Equal(12, count);
        Assert.Equal(20, Advent2025Day01.GetPos("R220", 0, ref count));
        Assert.Equal(14, count);
        Assert.Equal(0, Advent2025Day01.GetPos("L10", 10, ref count));
        Assert.Equal(15, count);
        Assert.Equal(0, Advent2025Day01.GetPos("R10", 90, ref count));
        Assert.Equal(16, count);
    }
}