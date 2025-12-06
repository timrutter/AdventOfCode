using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Helpers;

public static class Maths
{
    #region Methods

    public static long Product(this IEnumerable<int> list)
    {
        return list.Aggregate<int, long>(1, (current, i) => current * i);
    }

    public static long Product(this IEnumerable<long> list)
    {
        return list.Aggregate<long, long>(1, (current, i) => current * i);
    }

    public static int Factorial(this int number)
    {
        var result = 1;
        while (number != 1)
        {
            result *= number;
            number -= 1;
        }

        return result;
    }

    public static int EuclidGCD(int a, int b)
    {
        while (true)
        {
            if (b == 0) return a;
            var aRem = a % b;
            a = b;
            b = aRem;
        }
    }

    public static long EuclidGCD(long a, long b)
    {
        while (true)
        {
            if (b == 0) return a;
            var aRem = a % b;
            a = b;
            b = aRem;
        }
    }

    public static long ApplyMask(string mask, long val)
    {
        for (var index = 0; index < mask.Length; index++)
        {
            var ch = mask[index];
            switch (ch)
            {
                case '1':
                    var v = (long)1 << (36 - index - 1);
                    val |= v;
                    break;
                case '0':
                    val &= ~((long)1 << (36 - index - 1));
                    break;
            }
        }

        return val;
    }

    public static long LowestCommonMultiple(int a, int b)
    {
        var gcd = EuclidGCD(a, b);
        return a / gcd * b;
    }

    public static long LowestCommonMultiple(long a, long b)
    {
        var gcd = EuclidGCD(a, b);
        return a / gcd * b;
    }

    public static long LowestCommonMultiple(this IEnumerable<int> values)
    {
        var enumerable = values.ToList();
        if (enumerable.Count < 2)
            return 0;

        var val = LowestCommonMultiple(enumerable[0], enumerable[1]);
        for (var i = 2; i < enumerable.Count; i++) val = LowestCommonMultiple(val, enumerable[i]);

        return val;
    }


    #endregion
}