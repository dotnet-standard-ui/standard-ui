using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace Microsoft.StandardUI.Maui
{
    public class UIElementCollection<TNativeUIElment, TStandardUIElement> : IList<TNativeUIElment>
        where TNativeUIElment : View where TStandardUIElement : IUIElement
    {
        public UIElementCollection(Element parent)
        {
        }

        public IUICollection<TStandardUIElement> ToStandardUIElementCollection() => throw new NotImplementedException();

#if TODO
        private ObservableCollection<Xamarin.Forms.Element> _collection;

        public UIElementCollection(Xamarin.Forms.Element parent)
        {
            _collection = new ObservableCollection<Xamarin.Forms.Element>();
        }

        public IUIElement this[int index]
        {
            get => (IUIElement)_collection[index];
            set => _collection[index] = (Xamarin.Forms.Element)value;
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => false;

        public void Add(IUIElement item)
        {
            _collection.Add((Xamarin.Forms.Element) item);
        }

        public void Clear() => _collection.Clear();

        public bool Contains(IUIElement item) => _collection.Contains((Xamarin.Forms.Element)item);

        public void CopyTo(IUIElement[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IUIElement> GetEnumerator() => new Enumerator(_collection.GetEnumerator());

        public int IndexOf(IUIElement item) => _collection.IndexOf((Xamarin.Forms.Element)item);

        public void Insert(int index, IUIElement item) => _collection.Insert(index, (Xamarin.Forms.Element)item);

        public bool Remove(IUIElement item)
        {
            int index = _collection.IndexOf((Xamarin.Forms.Element)item);
            if (index == -1)
                return false;
            _collection.RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index) => _collection.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

        private class Enumerator : IEnumerator<IUIElement>
        {
            private IEnumerator _enumerator;

            public Enumerator(IEnumerator enumerator)
            {
                _enumerator = enumerator;
            }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
            public IUIElement? Current => (IUIElement?)_enumerator.Current;
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

            object? IEnumerator.Current => _enumerator.Current;

            public void Dispose() { }

            public bool MoveNext() => _enumerator.MoveNext();

            public void Reset() => _enumerator.Reset();
        }
#endif
        TNativeUIElment IList<TNativeUIElment>.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        int ICollection<TNativeUIElment>.Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ICollection<TNativeUIElment>.IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void ICollection<TNativeUIElment>.Add(TNativeUIElment item)
        {
            throw new NotImplementedException();
        }

        void ICollection<TNativeUIElment>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<TNativeUIElment>.Contains(TNativeUIElment item)
        {
            throw new NotImplementedException();
        }

        void ICollection<TNativeUIElment>.CopyTo(TNativeUIElment[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator<TNativeUIElment> IEnumerable<TNativeUIElment>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        int IList<TNativeUIElment>.IndexOf(TNativeUIElment item)
        {
            throw new NotImplementedException();
        }

        void IList<TNativeUIElment>.Insert(int index, TNativeUIElment item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<TNativeUIElment>.Remove(TNativeUIElment item)
        {
            throw new NotImplementedException();
        }

        void IList<TNativeUIElment>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
    }
}
