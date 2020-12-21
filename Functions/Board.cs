using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Functions
{
    public static class BoardExtensions
    {
       
        public static Board Jump(this Board board, int x, int y)
        {
            board.SetPosition(x,y);
            return board;
        }
        public static Board Up(this Board board, int count =1)
        {
            board.MoveUp(count);
            return board;
        }
        public static Board Down(this Board board, int count =1)
        {
            board.MoveDown(count);
            return board;
        }
        public static Board Left(this Board board, int count =1)
        {
            board.MoveLeft(count);
            return board;
        }
        public static Board Right(this Board board, int count =1)
        {
            board.MoveRight(count);
            return board;
        }
        public static Board UpLeft(this Board board, int count =1)
        {
            board.MoveUpLeft(count);
            return board;
        }
        public static Board UpRight(this Board board, int count =1)
        {
            board.MoveUpRight(count);
            return board;
        }
        public static Board DownRight(this Board board, int count =1)
        {
            board.MoveDownRight(count);
            return board;
        }
        public static Board DownLeft(this Board board, int count =1)
        {
            board.MoveDownLeft(count);
            return board;
        }
        public static Board UpWhile(this Board board, Func<char, bool> predicate)
        {
            board.MoveUpWhile(predicate);
            return board;
        }
        public static Board DownWhile(this Board board, Func<char, bool> predicate)
        {
            board.MoveDownWhile(predicate);
            return board;
        }
        public static Board LeftWhile(this Board board, Func<char, bool> predicate)
        {
            board.MoveLeftWhile(predicate);
            return board;
        }
        public static Board RightWhile(this Board board, Func<char, bool> predicate)
        {
            board.MoveRightWhile(predicate);
            return board;
        }
        public static Board UpLeftWhile(this Board board, Func<char, bool> predicate)
        {
            board.MoveUpLeftWhile(predicate);
            return board;
        }
        public static Board UpRightWhile(this Board board, Func<char, bool> predicate)
        {
            board.MoveUpRightWhile(predicate);
            return board;
        }
        public static Board DownRightWhile(this Board board, Func<char, bool> predicate)
        {
            board.MoveDownRightWhile(predicate);
            return board;
        }
        public static Board DownLeftWhile(this Board board, Func<char, bool> predicate)
        {
            board.MoveDownLeftWhile(predicate);
            return board;
        }
        public static List<(int x, int y, int z, int w)> GetNeighbours(int xS, int yS, int zS, int wS)
        {
            var list = new List<(int x, int y, int z, int w)>();
            for (int x = xS -1 ; x <= xS+1; x++)
            for (int y = yS -1 ; y <= yS+1; y++)
            for (int z= zS -1 ; z <= zS+1; z++)
            for (int w= wS -1 ; w <= wS+1; w++)
            {
                if (x != xS || y != yS || z != zS || w != wS)
                    list.Add((x,y,z,w));
            }
            return list;
              
        }
        public static List<(int x, int y, int z)> GetNeighbours(int xS, int yS, int zS)
        {
            var list = new List<(int x, int y, int z)>();
            for (int x = xS -1 ; x <= xS+1; x++)
            for (int y = yS -1 ; y <= yS+1; y++)
            for (int z= zS -1 ; z <= zS+1; z++)
            {
                if (x != xS || y != yS || z != zS)
                    list.Add((x,y,z));
            }
            return list;
              
        }
        public static List<(int x, int y)> GetNeighbours(int xS, int yS)
        {
            var list = new List<(int x, int y)>();
            for (int x = xS -1 ; x <= xS+1; x++)
            for (int y = yS -1 ; y <= yS+1; y++)
            {
                if (x != xS || y != yS)
                    list.Add((x,y));
            }
            return list;
              
        }
    }
    public class Board
    {
        
        #region Fields

        private char[,] _board;

        #endregion

        #region Constructors

        public Board(int width, int height)
        {
            _board = new char[width, height];
        }

        public Board(Board from)
        {
            _board = new char[from.Width, from.Height];
            Array.Copy(from._board, _board, _board.Length);
        }

        #endregion

        #region Properties
        public IEnumerable<( int x, int y)> Positions
        {
            get
            {
                for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    yield return ( x, y);
            }
        }
        public int Width => _board.GetLength(0);
        public int Height => _board.GetLength(1);

        public char Value => XInRange(X) && YInRange(Y) ? _board[X, Y] : char.MaxValue;

        public int X { get; private set; }
        public int Y { get; private set; }
        public List<(string, Board)> BoardsLeft { get;  } = new List<(string, Board)>();
        public List<(string, Board)> BoardsRight { get;  } = new List<(string, Board)>();
        public List<(string, Board)> BoardsTop { get;  } = new List<(string, Board)>();
        public List<(string, Board)> BoardsBottom { get;  } = new List<(string, Board)>();

        #endregion

        #region Methods

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public char ValueAt(int x, int y)
        {
            if (XInRange(x) && YInRange(y))
                return _board[x, y];
            return char.MaxValue;
        }

        public bool SetValueAt(int x, int y, char ch)
        {
            if (!XInRange(x) || !YInRange(y)) return false;
            _board[x, y] = ch;
            return true;
        }

        public bool XInRange(int x)
        {
            return x >= 0 && x < _board.GetLength(0);
        }

        public bool YInRange(int y)
        {
            return y >= 0 && y < _board.GetLength(1);
        }

        public char MoveUp(int count = 1)
        {
            Y -= count;
            return Value;
        }

        public char MoveDown(int count = 1)
        {
            Y += count;
            return Value;
        }

        public char MoveLeft(int count = 1)
        {
            X -= count;
            return Value;
        }

        public char MoveRight(int count = 1)
        {
            X += count;
            return Value;
        }

        public char MoveUpLeft(int xcount = 1, int ycount = 1)
        {
            Y -= ycount;
            X -= xcount;
            return Value;
        }

        public char MoveUpRight(int xcount = 1, int ycount = 1)
        {
            Y -= ycount;
            X += xcount;
            return Value;
        }

        public char MoveDownRight(int xcount = 1, int ycount = 1)
        {
            Y += ycount;
            X += xcount;
            return Value;
        }

        public char MoveDownLeft(int xcount = 1, int ycount = 1)
        {
            Y += ycount;
            X -= xcount;
            return Value;
        }

        public bool CanMoveUp(int count = 1) => YInRange(Y - count);

        public bool CanMoveDown(int count = 1) => YInRange(Y + count);

        public bool CanMoveLeft(int count = 1) => XInRange(X - count);

        public bool CanMoveRight(int count = 1) => XInRange(X + count);

        public bool CanMoveUpLeft(int xcount = 1, int ycount = 1) => YInRange(Y - ycount) && XInRange(X - xcount);

        public bool CanMoveUpRight(int xcount = 1, int ycount = 1) => YInRange(Y - ycount) && XInRange(X + xcount);

        public bool CanMoveDownRight(int xcount = 1, int ycount = 1) => YInRange(Y + ycount) && XInRange(X + xcount);

        public bool CanMoveDownLeft(int xcount = 1, int ycount = 1) => YInRange(Y + ycount) && XInRange(X - xcount);
        
        public char MoveUpWhile(Func<char,bool> predicate)
        {
            while (true)
                if (CanMoveUp() && predicate.Invoke(Value)) MoveUp();
                else break;
            return Value;
        }

        public char MoveDownWhile(Func<char,bool> predicate)
        {
            while (true)
                if (CanMoveDown() && predicate.Invoke(Value)) MoveDown();
                else break;
            return Value;
        }

        public char MoveLeftWhile(Func<char,bool> predicate)
        {
            while (true)
                if (CanMoveLeft() && predicate.Invoke(Value)) MoveLeft();
                else break;
            return Value;
        }

        public char MoveRightWhile(Func<char,bool> predicate)
        {
            while (true)
                if (CanMoveRight() && predicate.Invoke(Value)) MoveRight();
                else break;
            return Value;
        }

        public char MoveUpLeftWhile(Func<char,bool> predicate)
        {
            while (true)
                if (CanMoveUpLeft() && predicate.Invoke(Value)) MoveUpLeft();
                else break;
            return Value;
        }

        public char MoveUpRightWhile(Func<char,bool> predicate)
        {
            while (true)
                if (CanMoveUpRight() && predicate.Invoke(Value)) MoveUpRight();
                else break;
            return Value;
        }

        public char MoveDownRightWhile(Func<char,bool> predicate)
        {
            while (true)
                if (CanMoveDownRight() && predicate.Invoke(Value)) MoveDownRight();
                else break;
            return Value;
        }

        public char MoveDownLeftWhile(Func<char,bool> predicate)
        {
            while (true)
                if (CanMoveDownLeft() && predicate.Invoke(Value)) MoveDownLeft();
                else break;
            return Value;
        }

        public void Dump()
        {
            Console.WriteLine();

            for (var y = 0; y < _board.GetLength(1); y++)
            {
                for (var x = 0; x < _board.GetLength(0); x++) 
                    Console.Write(_board[x, y]);
                Console.WriteLine();
            }
        }

        public int CountValues(char c)
        {
            var count = 0;
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (_board[x, y] == '#')
                    count++;
            return count;
        }

        #endregion

        public void LoadFromStrings(IEnumerable<string> strings)
        {
            var ss = strings.ToList();
            _board = new char[ss.Count, ss.First().Length];
            for (int y = 0; y < ss.Count; y++)
                for (int x = 0; x < ss[y].Length; x++)
                    _board[x,y] = ss[y][x];
        }

        public Board RotateACW()
        {
            var b = new Board(Height, Width);
            var x2 = 0;
            
            for (int y = 0; y < Height; y++)
            {
                var y2 = Height -1;
                
                for (int x = 0; x < Width; x++)
                {
                    b.SetValueAt(x2,y2,_board[x,y]);
                    y2--;
                }
                
                x2++;
            }

            return b;
        }
        public Board FlipX()
        {
            var b = new Board(Width,Height);

            for (int y = 0; y < Height; y++)
            {
                var x2 = 0;
                for (int x = Width - 1; x >= 0; x--)
                {
                    b.SetValueAt(x2,y,_board[x,y]);
                    x2++;
                }

            }

            return b;
        }
        public Board FlipY()
        {
            var b = new Board(Width,Height);
            for (int x = 0; x < Width; x++)
            {
                var y2 = 0;
                for (int y = Height - 1; y >= 0; y--)
                {
                    b.SetValueAt(x,y2,_board[x,y]);
                    y2++;
                }
            }

            return b;
        }

        private int _reh = -1;
        private int _leh = -1;
        private int _teh = -1;
        private int _beh = -1;
        public int RightEdgeHash()
        {
            if (_reh == -1)
            {
                var arr = new char[Height];
                for (int y = 0; y < Height; y++)
                    arr[y] = _board[Width - 1, y];
                _reh =  ((IStructuralEquatable) arr).GetHashCode(EqualityComparer<char>.Default);
            }

            return _reh;
        }
        public int LeftEdgeHash()
        {
            if (_leh == -1)
            {
                var arr = new char[Height];
                for (int y = 0; y < Height; y++)
                    arr[y] = _board[0, y];
                _leh =  ((IStructuralEquatable) arr).GetHashCode(EqualityComparer<char>.Default);
            }

            return _leh;
        } 
        public int TopEdgeHash()
        {
            if (_teh == -1)
            {
                var arr = new char[Width];
                for (int x = 0; x < Width; x++)
                    arr[x] = _board[x, Height -1];
                _teh =  ((IStructuralEquatable) arr).GetHashCode(EqualityComparer<char>.Default);
            }

            return _teh;
        }
        public int BottomEdgeHash()
        {
            if (_beh == -1)
            {
                var arr = new char[Width];
                for (int x = 0; x < Width; x++)
                    arr[x] = _board[x, 0];
                _beh =  ((IStructuralEquatable) arr).GetHashCode(EqualityComparer<char>.Default);
            }

            return _beh;
        }
    }
}