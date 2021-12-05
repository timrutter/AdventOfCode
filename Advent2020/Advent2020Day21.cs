using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day21 : Solution
    {
        public Advent2020Day21()
        {
            Answer1 = 2078;
            Answer2 = "lmcqt,kcddk,npxrdnd,cfb,ldkt,fqpt,jtfmtpd,tsch";
        }
        public override object ExecutePart1()
        {
            var lines = DataFile.ReadAll<string>().ToList();
            var lines2 = new List<(List<string> ingredients, List<string> allergens)>();
            foreach (var line in lines)
            {
                var split = line.Split("(contains ");
                lines2.Add((split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    split[1].TrimEnd(')').Split(", ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()));
            }

            var allIngredients = lines2.SelectMany(f => f.ingredients).Distinct();
            var allAllergens = lines2.SelectMany(l => l.allergens).Distinct()
                .ToDictionary(d => d, d => new List<string>());
            foreach (var allergen in allAllergens.Keys.ToList())
            {
                var foods = lines2.Where(l => l.allergens.Contains(allergen)).ToList();
                var commonIngredients = foods
                    .SelectMany(f => f.ingredients).Distinct().Where(f => foods.All(f2 => f2.ingredients.Contains(f)));
                allAllergens[allergen] = commonIngredients.ToList();
            }

            var dictionary = new Dictionary<string, string>();
            while (allAllergens.Any())
                foreach (var allAllergen in allAllergens.Keys.ToList())
                {
                    var unknown = allAllergens[allAllergen].Except(dictionary.Values).ToList();
                    if (unknown.Count == 1)
                    {
                        dictionary[allAllergen] = unknown.First();
                        allAllergens.Remove(allAllergen);
                    }
                }

            var noAllergens = allIngredients.Except(dictionary.Values).ToList();
            return noAllergens.Sum(a => lines2.Sum(l2 => l2.ingredients.Count(i => i == a)));
        }

        public override object ExecutePart2()
        {
            var lines = DataFile.ReadAll<string>().ToList();
            var lines2 = new List<(List<string> ingredients, List<string> allergens)>();
            foreach (var line in lines)
            {
                var split = line.Split("(contains ");
                lines2.Add((split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    split[1].TrimEnd(')').Split(", ").Where(s => !string.IsNullOrWhiteSpace(s)).ToList()));
            }

            var allAllergens = lines2.SelectMany(l => l.allergens).Distinct()
                .ToDictionary(d => d, d => new List<string>());
            foreach (var allergen in allAllergens.Keys.ToList())
            {
                var foods = lines2.Where(l => l.allergens.Contains(allergen)).ToList();
                var commonIngredients = foods
                    .SelectMany(f => f.ingredients).Distinct().Where(f => foods.All(f2 => f2.ingredients.Contains(f)));
                allAllergens[allergen] = commonIngredients.ToList();
            }

            var dictionary = new Dictionary<string, string>();
            while (allAllergens.Any())
                foreach (var allAllergen in allAllergens.Keys.ToList())
                {
                    var unknown = allAllergens[allAllergen].Except(dictionary.Values).ToList();
                    if (unknown.Count == 1)
                    {
                        dictionary[allAllergen] = unknown.First();
                        allAllergens.Remove(allAllergen);
                    }
                }

            var allergens = dictionary.Keys.ToList();
            allergens.Sort();
            var ings = allergens.Select(a => dictionary[a]).ToList();
            return string.Join(",", ings);
        }
    }
}