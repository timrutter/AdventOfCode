using System.Collections.Generic;

namespace AdventOfCode.Advent2020
{
    public class CircularLinkListNode<T>
    {
        private readonly Dictionary<T, CircularLinkListNode<T>> _available;

        #region Constructors
        
        public CircularLinkListNode(T value, Dictionary<T, CircularLinkListNode<T>> available)
        {
            _available = available;
            Value = value;
        }
        
        #endregion
        
        #region Properties
        
        public T Value { get; }
        public CircularLinkListNode<T> Next { get; private set; }
        public CircularLinkListNode<T> Previous { get; private set;}
        
        #endregion


        public void SetNext(T nextValue)
        {
            Next = _available[nextValue];
            _available[nextValue].Previous = this;
        }
        public void SetPrevious(T prevValue)
        {
            Previous = _available[prevValue];
            _available[prevValue].Next = this;
        }
    }
}