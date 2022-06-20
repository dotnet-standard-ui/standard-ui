using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms
{
    public class UIElementCollection : IUIElementCollection
    {
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
    }
}
