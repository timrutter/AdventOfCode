using AdventOfCode.Advent2021;
using AdventOfCode.Advent2025;

namespace AdventOfCodeTests.Advent2025;

public class Advent2025Tests
{
    [Fact]
    public void Day01Should()
    {
        var count = 0;
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

    [Fact]
    public void Day02ShouldIsWrong()
    {
        Assert.False(Advent2025Day02.IsWrong(12));
        Assert.False(Advent2025Day02.IsWrong(1698528));
        Assert.False(Advent2025Day02.IsWrong(115));
        Assert.False(Advent2025Day02.IsWrong(101));

        Assert.True(Advent2025Day02.IsWrong(1188511885));
        Assert.True(Advent2025Day02.IsWrong(222222));
        Assert.True(Advent2025Day02.IsWrong(446446));
        Assert.True(Advent2025Day02.IsWrong(38593859));
    }

    [Fact]
    public void Day02ShouldIsWrong2()
    {
        Assert.False(Advent2025Day02.IsWrong2(12));
        Assert.False(Advent2025Day02.IsWrong2(1698528));
        Assert.False(Advent2025Day02.IsWrong2(115));
        Assert.False(Advent2025Day02.IsWrong2(101));

        Assert.True(Advent2025Day02.IsWrong2(1188511885));
        Assert.True(Advent2025Day02.IsWrong2(222222));
        Assert.True(Advent2025Day02.IsWrong2(446446));
        Assert.True(Advent2025Day02.IsWrong2(38593859));
        Assert.True(Advent2025Day02.IsWrong2(123123123));
        Assert.True(Advent2025Day02.IsWrong2(1212121212));
        Assert.True(Advent2025Day02.IsWrong2(1111111));
    }

    [Fact]
    public void Day04Should()
    {
        Assert.Equal(3, Advent2025Day03.GetMax("23", 1));
        Assert.Equal(43, Advent2025Day03.GetMax("243", 2));
        Assert.Equal(987654321111, Advent2025Day03.GetMax("987654321111111", 12));
        Assert.Equal(811111111119, Advent2025Day03.GetMax("811111111111119", 12));
        Assert.Equal(434234234278, Advent2025Day03.GetMax("234234234234278", 12));
        Assert.Equal(888911112111, Advent2025Day03.GetMax("818181911112111", 12));


        Assert.Equal(998767332373,
            Advent2025Day03.GetMax(
                "2433733322122313275322425557825226339325552533247133133749552235546742511874255634664556254127332373",
                12));
        Assert.Equal(999994444284,
            Advent2025Day03.GetMax(
                "3522942223322645227152924923662823415542242241332558832344533212792344324625537266163552229423444284",
                12));
        Assert.Equal(955522553442,
            Advent2025Day03.GetMax(
                "2231522222122331262221211664522262242625232222221332224222254432223362222322222911252222551222553442",
                12));
        Assert.Equal(976531423226,
            Advent2025Day03.GetMax(
                "2473223324383134323333232322536353138286353628313413243292544234432314332342341424434427632531423226",
                12));
    }

    [Fact]
    public void Day05ShouldOverlaps()
    {
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(0,11)));
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(1,11)));
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(2,11)));
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(2,10)));
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(2,09)));
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(2,11)));
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(2,11)));
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(2,11)));
        Assert.True(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(2,9)));
        Assert.False(new AdventOfCode.Helpers.Range(1,10).Overlaps(new AdventOfCode.Helpers.Range(11,20)));
        
        
        Assert.Equal(0,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(0,11)).Min);
        Assert.Equal(1, new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(1,11)).Min);
        Assert.Equal(1,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,11)).Min);
        Assert.Equal(1,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,10)).Min);
        Assert.Equal(1,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,09)).Min);
        Assert.Equal(1,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,11)).Min);
        Assert.Equal(1,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,11)).Min);
        Assert.Equal(1,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,11)).Min);
        Assert.Equal(1,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,9)).Min);
        
        
        Assert.Equal(11,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(0,11)).Max);
        Assert.Equal(11, new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(1,11)).Max);
        Assert.Equal(11,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,11)).Max);
        Assert.Equal(10,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,10)).Max);
        Assert.Equal(10,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,09)).Max);
        Assert.Equal(11,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,11)).Max);
        Assert.Equal(11,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,11)).Max);
        Assert.Equal(11,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,11)).Max);
        Assert.Equal(10,new AdventOfCode.Helpers.Range(1,10).CombineRanges(new AdventOfCode.Helpers.Range(2,9)).Max);
    }

    [Fact]
    public void Day06Should()
    {
    }

    [Fact]
    public void Day07Should()
    {
    }

    [Fact]
    public void Day08Should()
    {
    }

    [Fact]
    public void Day09Should()
    {
    }

    [Fact]
    public void Day10Should()
    {
    }

    [Fact]
    public void Day11Should()
    {
    }

    [Fact]
    public void Day12Should()
    {
    }
}