using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Functions;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day20 : Solution
    {
        public Advent2020Day20()
        {
            Answer1 = 104831106565027;
            Answer2 = 2093;
        }
        private class Board2 : IEquatable<Board2>
        {
            #region Constructors

            public Board2(int leftHash, int rightHash, int bottomHash, int topHash, string id, Board<char> board)
            {
                LeftEdgeHash = leftHash;
                RightEdgeHash = rightHash;
                TopEdgeHash = topHash;
                BottomEdgeHash = bottomHash;
                ID = id;
                Board = board;
            }

            #endregion

            #region Properties

            public int LeftEdgeHash { get; }
            public int RightEdgeHash { get; }
            public int TopEdgeHash { get; }
            public int BottomEdgeHash { get; }
            public string ID { get; }
            public Board<char> Board { get; }

            #endregion

            #region Methods

            public override string ToString()
            {
                return $"{ID} {GetHashCode()}";
            }

            public bool Equals(Board2 other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return LeftEdgeHash == other.LeftEdgeHash && RightEdgeHash == other.RightEdgeHash &&
                       TopEdgeHash == other.TopEdgeHash && BottomEdgeHash == other.BottomEdgeHash;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Board2)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(LeftEdgeHash, RightEdgeHash, TopEdgeHash, BottomEdgeHash);
            }

            #endregion
        }
        private Dictionary<string, Board<char>> Puzzle20LoadData()
        {
            var strings = DataFile.ReadAll<string>();
            var dict = new Dictionary<string, Board<char>>();
            for (var i = 0; i < strings.Length; i++)
            {
                var s = strings[i];
                i++;
                var b = strings.Skip(i).Take(10).LoadCharBoard();
                i += 10;
                dict.Add(s, b);
            }

            return dict;
        }

        private static List<Board<char>> GetAllBoards(Board<char> b)
        {
            var ret = new List<Board<char>> { b, b.RotateACW() };
            ret.Add(ret[1].RotateACW());
            ret.Add(ret[2].RotateACW());
            ret.Add(ret[0].FlipX());
            ret.Add(ret[0].FlipY());
            ret.Add(ret[1].FlipX());
            ret.Add(ret[1].FlipY());
            ret.Add(ret[2].FlipX());
            ret.Add(ret[2].FlipY());
            ret.Add(ret[3].FlipX());
            ret.Add(ret[3].FlipY());
            return ret;
        }
        public override object ExecutePart1()
        {
            var dict1 = Puzzle20LoadData();
            var size = (int)Math.Sqrt(dict1.Count);

            var allBoards = dict1.SelectMany(kvp => GetAllBoards(kvp.Value)
                .Select(b => new Board2(b.LeftEdgeHash(), b.RightEdgeHash(), b.BottomEdgeHash(),
                    b.TopEdgeHash(), kvp.Key, b)).Distinct()).ToList();

            List<Board2[,]> FillNextSquare(List<Board2[,]> options, int x, int y)
            {
                while (true)
                {
                    var ret = new List<Board2[,]>();
                    foreach (var o in options)
                    {
                        foreach (var board2 in allBoards.Where(b => o.Cast<Board2>().All(b2 => b2 == null || b.ID != b2.ID)))
                            if ((x == 0 || board2.LeftEdgeHash == o[x - 1, y].RightEdgeHash) && (y == 0 || board2.TopEdgeHash == o[x, y - 1].BottomEdgeHash))
                            {
                                var newO = new Board2[size, size];
                                for (var i = 0; i < size; i++)
                                    for (var j = 0; j < size; j++)
                                        newO[i, j] = o[i, j];
                                newO[x, y] = board2;
                                ret.Add(newO);
                            }
                    }

                    Console.WriteLine(ret.Count);
                    if (x + 1 == size)
                    {
                        x = 0;
                        y++;
                    }
                    else
                    {
                        x++;
                    }

                    if (y == size) return ret;
                    options = ret;
                }

            }


            var arrayOptions = FillNextSquare(allBoards.Select(b =>
            {
                var ret = new Board2[size, size];
                ret[0, 0] = b;
                return ret;
            }).ToList(), 1, 0);

            var first = arrayOptions[0];
            for (var y = 0; y < first.GetLength(1) * 10; y++)
            {
                for (var x = 0; x < first.GetLength(0) * 10; x++)

                {
                    var board = first[x / 10, y / 10].Board.FlipY();

                    Console.Write(board.ValueAt(x % 10, y % 10));
                    if (x % 10 == 9)
                        Console.Write(" ");
                }

                Console.WriteLine();
                if (y % 10 == 9)
                    Console.WriteLine();
            }

            for (var y = 0; y < first.GetLength(1); y++)
            {
                for (var x = 0; x < first.GetLength(0); x++)
                {
                    Console.Write(first[x, y].ID);
                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            var num =
                long.Parse(arrayOptions[0][0, 0].ID.Substring(5, 4)) *
                long.Parse(arrayOptions[0][0, size - 1].ID.Substring(5, 4)) *
                long.Parse(arrayOptions[0][size - 1, 0].ID.Substring(5, 4)) *
                long.Parse(arrayOptions[0][size - 1, size - 1].ID.Substring(5, 4));
            return num;
        }

        public override object ExecutePart2()
        {
            var strings = "Advent2020\\Data\\Day20a.txt".ReadAll<string>();
            var size = 12;
            var boardStart = new Board<char>(size * 8, size * 8, char.MaxValue);
            var x2 = 0;
            var y2 = 0;
            for (var y = 0; y < strings.Length; y++)
            {
                if (y % 10 == 0 || y % 10 == 9) continue;
                for (var x = 0; x < strings[0].Length; x++)
                {
                    if (x % 10 == 0 || x % 10 == 9) continue;
                    boardStart.SetValueAt(x2, y2, strings[y][x]);
                    x2++;
                }

                y2++;
                x2 = 0;
            }

            boardStart.Dump();
            var boards = GetAllBoards(boardStart);

            var pattern = new List<string>
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            Board<char> boardWithMonsters = null;
            var count = 0;
            foreach (var board in boards)
            {
                for (var x = 0; x < size * 8 - pattern[0].Length; x++)
                {
                    for (var y = 0; y < size * 8 - 3; y++)
                    {
                        var match = true;

                        for (var yP = 0; yP < pattern.Count; yP++)
                            for (var xP = 0; xP < pattern[0].Length; xP++)
                            {
                                if (pattern[yP][xP] != '#') continue;
                                if (board.ValueAt(x + xP, y + yP) != '#')
                                    match = false;
                            }

                        if (match)
                        {
                            count++;
                            for (var yP = 0; yP < pattern.Count; yP++)
                                for (var xP = 0; xP < pattern[0].Length; xP++)
                                {
                                    if (pattern[yP][xP] != '#') continue;
                                    board.SetValueAt(x + xP, y + yP, 'O');
                                }

                        }
                    }

                }

                if (count > 0)
                {
                    boardWithMonsters = board;
                    break;

                }
            }

            var countRough = 0;
            for (var x = 0; x < size * 8; x++)
            {
                for (var y = 0; y < size * 8; y++)
                {
                    if (boardWithMonsters?.ValueAt(x, y) == '#')
                        countRough++;
                }

            }
            return countRough;
        }
    }
}