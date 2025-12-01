using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2015;

public class Advent2015Day04 : Solution
{
    public Advent2015Day04()
    {
        Answer1 = 282749;
        Answer2 = 9962624;
    }

    public override object ExecutePart1()
    {
        var input = "yzbqklnj";
        var md5 = MD5.Create();
        var answer = 0;
        while (true)
        {
            var inputBytes = Encoding.ASCII.GetBytes($"{input}{answer.ToString()}");
            var hashBytes = md5.ComputeHash(inputBytes).ToHexString();
            var areEqual = hashBytes.StartsWith("00000");

            if (areEqual)
                return answer;
            answer++;
        }
    }

    public override object ExecutePart2()
    {
        var input = "yzbqklnj";
        var md5 = MD5.Create();
        var answer = 0;
        while (true)
        {
            var inputBytes = Encoding.ASCII.GetBytes($"{input}{answer}");
            var hashBytes = md5.ComputeHash(inputBytes).ToHexString();
            var areEqual = hashBytes.StartsWith("000000");

            if (areEqual)
                return answer;
            answer++;
        }
    }
}