using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Helpers;

public static class BoardExtensions
{
    public static Board<T> LoadBoard<T>(this string fileName) where T : struct
    {
        return File.ReadAllLines(fileName).LoadBoard<T>();
    }

    public static Board<T> LoadBoard<T>(this IEnumerable<string> strings) where T : struct
    {
        var ss = strings.ToList();
        var board = new Board<T>(ss.First().Length, ss.Count);
        for (var y = 0; y < ss.Count; y++)
        for (var x = 0; x < ss[y].Length; x++)
            board[x, y] = (T)Convert.ChangeType(ss[y][x].ToString(), typeof(T));
        return board;
    }

    public static Board<T> LoadBoard<T>(this string fileName, string splitChar,
        StringSplitOptions options = StringSplitOptions.None) where T : struct
    {
        return File.ReadAllLines(fileName).LoadBoard<T>(splitChar, options);
    }

    public static Board<T> LoadBoard<T>(this IEnumerable<string> strings, string splitChar,
        StringSplitOptions options = StringSplitOptions.None) where T : struct
    {
        var ss = strings.ToList();
        var board = new Board<T>(ss.Count, ss.First().Split(splitChar, options).Length);
        for (var y = 0; y < ss.Count; y++)
        {
            var bits = ss[y].Split(splitChar);
            for (var x = 0; x < bits.Length; x++)
                board[x, y] = (T)Convert.ChangeType(bits[x], typeof(T));
        }

        return board;
    }

    extension<T>(Board<T> board) where T : struct
    {
        public Board<T> Jump(int x, int y)
        {
            board.SetPosition(x, y);
            return board;
        }

        public Board<T> Up(int count = 1)
        {
            board.MoveUp(count);
            return board;
        }

        public Board<T> Down(int count = 1)
        {
            board.MoveDown(count);
            return board;
        }

        public Board<T> Left(int count = 1)
        {
            board.MoveLeft(count);
            return board;
        }

        public Board<T> Right(int count = 1)
        {
            board.MoveRight(count);
            return board;
        }

        public Board<T> UpLeft(int count = 1)
        {
            board.MoveUpLeft(count);
            return board;
        }

        public Board<T> UpRight(int count = 1)
        {
            board.MoveUpRight(count);
            return board;
        }

        public Board<T> DownRight(int count = 1)
        {
            board.MoveDownRight(count);
            return board;
        }

        public Board<T> DownLeft(int count = 1)
        {
            board.MoveDownLeft(count);
            return board;
        }

        public Board<T> UpWhile(Func<T, bool> predicate)
        {
            board.MoveUpWhile(predicate);
            return board;
        }

        public Board<T> DownWhile(Func<T, bool> predicate)
        {
            board.MoveDownWhile(predicate);
            return board;
        }

        public Board<T> LeftWhile(Func<T, bool> predicate)
        {
            board.MoveLeftWhile(predicate);
            return board;
        }

        public Board<T> RightWhile(Func<T, bool> predicate)
        {
            board.MoveRightWhile(predicate);
            return board;
        }

        public Board<T> UpLeftWhile(Func<T, bool> predicate)
        {
            board.MoveUpLeftWhile(predicate);
            return board;
        }

        public Board<T> UpRightWhile(Func<T, bool> predicate)
        {
            board.MoveUpRightWhile(predicate);
            return board;
        }

        public Board<T> DownRightWhile(Func<T, bool> predicate)
        {
            board.MoveDownRightWhile(predicate);
            return board;
        }

        public Board<T> DownLeftWhile(Func<T, bool> predicate)
        {
            board.MoveDownLeftWhile(predicate);
            return board;
        }
    }

    public static List<(int x, int y, int z, int w)> GetNeighbours(int xS, int yS, int zS, int wS)
    {
        var list = new List<(int x, int y, int z, int w)>();
        for (var x = xS - 1; x <= xS + 1; x++)
        for (var y = yS - 1; y <= yS + 1; y++)
        for (var z = zS - 1; z <= zS + 1; z++)
        for (var w = wS - 1; w <= wS + 1; w++)
            if (x != xS || y != yS || z != zS || w != wS)
                list.Add((x, y, z, w));
        return list;
    }

