using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace AdventOfCode.Helpers
{
    public static class BoardExtensions
    {
        public static Board<T> LoadBoard<T>(this string fileName) where T : struct
        {
            return File.ReadAllLines(fileName).LoadBoard<T>();
        }
        public static Board<T> LoadBoard<T>(this IEnumerable<string> strings) where T : struct
        {
            var ss = strings.ToList();
            var board = new Board<T>(ss.First().Length,ss.Count );
            for (int y = 0; y < ss.Count; y++)
            for (int x = 0; x < ss[y].Length; x++)
            {
                board[x,y] = (T)Convert.ChangeType(ss[y][x].ToString(), typeof(T));
            }
            return board;
        }

        public static Board<T> LoadBoard<T>(this string fileName, string splitChar, StringSplitOptions options = StringSplitOptions.None) where T : struct
        {
            return File.ReadAllLines(fileName).LoadBoard<T>(splitChar, options);
        }

        public static Board<T> LoadBoard<T>(this IEnumerable<string> strings,string splitChar, StringSplitOptions options = StringSplitOptions.None) where T : struct
        {
            var ss = strings.ToList();
            var board = new Board<T>(ss.Count, ss.First().Split(splitChar, options).Length);
            for (int y = 0; y < ss.Count; y++)
            {
                var bits = ss[y].Split(splitChar);
                for (int x = 0; x < bits.Length; x++)
                    board[x, y] = (T) Convert.ChangeType(bits[x], typeof(T));
            }

            return board;
        }

        public static Board<T> Jump<T>(this Board<T> board, int x, int y) where T: struct
        {
            board.SetPosition(x,y);
            return board;
        }
        public static Board<T> Up<T>(this Board<T> board, int count =1) where T : struct
        {
            board.MoveUp(count);
            return board;
        }
        public static Board<T> Down<T>(this Board<T> board, int count =1) where T : struct
        {
            board.MoveDown(count);
            return board;
        }
        public static Board<T> Left<T>(this Board<T> board, int count =1) where T : struct
        {
            board.MoveLeft(count);
            return board;
        }
        public static Board<T> Right<T>(this Board<T> board, int count =1) where T : struct
        {
            board.MoveRight(count);
            return board;
        }
        public static Board<T> UpLeft<T>(this Board<T> board, int count =1) where T : struct
        {
            board.MoveUpLeft(count);
            return board;
        }
        public static Board<T> UpRight<T>(this Board<T> board, int count =1) where T : struct
        {
            board.MoveUpRight(count);
            return board;
        }
        public static Board<T> DownRight<T>(this Board<T> board, int count =1) where T : struct
        {
            board.MoveDownRight(count);
            return board;
        }
        public static Board<T> DownLeft<T>(this Board<T> board, int count =1) where T : struct
        {
            board.MoveDownLeft(count);
            return board;
        }
        public static Board<T> UpWhile<T>(this Board<T> board, Func<T, bool> predicate) where T : struct
        {
            board.MoveUpWhile(predicate);
            return board;
        }
        public static Board<T> DownWhile<T>(this Board<T> board, Func<T, bool> predicate) where T : struct
        {
            board.MoveDownWhile(predicate);
            return board;
        }
        public static Board<T> LeftWhile<T>(this Board<T> board, Func<T, bool> predicate) where T : struct
        {
            board.MoveLeftWhile(predicate);
            return board;
        }
        public static Board<T> RightWhile<T>(this Board<T> board, Func<T, bool> predicate) where T : struct
        {
            board.MoveRightWhile(predicate);
            return board;
        }
        public static Board<T> UpLeftWhile<T>(this Board<T> board, Func<T, bool> predicate) where T : struct
        {
            board.MoveUpLeftWhile(predicate);
            return board;
        }
        public static Board<T> UpRightWhile<T>(this Board<T> board, Func<T, bool> predicate) where T : struct
        {
            board.MoveUpRightWhile(predicate);
            return board;
        }
        public static Board<T> DownRightWhile<T>(this Board<T> board, Func<T, bool> predicate) where T : struct
        {
            board.MoveDownRightWhile(predicate);
            return board;
        }
        public static Board<T> DownLeftWhile<T>(this Board<T> board, Func<T, bool> predicate) where T : struct
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
        public static List<Point> GetNeighbours(int xS, int yS)
        {
            var list = new List<Point>();
            for (int x = xS -1 ; x <= xS+1; x++)
            for (int y = yS -1 ; y <= yS+1; y++)
            {
                if (x != xS || y != yS)
                    list.Add(new Point(x,y));
            }
            return list;
              
        }
    }
    public class Board<T> where T:struct
    {
        #region Fields

        private T[,] _board;

        #endregion

        #region Constructors

        public Board(int width, int height, Func<Point, int, T> initFunc = null)
        {
            _board = new T[width, height];
            if (initFunc == null) return;
            int i = 0;
            foreach (var boardPosition in Positions)
                SetValueAt(boardPosition, initFunc(boardPosition, i++));
        }

        public Board(Board<T> from)
        {
            _board = new T[from.Width, from.Height];
            Array.Copy(from._board, _board, _board.Length);
        }

        #endregion

        #region Properties
        public IEnumerable<Point> Positions
        {
            get
            {
                for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    yield return new Point(x, y);
            }
        }

        public void SetValues(IEnumerable<T> values)
        {
            var l = values.ToList();
            for (int i = 0; i < l.Count; i++)
            {
                SetValueAt(i % Width,  i / Height , l[i]);
            }
        }
        public IEnumerable<T> Values
        {
            get
            {
                for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                {
                    yield return ValueAt(x,y);
                }
            }
        }
        public IEnumerable<(Point pos, T value)> ValuesAndPositions
        {
            get
            {
                for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                {
                    yield return (new Point(x, y), value: ValueAt(x, y));
                }
            }
        }

        public T this[int x, int y]
        {
            get => ValueAt(x, y);
            set => SetValueAt(x, y, value);
        }

        public int Width => _board.GetLength(0);
        public int Height => _board.GetLength(1);

        public T Value
        {
            get
            {
                if (XInRange(X) && YInRange(Y)) return _board[X, Y];
                return default;
            }
            set
            {
                if (!XInRange(X) || !YInRange(Y)) return ;
                _board[X, Y] = value;
            }
        }

        public int X { get; set; }
        public int Y { get; set; }

        public IEnumerable<T> GetRow(int y)
        {
            for (int x = 0; x < _board.GetLength(0); x++)
            {
                yield return ValueAt(x, y);
            }
        }
        public IEnumerable<IEnumerable<T>> GetRows()
        {
            for (int y = 0; y < _board.GetLength(1); y++)
            {
                yield return GetRow(y);
            }
        }

        public IEnumerable<T> GetColumn(int x)
        {
            for (int y = 0; y < _board.GetLength(1); y++)
            {
                yield return ValueAt(x, y);
            }
        }
        public IEnumerable<IEnumerable<T>> GetColumns()
        {
            for (int x = 0; x < _board.GetLength(0); x++)
            {
                yield return GetColumn(x);
            }
        }

        #endregion

        #region Methods

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public T ValueAt(int x, int y)
        {
            if (XInRange(x) && YInRange(y))
                return _board[x, y];
            return default;
        }


        public T ValueAt(Point pos)
        {
            if (XInRange(pos.X) && YInRange(pos.Y))
                return _board[pos.X, pos.Y];
            return default;
        }

        public bool TryGetValueAt(int x, int y, out T val)
        {
            if (!XInRange(x) || !YInRange(y))
            {
                val = default;
                return false;
            }
            val = _board[x, y];
            return true;
        }
        public bool TryGetValueAt(Point pos, out T val)
        {
            if (!XInRange(pos.X) || !YInRange(pos.Y))
            {
                val = default;
                return false;
            }
            val = _board[pos.X, pos.Y];
            return true;
        }

        public bool SetValueAt(int x, int y, T ch)
        {
            if (!XInRange(x) || !YInRange(y)) return false;
            _board[x, y] = ch;
            return true;
        }

        public bool SetValueAt(Point pos, T ch)
        {
            if (!XInRange(pos.X) || !YInRange(pos.Y)) return false;
            _board[pos.X, pos.Y] = ch;
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


        public bool PositionInRange(Point pos)
        {
            return XInRange(pos.X) && YInRange(pos.Y);
        }
        public bool PositionInRange(int x, int y)
        {
            return XInRange(x) && YInRange(y);
        }

        public T MoveUp(int count = 1)
        {
            Y -= count;
            return Value;
        }

        public T MoveDown(int count = 1)
        {
            Y += count;
            return Value;
        }

        public T MoveLeft(int count = 1)
        {
            X -= count;
            return Value;
        }

        public T MoveRight(int count = 1)
        {
            X += count;
            return Value;
        }

        public T MoveUpLeft(int xcount = 1, int ycount = 1)
        {
            Y -= ycount;
            X -= xcount;
            return Value;
        }

        public T MoveUpRight(int xcount = 1, int ycount = 1)
        {
            Y -= ycount;
            X += xcount;
            return Value;
        }

        public T MoveDownRight(int xcount = 1, int ycount = 1)
        {
            Y += ycount;
            X += xcount;
            return Value;
        }

        public T MoveDownLeft(int xcount = 1, int ycount = 1)
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
        
        public T MoveUpWhile(Func<T,bool> predicate)
        {
            while (true)
                if (CanMoveUp() && predicate.Invoke(Value)) MoveUp();
                else break;
            return Value;
        }

        public T MoveDownWhile(Func<T,bool> predicate)
        {
            while (true)
                if (CanMoveDown() && predicate.Invoke(Value)) MoveDown();
                else break;
            return Value;
        }

        public T MoveLeftWhile(Func<T,bool> predicate)
        {
            while (true)
                if (CanMoveLeft() && predicate.Invoke(Value)) MoveLeft();
                else break;
            return Value;
        }

        public T MoveRightWhile(Func<T,bool> predicate)
        {
            while (true)
                if (CanMoveRight() && predicate.Invoke(Value)) MoveRight();
                else break;
            return Value;
        }

        public T MoveUpLeftWhile(Func<T,bool> predicate)
        {
            while (true)
                if (CanMoveUpLeft() && predicate.Invoke(Value)) MoveUpLeft();
                else break;
            return Value;
        }

        public T MoveUpRightWhile(Func<T,bool> predicate)
        {
            while (true)
                if (CanMoveUpRight() && predicate.Invoke(Value)) MoveUpRight();
                else break;
            return Value;
        }

        public T MoveDownRightWhile(Func<T,bool> predicate)
        {
            while (true)
                if (CanMoveDownRight() && predicate.Invoke(Value)) MoveDownRight();
                else break;
            return Value;
        }

        public T MoveDownLeftWhile(Func<T,bool> predicate)
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

        public int CountValues(T c)
        {
            return ValuesAndPositions.Count(v => v.value.Equals(c));
        }

        #endregion


        public Board<T> RotateAcw()
        {
            var b = new Board<T>(Height, Width);
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
        public Board<T> FlipX()
        {
            var b = new Board<T>(Width,Height);

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
        public Board<T> FlipY()
        {
            var b = new Board<T>(Width,Height);
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
                int hash = 0;
                for (int y = 0; y < Height; y++)
                    hash += HashCode.Combine(hash, _board[Width - 1, y]);
                _reh = hash;
            }

            return _reh;
        }
        public int LeftEdgeHash()
        {
            if (_leh == -1)
            {
                int hash = 0;
                for (int y = 0; y < Height; y++)
                    hash += HashCode.Combine(hash, _board[0, y]);
                _leh = hash;
            }

            return _leh;
        } 
        public int TopEdgeHash()
        {
            if (_teh == -1)
            {
                int hash = 0;
                for (int x = 0; x < Width; x++)
                    hash += HashCode.Combine(hash, _board[x, 0]);
                _teh = hash;
            }

            return _teh;
        }
        public int BottomEdgeHash()
        {
            if (_beh == -1)
            {
                int hash = 0;
                for (int x = 0; x < Width; x++)
                    hash += HashCode.Combine(hash, _board[x, Height -1]);
                _beh = hash;
            }

            return _beh;
        }

        public int Count(Func<T, bool> func)
        {
            return Values.Count(func);
        }

        public bool PositionIsValid => XInRange(X) && YInRange(Y);
        public Point BottomRight => new Point(Width - 1, Height - 1);
        public Point TopRight => new Point(Width - 1, 0);
        public Point BottomLeft => new Point(0, Height - 1);
        public Point TopLeft => new Point(0,0);

        public string WriteToString(string rowConcatenator = "\r\n", string columnConcatenator = "")
        {
            return string.Join(rowConcatenator, GetRows().Select(r => string.Join(columnConcatenator, r)));
        }

        public (Board<T> board1, Board<T> board2) SplitAtXLine(int x)
        {
            var board1 = new Board<T>(x, Height);
            var board2 = new Board<T>(Width - x - 1, Height);
            foreach (var (pos, value) in ValuesAndPositions)
            {
                if (board1.PositionInRange(pos)) board1.SetValueAt(pos, value);
                else board2.SetValueAt(pos.X - x - 1, pos.Y, value);
            }

            return (board1, board2);
        }

        public (Board<T> board1, Board<T> board2) SplitAtYLine(int y)
        {
            var board1 = new Board<T>(Width, y);
            var board2 = new Board<T>(Width, Height - y - 1);
            foreach (var p in ValuesAndPositions)
            {
                if (board1.PositionInRange(p.pos)) board1.SetValueAt(p.pos, p.value);
                else board2.SetValueAt(p.pos.X, p.pos.Y - y - 1, p.value);
            }

            return (board1, board2);
        }
    }
}