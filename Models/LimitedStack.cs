using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SNAMP.Models
{
    public class LimitedStack<T>
    {
        public int Count { get { return _list.Count; } }

        private int _limit;
        private LinkedList<T> _list;

        public LimitedStack(int maxSize)
        {
            _limit = maxSize;
            _list = new LinkedList<T>();

        }

        public void Push(T value)
        {
            if (_list.Count == _limit)
                _list.RemoveLast();

            _list.AddFirst(value);
        }

        public T Pop()
        {
            if (_list.Count > 0)
            {
                T value = _list.First.Value;
                _list.RemoveFirst();
                return value;
            }

            throw new InvalidOperationException("The Stack is empty");
        }

        public T Peek()
        {
            if (_list.Count > 0)
                return _list.First.Value;

            throw new InvalidOperationException("The Stack is empty");
        }

        public void Clear() => _list.Clear();

        public bool Contains(T value) => Count > 0 && _list.Contains(value);

        public void Reverse() => _list.Reverse();

        public IEnumerator GetEnumerator() => _list.GetEnumerator();
    }
}
