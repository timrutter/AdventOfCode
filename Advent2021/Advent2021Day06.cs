using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021
{
    public class Advent2021Day06 : Solution
    {
        public Advent2021Day06()
        {
            Answer1 = 352151;
            Answer2 = 1601616884019;
        }
        public override object ExecutePart1()
        {
            var fish = File.ReadAllText(DataFile).SplitToType<int>(",").ToList();
            const int days = 80;
            for (int i = 0; i < days; i++)
            {
                var c = fish.Count;
                for (int j = 0; j < c; j++)
                {
                    fish[j]--;
                    if (fish[j] != -1) continue;
                    fish.Add(8);
                    fish[j] = 6;
                }
            }
            return fish.Count;
        }

        public override object ExecutePart2()
        {
            var fish = File.ReadAllText(DataFile).SplitToType<int>(",").ToList();
            var dictionary = Functions.Range(-1, 8).ToDictionary(i => i, i => (long)0);

            foreach (int t in fish)
            {
                dictionary[t]++;
            }
            const int days = 256;
            for (int i = 0; i < days; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    dictionary[j - 1] = dictionary[j];
                }

                dictionary[8] = dictionary[-1];
                dictionary[6] += dictionary[-1];
                dictionary[-1] = 0;
            }
            return dictionary.Values.Sum();
        }
    }
}