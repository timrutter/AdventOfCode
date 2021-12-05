using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Helpers
{
    /// <summary>
    ///     A generic tree class to be inherited from. Provides functions for recursing the tree
    /// </summary>
    /// <typeparam name="TTree">The type of node</typeparam>
    public abstract class Tree<TTree>  where TTree : Tree<TTree>
    {
        #region Fields

        #endregion

        #region Constructors

        protected Tree()
        {
            Children = new List<TTree>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>
        ///     The parent.
        /// </value>
        public TTree Parent { get; private set; }


        /// <summary>
        ///     Gets the children.
        /// </summary>
        /// <value>
        ///     The children.
        /// </value>
        public List<TTree> Children { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is root.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is root; otherwise, <c>false</c>.
        /// </value>
        public bool IsRoot => Parent == null;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is leaf.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is leaf; otherwise, <c>false</c>.
        /// </value>
        public bool IsLeaf => Children.Count == 0;

        public int Count => Children.Count;

        public TTree this[int index]
        {
            get
            {
                if (index >= 0 && index < Children.Count)
                    return Children[index];
                return null;
            }
        }

        #endregion

        #region Methods

        public void SetChildren(List<TTree> items)
        {
            Children.Clear();
            Children.AddRange(items);
        }


        public void AddChildren(IEnumerable<TTree> children)
        {
            var enumerable = children.ToList();
            foreach (var child in enumerable)
            {
                child.Parent = this as TTree;
            }
            Children.AddRange(enumerable);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public TTree GetRoot()
        {
            return IsRoot ? (TTree) this : Parent.GetRoot();
        }


        public IEnumerable<TTree> Traverse()
        {
            var stack = new Stack<TTree>();
            stack.Push(this as TTree); 

            while (stack.Any())
            {
                var current = stack.Pop();
                yield return current;
                for (int i = current.Children.Count -1; i >=0; i--)
                    stack.Push(current.Children[i]);
            }
        }

        /// <summary>
        ///     Returns the match index for this item of all the items that match the predicate
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The index of the </returns>
        public int GetMatchIndex(Func<TTree, bool> predicate)
        {
            var children =
                GetRoot().FindChildren(predicate);
            return children.IndexOf((TTree) this);
        }

        public TTree FindParent(Func<TTree, bool> func)
        {
            if (Parent == null) return null;
            return func.Invoke(Parent) ? Parent : Parent.FindParent(func);
        }

        public TType FindParent<TType>() where TType : class
        {
            if (Parent == null) return null;
            return Parent is TType ttype ? ttype : Parent.FindParent<TType>();
        }

        public void AddChild(TTree child, int index = -1)
        {
            child.Parent = this as TTree;
            Children.Add(child);
        }
        

        public void RemoveChild(TTree child)
        {
            if (child == null) return;
            Children.Remove(child);
        }

        public void RemoveChildren(IEnumerable<TTree> children)
        { 
            if (children == null) return;
            foreach (var child in children.ToList())
                Children.Remove(child);
        }

        /// <summary>
        ///     Recursively get the parents.
        /// </summary>
        /// <returns>A list of parents starting with top level parent</returns>
        public List<TTree> GetParents()
        {
            if (IsRoot)
                return new List<TTree>();

            var ret = Parent?.GetParents();
            if (ret == null)
                //top level, return a new list with this parent
                return new List<TTree> {Parent};

            ret.Insert(0, Parent);
            return ret;
        }

        /// <summary>
        ///     Performs the action recursively
        /// </summary>
        /// <param name="action">The action.</param>
        public void RecursiveAction(Action<TTree> action)
        {
            if (this is TTree t)
                action.Invoke(t);
            foreach (var c in Children)
                c?.RecursiveAction(action);
        }

        /// <summary>
        ///     Performs the action recursively
        /// </summary>
        /// <param name="func">The action.</param>
        public IEnumerable<TGraph> RecursiveFunc<TGraph>(Func<TTree, TGraph> func)
        {
            if (this is TTree t)
                yield return func.Invoke(t);
            foreach (var c in Children)
            foreach (var r in c.RecursiveFunc(func))
                yield return r;
        }

        public void BubbleUpAction(Action<TTree> action)
        {
            if (this is TTree t)
                action.Invoke(t);
            if (Parent is TTree p)
                p.BubbleUpAction(action);
        }

        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public TFind FindChild<TFind>(Func<TFind, bool> function, bool recursive = true)
        {
            if (this is TFind t && function(t)) return t;
            return recursive
                ? Children.Select(c => c.FindChild(function)).FirstOrDefault(ret => ret != null)
                : Children.OfType<TFind>().FirstOrDefault(function);
        }

        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public TTree FindChild(Func<TTree, bool> function, bool recursive = true)
        {
            if (this is TTree t && function(t)) return t;
            return recursive
                ? Children.Select(c => c.FindChild(function)).FirstOrDefault(ret => ret != null)
                : Children.FirstOrDefault(function);
        }

        /// <summary>
        ///     Finds a nth child found based on the predicate.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="index">The nth index</param>
        /// <returns>
        ///     The found child or null
        /// </returns>
        public TTree FindChild(Func<TTree, bool> function, int index)
        {
            var children =
                FindChildren(function.Invoke);
            return index >= children.Count || index < 0 ? null : children[index];
        }

        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public List<TTree> FindChildren(Func<TTree, bool> function)
        {
            var ret = new List<TTree>();
            if (this is TTree t && function(t)) ret.Add(this as TTree);
            foreach (var c in Children)
                ret.AddRange(c.FindChildren(function));
            return ret;
        }

        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public List<TOther> FindChildren<TOther>(Func<TOther, bool> function) where TOther : class
        {
            var ret = new List<TOther>();
            if (this is TOther t && function.Invoke(t)) ret.Add(t);
            foreach (var c in Children)
                ret.AddRange(c.FindChildren(function));
            return ret;
        }
        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public List<TOther> FindChildren<TOther>() where TOther : class
        {
            var ret = new List<TOther>();
            if (this is TOther t) ret.Add(t);
            foreach (var c in Children)
                ret.AddRange(c.FindChildren<TOther>());
            return ret;
        }

        #endregion
    }

    /// <summary>
    ///     A generic graph class to be inherited from. Provides functions for recursing the tree
    /// </summary>
    /// <typeparam name="THeirarchy">The type of node</typeparam>
    public abstract class Graph<THeirarchy> where THeirarchy : Graph<THeirarchy>
    {
        private List<THeirarchy> _children;

        #region Fields

        #endregion

        #region Constructors

        protected Graph()
        {
            _children = new List<THeirarchy>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>
        ///     The parent.
        /// </value>
        public List<THeirarchy> Parents { get;  } = new List<THeirarchy>();


        /// <summary>
        ///     Gets the children.
        /// </summary>
        /// <value>
        ///     The children.
        /// </value>
        public IReadOnlyList<THeirarchy> Children => _children;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is root.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is root; otherwise, <c>false</c>.
        /// </value>
        public bool IsRoot => Parents.Count ==0;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is leaf.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is leaf; otherwise, <c>false</c>.
        /// </value>
        public bool IsLeaf => !Children.Any();

        public int Count => Children.Count();

        public THeirarchy this[int index]
        {
            get
            {
                if (index >= 0 && index < Children.Count())
                    return Children[index];
                return null;
            }
        }

        #endregion

        #region Methods

        public void SetChildren(List<THeirarchy> items)
        {
            _children.Clear();
            _children.AddRange(items);
        }


        public void AddChildren(IEnumerable<THeirarchy> children)
        {
            var enumerable = children.ToList();
            foreach (var child in enumerable)
            {
                child.Parents.Add(this as THeirarchy);
            }
            _children.AddRange(enumerable);
        }

        public void Clear()
        {
            _children.Clear();
        }

       
        public IEnumerable<THeirarchy> Traverse()
        {
            var stack = new Stack<THeirarchy>();
            stack.Push(this as THeirarchy); 

            while (stack.Any())
            {
                var current = stack.Pop();
                yield return current;
                for (int i = current.Children.Count -1; i >=0; i--)
                    stack.Push(current.Children[i]);
            }
        }

        

        public THeirarchy FindParent(Func<THeirarchy, bool> func)
        {
            if (Parents.Count == 0) return null;
            foreach (var p in Parents)
            {
                if (func.Invoke(p)) return p;
                var pe= p.FindParent(func);
                if (pe != null) return pe;
            }

            return null;
        }

        public void AddChild(THeirarchy child, int index = -1)
        {
            child.Parents.Add(this as THeirarchy);
            if (index ==-1)
                _children.Add(child);
            else _children.Insert(index,child);
        }

        
        public void RemoveChild(THeirarchy child)
        {
            if (child == null) return;
            _children.Remove(child);
        }

        public void RemoveChildren(IEnumerable<THeirarchy> children)
        { 
            if (children == null) return;
            foreach (var child in children.ToList())
                _children.Remove(child);

        }

        public void ClearAndDisposeChildren()
        {
            if (Children.Count == 0) return;
            foreach (var child in Children)
            {
                child.ClearAndDisposeChildren();
                if (child is IDisposable disposable)
                    disposable.Dispose();
            }
            
            _children.Clear();
        }
        /// <summary>
        ///     Recursively get the parents.
        /// </summary>
        /// <returns>A list of parents starting with top level parent</returns>
        public List<THeirarchy> GetParents()
        {
            var ret = new List<THeirarchy>();
             if (IsRoot)
                return ret;
            
            foreach (var p in Parents)
            {
                ret.Add(p);    
                ret.AddRange(p.GetParents());
            }
            return ret;
        }

        /// <summary>
        ///     Performs the action recursively
        /// </summary>
        /// <param name="action">The action.</param>
        public void RecursiveAction(Action<THeirarchy> action)
        {
            if (this is THeirarchy t)
                action.Invoke(t);
            foreach (var c in Children)
                c?.RecursiveAction(action);
        }

        /// <summary>
        ///     Performs the action recursively
        /// </summary>
        /// <param name="func">The action.</param>
        public IEnumerable<TGraph> RecursiveFunc<TGraph>(Func<TGraph, TGraph> func)
        {
            if (this is TGraph t)
                yield return func.Invoke(t);
            foreach (var c in Children)
            foreach (var r in c.RecursiveFunc(func))
                yield return r;
        }
        
        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public TFind FindChild<TFind>(Func<TFind, bool> function, bool recursive = true)
        {
            if (this is TFind t && function(t)) return t;
            return recursive
                ? Children.Select(c => c.FindChild(function)).FirstOrDefault(ret => ret != null)
                : Children.OfType<TFind>().FirstOrDefault(function);
        }

        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public THeirarchy FindChild(Func<THeirarchy, bool> function, bool recursive = true)
        {
            if (this is THeirarchy t && function(t)) return t;
            return recursive
                ? Children.Select(c => c.FindChild(function)).FirstOrDefault(ret => ret != null)
                : Children.FirstOrDefault(function);
        }

        /// <summary>
        ///     Finds a nth child found based on the predicate.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="index">The nth index</param>
        /// <returns>
        ///     The found child or null
        /// </returns>
        public THeirarchy FindChild(Func<THeirarchy, bool> function, int index)
        {
            var children =
                FindChildren(function.Invoke);
            return index >= children.Count || index < 0 ? null : children[index];
        }

        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public List<THeirarchy> FindChildren(Func<THeirarchy, bool> function)
        {
            var ret = new List<THeirarchy>();
            if (this is THeirarchy t && function(t)) ret.Add(this as THeirarchy);
            foreach (var c in Children)
                ret.AddRange(c.FindChildren(function));
            return ret;
        }

        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public List<TOther> FindChildren<TOther>(Func<TOther, bool> function) where TOther : class
        {
            var ret = new List<TOther>();
            if (this is TOther t && function.Invoke(t)) ret.Add(t);
            foreach (var c in Children)
                ret.AddRange(c.FindChildren(function));
            return ret;
        }
        /// <summary>
        ///     Finds a child based on the predicate.
        /// </summary>
        /// <returns>The found child or null</returns>
        public List<TOther> FindChildren<TOther>() where TOther : class
        {
            var ret = new List<TOther>();
            if (this is TOther t) ret.Add(t);
            foreach (var c in Children)
                ret.AddRange(c.FindChildren<TOther>());
            return ret;
        }

        #endregion
    }
}