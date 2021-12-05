using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day22 : Solution
    {
        public Advent2020Day22()
        {
            Answer1 = 33559;
            Answer2 = 32789;
        }
        private static List<int> _primes = new List<int>
        {
            2,3,5,7,11,13,17,19,23,29
            ,31,37,41,43,47,53,59,61,67,71
            ,73,79,83,89,97,101,103,107,109,113
            ,127,131,137,139,149,151,157,163,167,173
            ,179,181,191,193,197,199,211,223,227,229
            ,233,239,241,251,257,263,269,271,277,281
            ,283,293,307,311,313,317,331,337,347,349
            ,353,359,367,373,379,383,389,397,401,409
            ,419,421,431,433,439,443,449,457,461,463
            ,467,479,487,491,499,503,509,521,523,541
            ,547,557,563,569,571,577,587,593,599,601
            ,607,613,617,619,631,641,643,647,653,659
            ,661,673,677,683,691,701,709,719,727,733
            ,739,743,751,757,761,769,773,787,797,809
            ,811,821,823,827,829,839,853,857,859,863
            ,877,881,883,887,907,911,919,929,937,941
            ,947,953,967,971,977,983,991,997,1009,1013
        };
        private static int GetHash(List<short> player1Cards, List<short> player2Cards)
        {
            int hc = player1Cards.Count * _primes[101] + player2Cards.Count * _primes[102];
            int hc2 = 0;
            int hc3 = 0;
            for (int i = 0; i < player1Cards.Count; ++i)
            {
                hc2 += unchecked(_primes[i] * player1Cards[i]);
            }
            for (int i = 0; i < player2Cards.Count; ++i)
            {
                hc3 += unchecked(_primes[(i + player1Cards.Count)] * player2Cards[i]);
            }

            hc = unchecked(_primes.Last() * hc + _primes[19] * hc2 + _primes[20] * hc3);
            return hc;
        }
        private static int Combat(List<short> player1Cards, List<short> player2Cards, Dictionary<int, int> seenBefore)
        {
            HashSet<int> seenThis = new HashSet<int>();
            var startHash = GetHash(player1Cards, player2Cards);
            if (seenBefore.ContainsKey(startHash))
            {
                return seenBefore[startHash];
            }
            var hash = startHash;
            int winner = 1;
            while (player1Cards.Any() && player2Cards.Any())
            {
                if (seenThis.Contains(hash))
                {
                    seenBefore.Add(startHash, 1);
                    return 1;
                }
                seenThis.Add(hash);
                var c1 = player1Cards.First();
                var c2 = player2Cards.First();
                if (player1Cards.Count >= c1 + 1 && player2Cards.Count >= c2 + 1)
                    winner = Combat(player1Cards.Skip(1).Take(c1).ToList(), player2Cards.Skip(1).Take(c2).ToList(), seenBefore);
                else if (player1Cards[0] > player2Cards[0])
                    winner = 1;
                else
                    winner = 2;

                if (winner == 1)
                {
                    player1Cards.Add(c1);
                    player1Cards.Add(c2);
                }
                else
                {
                    player2Cards.Add(c2);
                    player2Cards.Add(c1);
                }
                player1Cards.RemoveAt(0);
                player2Cards.RemoveAt(0);
                hash = GetHash(player1Cards, player2Cards);
            }

            seenBefore.Add(startHash, winner);
            return winner;

        }
        public override object ExecutePart1()
        {// var strings = DataFile.ReadAll<string>();
            // var player1Cards = strings.Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ReadAll<int>().ToList();
            // var player2Cards = strings.Skip(3 + player1Cards.Count()).ReadAll<int>().ToList();
            //
            // while (player1Cards.Count() > 0 && player2Cards.Count() > 0)
            // {
            //     if (player1Cards[0] > player2Cards[0])
            //     {
            //         player1Cards.Add(player1Cards[0]);
            //         player1Cards.Add(player2Cards[0]);
            //     }
            //     else 
            //     {
            //         player2Cards.Add(player2Cards[0]);
            //         player2Cards.Add(player1Cards[0]);
            //     }
            //     player1Cards.RemoveAt(0);
            //     player2Cards.RemoveAt(0);
            // }
            //
            // long score = 0;
            // if (player1Cards.Count > 0)
            //     for (int i = player1Cards.Count - 1; i >= 0; i--)
            //     {
            //         score += player1Cards[i] * (player1Cards.Count - i);
            //     }
            // else 
            //     for (int i = player2Cards.Count - 1; i >= 0; i--)
            //     {
            //         score += player2Cards[i] * (player2Cards.Count - i);
            //     }
            // return score;
            return -1;
        }

        public override object ExecutePart2()
        {
            var strings = DataFile.ReadAll<string>();
            var player1Cards = strings.Skip(1).TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ReadAll<short>().ToList();
            var player2Cards = strings.Skip(3 + player1Cards.Count).ReadAll<short>().ToList();

            var winner = Combat(player1Cards, player2Cards, new Dictionary<int, int>());

            long score = 0;
            var cards = (winner == 1 ? player1Cards : player2Cards);
            for (int i = cards.Count - 1; i >= 0; i--)
                score += cards[i] * (cards.Count - i);

            return score;
        }
    }
}