    public static List<(int x, int y, int z)> GetNeighbours(int xS, int yS, int zS)
    {
        var list = new List<(int x, int y, int z)>();
        for (var x = xS - 1; x <= xS + 1; x++)
        for (var y = yS - 1; y <= yS + 1; y++)
        for (var z = zS - 1; z <= zS + 1; z++)
            if (x != xS || y != yS || z != zS)
                list.Add((x, y, z));
        return list;
    }

    public static List<Point> GetNeighbours(int xS, int yS)
    {
        var list = new List<Point>();
        for (var x = xS - 1; x <= xS + 1; x++)
        for (var y = yS - 1; y <= yS + 1; y++)
            if (x != xS || y != yS)
                list.Add(new Point(x, y));
        return list;
    }
}

public class Board<T> where T : struct
{
    #region Fields

    private readonly T[,] _board;

    #endregion

    private int _beh = -1;
    private int _leh = -1;

    private int _reh = -1;
    private int _teh = -1;

    public bool PositionIsValid => XInRange(X) && YInRange(Y);
    public Point BottomRight => new(Width - 1, Height - 1);
    public Point TopRight => new(Width - 1, 0);
    public Point BottomLeft => new(0, Height - 1);
    public Point TopLeft => new(0, 0);


    public Board<T> RotateAcw()
    {
        var b = new Board<T>(Height, Width);
        var x2 = 0;

        for (var y = 0; y < Height; y++)
        {
            var y2 = Height - 1;

            for (var x = 0; x < Width; x++)
            {
                b.SetValueAt(x2, y2, _board[x, y]);
                y2--;
            }

            x2++;
        }

        return b;
    }

    public Board<T> FlipX()
    {
        var b = new Board<T>(Width, Height);

        for (var y = 0; y < Height; y++)
        {
            var x2 = 0;
            for (var x = Width - 1; x >= 0; x--)
            {
                b.SetValueAt(x2, y, _board[x, y]);
                x2++;
            }
        }

        return b;
    }

    public Board<T> FlipY()
    {
        var b = new Board<T>(Width, Height);
        for (var x = 0; x < Width; x++)
        {
            var y2 = 0;
            for (var y = Height - 1; y >= 0; y--)
            {
                b.SetValueAt(x, y2, _board[x, y]);
                y2++;
            }
        }

        return b;
    }

    public int RightEdgeHash()
    {
        if (_reh == -1)
        {
            var hash = 0;
            for (var y = 0; y < Height; y++)
                hash += HashCode.Combine(hash, _board[Width - 1, y]);
            _reh = hash;
        }

        return _reh;
    }

    public int LeftEdgeHash()
    {
        if (_leh == -1)
        {
            var hash = 0;
            for (var y = 0; y < Height; y++)
                hash += HashCode.Combine(hash, _board[0, y]);
            _leh = hash;
        }

        return _leh;
    }

    public int TopEdgeHash()
    {
        if (_teh == -1)
        {
            var hash = 0;
            for (var x = 0; x < Width; x++)
                hash += HashCode.Combine(hash, _board[x, 0]);
            _teh = hash;
        }

        return _teh;
    }

    public int BottomEdgeHash()
    {
        if (_beh == -1)
        {
            var hash = 0;
            for (var x = 0; x < Width; x++)
                hash += HashCode.Combine(hash, _board[x, Height - 1]);
            _beh = hash;
        }

        return _beh;
    }

    public int Count(Func<T, bool> func)
    {
        return Values.Count(func);
    }

    public string WriteToString(string rowConcatenator = "\r\n", string columnConcatenator = "")
    {
        return string.Join(rowConcatenator, GetRows().Select(r => string.Join(columnConcatenator, r)));
    }

