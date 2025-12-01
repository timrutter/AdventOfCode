using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020;

public class Advent2020Day04 : Solution
{
    public Advent2020Day04()
    {
        Answer1 = 250;
        Answer2 = 158;
    }

    public override object ExecutePart1()
    {
        var passports = DataFile.ReadAllBlankLineSeparatedRecords(true);
        var count = 0;
        var required = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };


        foreach (var passport in passports)
        {
            var bits = passport.Trim().Split(" ");
            var keys = bits.Select(b => b.Split(":")[0]).ToList();
            if (required.All(r => keys.Contains(r)))
                count++;
        }


        return count;
    }

    public override object ExecutePart2()
    {
        var passports = DataFile.ReadAllBlankLineSeparatedRecords(true);
        var count = 0;
        var required = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

        foreach (var passport in passports)
        {
            var bits = passport.Trim().Split(" ");
            var dict = bits.ToDictionary(b => b.Split(":")[0], b => b.Split(":")[1]);
            var valid = required.All(r => dict.Keys.Contains(r));
            if (!valid) continue;
            foreach (var kvp in dict)
            {
                switch (kvp.Key)
                {
                    case "byr":
                    {
                        valid &= kvp.Value.Length == 4 && int.TryParse(kvp.Value, out var yr) && yr >= 1920 &&
                                 yr <= 2002;
                        break;
                    }
                    case "iyr":
                    {
                        valid &= kvp.Value.Length == 4 && int.TryParse(kvp.Value, out var yr) && yr >= 2010 &&
                                 yr <= 2020;
                        break;
                    }
                    case "eyr":
                    {
                        valid &= kvp.Value.Length == 4 && int.TryParse(kvp.Value, out var yr) && yr >= 2020 &&
                                 yr <= 2030;
                        break;
                    }
                    case "hgt":
                    {
                        valid &= kvp.Value.EndsWith("cm")
                            ? int.TryParse(kvp.Value[..^2], out var hecm) &&
                              hecm >= 150 && hecm <= 193
                            : kvp.Value.EndsWith("in") &&
                              int.TryParse(kvp.Value[..^2], out var he) && he >= 59 &&
                              he <= 76;
                        break;
                    }
                    case "hcl":
                    {
                        valid &= kvp.Value.Length == 7 && kvp.Value.StartsWith("#") &&
                                 kvp.Value[1..].All(c => "0123456789abcdef".Contains(c));
                        break;
                    }
                    case "ecl":
                    {
                        var colours = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                        valid &= colours.Contains(kvp.Value);
                        break;
                    }
                    case "pid":
                    {
                        valid &= kvp.Value.Length == 9 && kvp.Value.All(char.IsDigit);
                        break;
                    }
                    case "cid":
                        break;
                }

                if (!valid) break;
            }

            if (valid)
                count++;
        }

        return count;
    }
}