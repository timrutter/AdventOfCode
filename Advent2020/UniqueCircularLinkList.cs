using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Advent2020
{
    public class UniqueCircularLinkList<T>
    {
        private Dictionary<T, CircularLinkListNode<T>> dict;

        public UniqueCircularLinkList(List<T> uniqueValues)
        {
            if (!uniqueValues.Any()) return;
            dict = new Dictionary<T, CircularLinkListNode<T>>();
            Current = new CircularLinkListNode<T>(uniqueValues.First(), dict);
            dict.Add( uniqueValues.First(), Current);

            var c = Current;
            for (var i = 1; i < uniqueValues.Count; i++)
            {
                var ne = new CircularLinkListNode<T>(uniqueValues[i], dict);
                dict.Add(uniqueValues[i],ne );
                c.SetNext(uniqueValues[i] )  ;
                ne.SetPrevious(c.Value);
                c = c.Next;
            }
            c.SetNext(Current.Value);
            Current.SetPrevious(c.Value);
        }

        public CircularLinkListNode<T> Current { get; private set; }
        public CircularLinkListNode<T> Find(T value) => dict[value];

        public void MoveNext()
        {
            Current = Current.Next;
        }
        public void MovePrevious()
        {
            Current = Current.Previous;
        }
    }
}