    public (Board<T> board1, Board<T> board2) SplitAtXLine(int x)
    {
        var board1 = new Board<T>(x, Height);
        var board2 = new Board<T>(Width - x - 1, Height);
        foreach (var (pos, value) in ValuesAndPositions)
            if (board1.PositionInRange(pos)) board1.SetValueAt(pos, value);
            else board2.SetValueAt(pos.X - x - 1, pos.Y, value);

        return (board1, board2);
    }

    public (Board<T> board1, Board<T> board2) SplitAtYLine(int y)
    {
        var board1 = new Board<T>(Width, y);
        var board2 = new Board<T>(Width, Height - y - 1);
        foreach (var p in ValuesAndPositions)
            if (board1.PositionInRange(p.pos)) board1.SetValueAt(p.pos, p.value);
            else board2.SetValueAt(p.pos.X, p.pos.Y - y - 1, p.value);

        return (board1, board2);
    }

    #region Constructors

    public Board(int width, int height, Func<Point, int, T> initFunc = null)
    {
        _board = new T[width, height];
        if (initFunc == null) return;
        var i = 0;
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
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                yield return new Point(x, y);
        }
    }

    public IEnumerable<Point> Traverse
    {
        get
        {
            foreach (var p in Positions)
            {
                SetPosition(p);
                yield return p;
            }
        }
    }

    public Point Position => new(X, Y);

    public void SetValues(IEnumerable<T> values)
    {
        var l = values.ToList();
        for (var i = 0; i < l.Count; i++) SetValueAt(i % Width, i / Height, l[i]);
    }

    public IEnumerable<T> Values
    {
        get
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                yield return ValueAt(x, y);
        }
    }

    public IEnumerable<(Point pos, T value)> ValuesAndPositions
    {
        get
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                yield return (new Point(x, y), value: ValueAt(x, y));
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
            if (!XInRange(X) || !YInRange(Y)) return;
            _board[X, Y] = value;
        }
    }

    public int X { get; set; }
    public int Y { get; set; }

    public IEnumerable<T> GetRow(int y)
    {
        for (var x = 0; x < _board.GetLength(0); x++) yield return ValueAt(x, y);
    }

    public IEnumerable<IEnumerable<T>> GetRows()
    {
        for (var y = 0; y < _board.GetLength(1); y++) yield return GetRow(y);
    }

    public IEnumerable<T> GetColumn(int x)
    {
        for (var y = 0; y < _board.GetLength(1); y++) yield return ValueAt(x, y);
    }

    public IEnumerable<IEnumerable<T>> GetColumns()
    {
        for (var x = 0; x < _board.GetLength(0); x++) yield return GetColumn(x);
    }

    #endregion

    #region Methods

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetPosition(Point point)
    {
        X = point.X;
        Y = point.Y;
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

    public IEnumerable<Point> PositionsAround()
    {
        return new List<Point>
        {
            PositionDown(), PositionDownLeft(), PositionLeft(), PositionUpLeft(), PositionUp(), PositionUpRight(),
            PositionRight(), PositionDownRight()
        };
    }

    public IEnumerable<T> ValuesAround()
    {
        return new List<T>
        {
            ValueDown(), ValueDownLeft(), ValueLeft(), ValueUpLeft(), ValueUp(), ValueUpRight(), ValueRight(),
            ValueDownRight()
        };
    }

    public T ValueDown(int countY = 1)
    {
        return ValueAt(PositionDown(countY));
    }

    public T ValueDownLeft(int countX = 1, int countY = 1)
    {
        return ValueAt(PositionDownLeft(countX, countY));
    }

    public T ValueLeft(int countX = 1)
    {
        return ValueAt(PositionLeft(countX));
    }

    public T ValueUpLeft(int countX = 1, int countY = 1)
    {
        return ValueAt(PositionUpLeft(countX, countY));
    }

    public T ValueUp(int countY = 1)
    {
        return ValueAt(PositionUp(countY));
    }

    public T ValueUpRight(int countX = 1, int countY = 1)
    {
        return ValueAt(PositionUpRight(countX, countY));
    }

    public T ValueRight(int countX = 1)
    {
        return ValueAt(PositionRight(countX));
    }

    public T ValueDownRight(int countX = 1, int countY = 1)
    {
        return ValueAt(PositionDownRight(countX, countY));
    }


    public Point PositionDown(int countY = 1)
    {
        return new Point(X, Y - countY);
    }

    public Point PositionDownLeft(int countX = 1, int countY = 1)
    {
        return new Point(X - countX, Y - countY);
    }

    public Point PositionLeft(int countX = 1)
    {
        return new Point(X - countX, Y);
    }

    public Point PositionUpLeft(int countX = 1, int countY = 1)
    {
        return new Point(X - countX, Y + countY);
    }

    public Point PositionUp(int countY = 1)
    {
        return new Point(X, Y + countY);
    }

    public Point PositionUpRight(int countX = 1, int countY = 1)
    {
        return new Point(X + countX, Y + countY);
    }

    public Point PositionRight(int countX = 1)
    {
        return new Point(X + countX, Y);
    }

    public Point PositionDownRight(int countX = 1, int countY = 1)
    {
        return new Point(X + countX, Y - countY);
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

    public bool SetValueAtCurrent(T ch)
    {
        return SetValueAt(X, Y, ch);
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

    public bool CanMoveUp(int count = 1)
    {
        return YInRange(Y - count);
    }

    public bool CanMoveDown(int count = 1)
    {
        return YInRange(Y + count);
    }

    public bool CanMoveLeft(int count = 1)
    {
        return XInRange(X - count);
    }

    public bool CanMoveRight(int count = 1)
    {
        return XInRange(X + count);
    }

    public bool CanMoveUpLeft(int xcount = 1, int ycount = 1)
    {
        return YInRange(Y - ycount) && XInRange(X - xcount);
    }

    public bool CanMoveUpRight(int xcount = 1, int ycount = 1)
    {
        return YInRange(Y - ycount) && XInRange(X + xcount);
    }

    public bool CanMoveDownRight(int xcount = 1, int ycount = 1)
    {
        return YInRange(Y + ycount) && XInRange(X + xcount);
    }

    public bool CanMoveDownLeft(int xcount = 1, int ycount = 1)
    {
        return YInRange(Y + ycount) && XInRange(X - xcount);
    }

    public T MoveUpWhile(Func<T, bool> predicate)
    {
        while (true)
            if (CanMoveUp() && predicate.Invoke(Value)) MoveUp();
            else break;
        return Value;
    }

    public T MoveDownWhile(Func<T, bool> predicate)
    {
        while (true)
            if (CanMoveDown() && predicate.Invoke(Value)) MoveDown();
            else break;
        return Value;
    }

    public T MoveLeftWhile(Func<T, bool> predicate)
    {
        while (true)
            if (CanMoveLeft() && predicate.Invoke(Value)) MoveLeft();
            else break;
        return Value;
    }

    public T MoveRightWhile(Func<T, bool> predicate)
    {
        while (true)
            if (CanMoveRight() && predicate.Invoke(Value)) MoveRight();
            else break;
        return Value;
    }

    public T MoveUpLeftWhile(Func<T, bool> predicate)
    {
        while (true)
            if (CanMoveUpLeft() && predicate.Invoke(Value)) MoveUpLeft();
            else break;
        return Value;
    }

    public T MoveUpRightWhile(Func<T, bool> predicate)
    {
        while (true)
            if (CanMoveUpRight() && predicate.Invoke(Value)) MoveUpRight();
            else break;
        return Value;
    }

    public T MoveDownRightWhile(Func<T, bool> predicate)
    {
        while (true)
            if (CanMoveDownRight() && predicate.Invoke(Value)) MoveDownRight();
            else break;
        return Value;
    }

    public T MoveDownLeftWhile(Func<T, bool> predicate)
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
        return Values.Count(v => v.Equals(c));
    }

    #endregion
}