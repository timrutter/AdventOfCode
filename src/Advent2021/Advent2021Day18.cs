using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day18 : Solution
{
    public Advent2021Day18()
    {
        Answer1 = 4202;
        Answer2 = 4779;
    }

    public override object ExecutePart1()
    {
        var nums = DataFile.ReadAll<string>().ToList();

        var num = ReadNumber(nums[0][1..^1], null);
        for (var index = 1; index < nums.Count; index++)
        {
            num = Add(num, ReadNumber(nums[index][1..^1], null));
            num.Reduce();
        }

        return num.Magnitude;
    }

    private SFNumber Add(SFNumber s1, SFNumber s2)
    {
        return ReadNumber($"{s1},{s2}", null);
    }

    private SFNumber ReadNumber(string num, SFNumber parent)
    {
        var stack = new Stack<int>();
        var sfNumber = new SFNumber(parent);
        var openFound = false;
        var val1Done = false;
        for (var i = 0; i < num.Length; i++)
            if (num[i] == '[')
            {
                openFound = true;
                stack.Push(i);
            }
            else if (num[i] == ']')
            {
                var last = stack.Pop();
                if (stack.Count == 0)
                {
                    if (val1Done)
                    {
                        sfNumber.Value2 = ReadNumber(num[(last + 1)..i], sfNumber);
                        return sfNumber;
                    }

                    sfNumber.Value1 = ReadNumber(num[(last + 1)..i], sfNumber);
                    val1Done = true;
                    openFound = false;
                    i++;
                }
            }
            else if (!openFound)
            {
                if (val1Done)
                {
                    sfNumber.Value2 = int.Parse(num[i..]);
                    return sfNumber;
                }

                var com = num.IndexOf(',');
                sfNumber.Value1 = int.Parse(num[..com]);
                val1Done = true;
                i++;
            }

        return sfNumber;
    }

    public override object ExecutePart2()
    {
        var nums = DataFile.ReadAll<string>().Select(s => ReadNumber(s[1..^1], null)).ToList();

        var mag = int.MinValue;
        for (var i = 0; i < nums.Count; i++)
        for (var j = i + 1; j < nums.Count; j++)
        {
            var added1 = Add(nums[i], nums[j]);
            added1.Reduce();
            var mag1 = added1.Magnitude;
            if (mag1 > mag) mag = mag1;
            var added2 = Add(nums[j], nums[i]);
            added2.Reduce();
            var mag2 = added2.Magnitude;
            if (mag2 > mag) mag = mag2;
        }

        return mag;
    }

    private class SFNumber
    {
        public object Value1 = 0;
        public object Value2 = 0;

        public SFNumber(SFNumber parent)
        {
            Parent = parent;
        }

        public SFNumber Parent { get; }

        public int Level => Parent?.Level + 1 ?? 0;

        public int Magnitude
        {
            get
            {
                int mag1;
                if (Value1 is int v1)
                    mag1 = v1 * 3;
                else
                    mag1 = (Value1 as SFNumber).Magnitude * 3;
                int mag2;
                if (Value2 is int v2)
                    mag2 = v2 * 2;
                else
                    mag2 = (Value2 as SFNumber).Magnitude * 2;
                return mag1 + mag2;
            }
        }

        public void Reduce()
        {
            while (true)
            {
                if (Explode()) continue;
                if (Split()) continue;
                break;
            }
        }

        private bool Split()
        {
            if (Value1 is int v1)
            {
                if (v1 > 9)
                {
                    Value1 = new SFNumber(this) { Value1 = v1 / 2, Value2 = v1 / 2 + v1 % 2 };
                    return true;
                }
            }
            else
            {
                if ((Value1 as SFNumber).Split()) return true;
            }

            if (Value2 is int v2)
            {
                if (v2 > 9)
                {
                    Value2 = new SFNumber(this) { Value1 = v2 / 2, Value2 = v2 / 2 + v2 % 2 };
                    return true;
                }
            }
            else
            {
                if ((Value2 as SFNumber).Split()) return true;
            }

            return false;
        }

        public bool Explode()
        {
            if (Level > 3)
            {
                AddLeft((int)Value1);
                AddRight((int)Value2);
                if (Parent.Value1 == this) Parent.Value1 = 0;
                if (Parent.Value2 == this) Parent.Value2 = 0;
                return true;
            }

            if (Value1 is SFNumber v1 && v1.Explode()) return true;
            return Value2 is SFNumber v2 && v2.Explode();
        }

        public void AddLeft(int val)
        {
            if (Parent == null) return;
            if (this == Parent.Value2)
            {
                if (Parent.Value1 is int value1)
                    Parent.Value1 = value1 + val;
                else
                    (Parent.Value1 as SFNumber).AddRightChild(val);
                return;
            }

            Parent.AddLeft(val);
        }

        public void AddRight(int val)
        {
            if (Parent == null) return;
            if (this == Parent.Value1)
            {
                if (Parent.Value2 is int value2)
                    Parent.Value2 = value2 + val;
                else
                    (Parent.Value2 as SFNumber).AddLeftChild(val);
                return;
            }

            Parent.AddRight(val);
        }

        private void AddLeftChild(int val)

        {
            if (Value1 is int value1) Value1 = value1 + val;
            else (Value1 as SFNumber).AddLeftChild(val);
        }

        private void AddRightChild(int val)

        {
            if (Value2 is int value2) Value2 = value2 + val;
            else (Value2 as SFNumber).AddRightChild(val);
        }

        public override string ToString()
        {
            return $"[{Value1},{Value2}]";
        }
    }
}