using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day24 : Solution
    {
        public Advent2020Day24()
        {
            Answer1 = 373;
            Answer2 = 3917;
        }
        private Dictionary<(int x, int y), int> GetDictionary()
        {
            var lines = DataFile.ReadAll<string>();
            Dictionary<(int x, int y), int> dict = new Dictionary<(int x, int y), int>();
            foreach (var line in lines)
            {
                var l = line;
                int x = 0;
                int y = 0;
                while (l.Length > 0)
                {
                    if (l.StartsWith("e"))
                    {
                        x++;
                        l = l.Substring(1);
                    }
                    else if (l.StartsWith("se"))
                    {
                        y--;
                        l = l.Substring(2);
                    }
                    else if (l.StartsWith("sw"))
                    {
                        x--;
                        y--;
                        l = l.Substring(2);
                    }
                    else if (l.StartsWith("w"))
                    {
                        x--;
                        l = l.Substring(1);
                    }
                    else if (l.StartsWith("nw"))
                    {
                        y++;
                        l = l.Substring(2);
                    }
                    else if (l.StartsWith("ne"))
                    {
                        y++;
                        x++;
                        l = l.Substring(2);
                    }
                }

                if (!dict.ContainsKey((x, y)))
                    dict[(x, y)] = 0;
                dict[(x, y)]++;
            }

            return dict;
        }
        public override object ExecutePart1()
        {
            var dict = GetDictionary();

            return dict.Values.Count(v => v % 2 == 1);
        }

        public override object ExecutePart2()
        {
            var hash = new HashSet<(int x, int y)>(GetDictionary().Where(d => d.Value % 2 == 1).Select(k => k.Key));

            int GetBlackCount((int x, int y) adjTile)
            {
                var bc = 0;
                if (hash.Contains((adjTile.x + 1, adjTile.y))) bc++;
                if (hash.Contains((adjTile.x, adjTile.y - 1))) bc++;
                if (hash.Contains((adjTile.x - 1, adjTile.y - 1))) bc++;
                if (hash.Contains((adjTile.x - 1, adjTile.y))) bc++;
                if (hash.Contains((adjTile.x, adjTile.y + 1))) bc++;
                if (hash.Contains((adjTile.x + 1, adjTile.y + 1))) bc++;
                return bc;
            }
            for (int i = 0; i < 100; i++)
            {
                var newHash = new HashSet<(int x, int y)>();
                foreach (var tile in hash)
                {
                    var blackCount = GetBlackCount(tile);

                    if (blackCount == 1 || blackCount == 2)
                        newHash.Add(tile);

                    void CheckAdjacentWhite((int x, int y) adjTile)
                    {
                        if (hash.Contains(adjTile)) return;
                        if (GetBlackCount(adjTile) == 2)
                            newHash.Add(adjTile);
                    }

                    CheckAdjacentWhite((tile.x + 1, tile.y));
                    CheckAdjacentWhite((tile.x, tile.y - 1));
                    CheckAdjacentWhite((tile.x - 1, tile.y - 1));
                    CheckAdjacentWhite((tile.x - 1, tile.y));
                    CheckAdjacentWhite((tile.x, tile.y + 1));
                    CheckAdjacentWhite((tile.x + 1, tile.y + 1));

                }
                hash = newHash;
            }

            return hash.Count;
        }
    }
}