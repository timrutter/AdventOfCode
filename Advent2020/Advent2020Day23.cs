using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Advent2020
{
    public class Advent2020Day23 : Solution
    {
        public Advent2020Day23()
        {
            Answer1 = "38925764";
            Answer2 = 131152940564;
        }
        public override object ExecutePart1()
        {
            var cups = "562893147".Select(c => int.Parse(c.ToString())).ToList();

            var current = 0;

            int RemoveNextCupIndex(int currentValue)
            {
                var index = (cups.IndexOf(currentValue) + 1) % cups.Count;
                var ret = cups[index];
                cups.RemoveAt(index);
                return ret;
            }
            int GetDestinationCup(int index)
            {
                var i = cups[index];
                while (true)
                {
                    i--;
                    if (i < cups.Min()) i = cups.Max();
                    var dIndex = cups.IndexOf(i);
                    if (dIndex != -1) return dIndex;
                }

            }
            for (int i = 0; i < 100; i++)
            {

                var currentValue = cups[current];
                var pickedUp = new List<int>
                {
                    RemoveNextCupIndex(currentValue),
                    RemoveNextCupIndex(currentValue),
                    RemoveNextCupIndex(currentValue)
                };
                var d = (GetDestinationCup(cups.IndexOf(currentValue)));
                if (d == cups.Count - 1)
                {
                    cups.Add(pickedUp[0]);
                    cups.Add(pickedUp[1]);
                    cups.Add(pickedUp[2]);
                }
                else
                {
                    cups.Insert((d + 1) % (cups.Count), pickedUp[2]);
                    cups.Insert((d + 1) % (cups.Count), pickedUp[1]);
                    cups.Insert((d + 1) % (cups.Count), pickedUp[0]);
                }
                current = (cups.IndexOf(currentValue) + 1) % cups.Count;
                // for (int j = 0; j < cups.Count; j++)
                // {
                //     if (j == current)
                //         Console.Write($"({cups[j]}) ");
                //     else 
                //         Console.Write($"{cups[j]} ");
                // }
                // Console.WriteLine();
            }

            var s = new StringBuilder();
            for (int i = 1; i < cups.Count; i++)
            {
                var index = (cups.IndexOf(1) + i) % cups.Count;
                s.Append(cups[index]);
            }
            return s.ToString();
        }

        public override object ExecutePart2()
        {
            var cups = "562893147".Select(c => int.Parse(c.ToString())).ToList();

            const int max = 1000000;
            for (var i = cups.Count + 1; i <= max; i++)
            {
                cups.Add(i);
            }
            var circularLinkedList = new UniqueCircularLinkList<int>(cups);

            CircularLinkListNode<int> RemoveCups()
            {
                var ret = circularLinkedList.Current.Next;
                circularLinkedList.Current.SetNext(circularLinkedList.Current.Next.Next.Next.Next.Value);
                circularLinkedList.Current.Next.SetPrevious(circularLinkedList.Current.Value);

                return ret;
            }

            CircularLinkListNode<int> GetDestinationCup(CircularLinkListNode<int> pickedUp)
            {
                var value = circularLinkedList.Current.Value;
                while (true)
                {
                    value--;
                    if (value < 1) value = max;
                    if (pickedUp.Value == value ||
                        pickedUp.Next.Value == value ||
                        pickedUp.Next.Next.Value == value) continue;
                    return circularLinkedList.Find(value);
                }
            }

            for (var i = 0; i < 10000000; i++)
            {
                var pickedUp = RemoveCups();
                var d = GetDestinationCup(pickedUp);
                var temp = d.Next;
                d.SetNext(pickedUp.Value);
                pickedUp.Next.Next.SetNext(temp.Value);
                circularLinkedList.MoveNext();
            }


            return circularLinkedList.Find(1).Next.Value * (long)circularLinkedList.Find(1).Next.Next.Value;
        }
    }
}