using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2015;

public class Advent2015Day11 : Solution
{
    public Advent2015Day11()
    {
        Answer1 = "cqjxxyzz";
        Answer2 = "cqjxjnds";
    }

    public override object ExecutePart1()
    {
        var input = "cqjxjnds";

        char IncChar(char ch)
        {
            var letNum = ch - 97;
            letNum = (letNum + 1) % 26;
            return (char)(97 + letNum);
        }

        string Increment(string str)
        {
            var chars = str.ToCharArray();
            for (var i = str.Length - 1; i >= 0; i--)
            {
                var newCh = IncChar(chars[i]);
                chars[i] = newCh;
                if (newCh != 'a')
                    return new string(chars);
            }

            return $"a{new string(chars)}";
        }

        bool IsValid(string password)
        {
            if (password.Contains("i") || password.Contains("o") || password.Contains("l")) return false;

            if (Characters.LowerAlphabet.Select(a => password.Contains($"{a}{a}")).Count(c => c) < 2) return false;

            if (Characters.LowerAlphabet.All(a => !password.Contains($"{a}{(char)(a + 1)}{(char)(a + 2)}")))
                return false;

            return true;
        }

        var newPass = Increment(input);
        while (!IsValid(newPass))
            newPass = Increment(newPass);

        newPass = Increment(newPass);
        while (!IsValid(newPass))
            newPass = Increment(newPass);
        return newPass;
    }

    public override object ExecutePart2()
    {
        return int.MaxValue;
    }
}