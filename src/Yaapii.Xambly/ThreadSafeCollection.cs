using System.Reflection;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// A collection which is threadsafe.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.ComVisible(false)]
    public class ThreadsafeCollection<T> : IList<T>, IList
    {
        /// <summary>
        /// List to protect
        /// </summary>
        private readonly IScalar<List<T>> items;
        /// <summary>
        /// Lock object
        /// </summary>
        private readonly IScalar<object> sync;

        /// <summary>
        /// A collection which is threadsafe.
        /// </summary>
        public ThreadsafeCollection() : this(
            new object(),
            new EnumerableOf<T>()
        )
        { }

        /// <summary>
        /// A collection which is threadsafe.
        /// </summary>
        /// <param name="sync">object to sync</param>
        public ThreadsafeCollection(object sync) : this(
            sync,
            new EnumerableOf<T>()
        )
        { }

        /// <summary>
        /// A collection which is threadsafe.
        /// </summary>
        /// <param name="items">list to instantiate from</param>
        public ThreadsafeCollection(IEnumerable<T> items) : this(
            new object(),
            items
        )
        { }

        /// <summary>
        /// A collection which is threadsafe.
        /// </summary>
        /// <param name="sync">object to sync</param>
        /// <param name="items">list to instantiate from</param>
        public ThreadsafeCollection(object sync, params T[] items) : this(
            sync,
            new EnumerableOf<T>(items)
        )
        { }

        /// <summary>
        /// A collection which is threadsafe.
        /// </summary>
        /// <param name="sync">object to sync</param>
        /// <param name="items">list to instantiate from</param>
        public ThreadsafeCollection(object sync, IEnumerable<T> items) : this(
            new StickyScalar<object>(() =>
            {
                new FailNull(
                    sync,
                    new ArgumentNullException("Syncronizing object is null")
                ).Go();
                return sync;
            }),
            new StickyScalar<List<T>>(() =>
            {
                new FailNull(
                    items,
                    new ArgumentNullException("List is null")
                ).Go();
                return new List<T>(items);
            })
            )
        { }

        /// <summary>
        /// A collection which is threadsafe.
        /// </summary>
        /// <param name="sync">object to sync</param>
        /// <param name="items">list to instantiate from</param>
        private ThreadsafeCollection(IScalar<object> sync, IScalar<List<T>> items)
        {
            this.items = items;
            this.sync = sync;
        }

        /// <summary>
        /// Count items
        /// </summary>
        public int Count
        {
            get { lock (this.sync) { return this.items.Value().Count; } }
        }

        /// <summary>
        /// The items
        /// </summary>
        protected List<T> Items
        {
            get { return this.items.Value(); }
        }

        /// <summary>
        /// Sync object
        /// </summary>
        public object SyncRoot
        {
            get { return this.sync; }
        }

        /// <summary>
        /// Access items by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                lock (this.sync)
                {
                    return this.items.Value()[index];
                }
            }
            set
            {
                lock (this.sync)
                {
                    if (index < 0 || index >= this.items.Value().Count)
                        throw new ArgumentOutOfRangeException("index", index, $"value {index} must be in range of {this.Items.Count}");

                    this.SetItem(index, value);
                }
            }
        }

        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (this.sync)
            {
                int index = this.items.Value().Count;
                this.InsertItem(index, item);
            }
        }

        /// <summary>
        /// Clear the list
        /// </summary>
        public void Clear()
        {
            lock (this.sync)
            {
                this.ClearItems();
            }
        }

        /// <summary>
        /// Copy this to an array, starting with given index
        /// </summary>
        /// <param name="array">target array</param>
        /// <param name="index">index to start with</param>
        public void CopyTo(T[] array, int index)
        {
            lock (this.sync)
            {
                this.items.Value().CopyTo(array, index);
            }
        }

        /// <summary>
        /// Determines whether this list contains the given item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            lock (this.sync)
            {
                return this.items.Value().Contains(item);
            }
        }

        /// <summary>
        /// An enumerator to iterate the list
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            lock (this.sync)
            {
                return this.items.Value().GetEnumerator();
            }
        }

        /// <summary>
        /// Index of an item
        /// </summary>
        /// <param name="item">item to search</param>
        /// <returns>the index</returns>
        public int IndexOf(T item)
        {
            lock (this.sync)
            {
                return this.InternalIndexOf(item);
            }
        }

        /// <summary>
        /// Insert an item at given position
        /// </summary>
        /// <param name="index">the position</param>
        /// <param name="item">the item to insert</param>
        public void Insert(int index, T item)
        {
            lock (this.sync)
            {
                if (index < 0 || index > this.items.Value().Count)
                    throw new ArgumentOutOfRangeException(
                        "index", index,
                        new FormattedText(
                            "value {0} must be in range of {1}", index, this.Items.Count).AsString());

                this.InsertItem(index, item);
            }
        }

        /// <summary>
        /// Internal IndexOf
        /// </summary>
        /// <param name="item">item to search</param>
        /// <returns>the index</returns>
        int InternalIndexOf(T item)
        {
            int count = items.Value().Count;

            for (int i = 0; i < count; i++)
            {
                if (object.Equals(items.Value()[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        /// <param name="item">item to remove</param>
        /// <returns>true if success, false if item wasnt found</returns>
        public bool Remove(T item)
        {
            lock (this.sync)
            {
                int index = this.InternalIndexOf(item);
                if (index < 0)
                    return false;

                this.RemoveItem(index);
                return true;
            }
        }

        /// <summary>
        /// Remove a given index
        /// </summary>
        /// <param name="index">the index</param>
        public void RemoveAt(int index)
        {
            lock (this.sync)
            {
                if (index < 0 || index >= this.items.Value().Count)
                    throw new ArgumentOutOfRangeException("index", index, $"value {index} must be in range of {this.Items.Count}");


                this.RemoveItem(index);
            }
        }

        /// <summary>
        /// Clears all items
        /// </summary>
        protected virtual void ClearItems()
        {
            this.items.Value().Clear();
        }

        /// <summary>
        /// Insert an item at given index
        /// </summary>
        /// <param name="index">index to insert at</param>
        /// <param name="item">item to insert</param>
        protected virtual void InsertItem(int index, T item)
        {
            this.items.Value().Insert(index, item);
        }

        /// <summary>
        /// Removes an item at given index.
        /// </summary>
        /// <param name="index">index to remove at</param>
        protected virtual void RemoveItem(int index)
        {
            this.items.Value().RemoveAt(index);
        }

        /// <summary>
        /// Replaces an item at the given index.
        /// </summary>
        /// <param name="index">index to replace at</param>
        /// <param name="item">replacement item</param>
        protected virtual void SetItem(int index, T item)
        {
            this.items.Value()[index] = item;
        }

        /// <summary>
        /// This collection is never readonly.
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Get the enumerator to iterate over the items
        /// </summary>
        /// <returns>the enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.Value().GetEnumerator();
        }

        /// <summary>
        /// This collection is always synchronized.
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        /// <summary>
        /// The sync object.
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return this.sync; }
        }

        /// <summary>
        /// Copy to an array starting with given index.
        /// </summary>
        /// <param name="array">array to copy to</param>
        /// <param name="index">index to start at</param>
        void ICollection.CopyTo(Array array, int index)
        {
            lock (this.sync)
            {
                ((IList)this.items.Value()).CopyTo(array, index);
            }
        }

        /// <summary>
        /// Access items by index
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>item</returns>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                VerifyValueType(value);
                this[index] = (T)value;
            }
        }

        /// <summary>
        /// This is always writable
        /// </summary>
        bool IList.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// This is not fixed size
        /// </summary>
        bool IList.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Internal add method
        /// </summary>
        /// <param name="value">value to add</param>
        /// <returns>new size</returns>
        int IList.Add(object value)
        {
            VerifyValueType(value);

            lock (this.sync)
            {
                this.Add((T)value);
                return this.Count - 1;
            }
        }

        /// <summary>
        /// Test if contains object
        /// </summary>
        /// <param name="value">value to search</param>
        /// <returns>true if this list contains the value</returns>
        bool IList.Contains(object value)
        {
            VerifyValueType(value);
            return this.Contains((T)value);
        }

        /// <summary>
        /// Index of given value
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>the index or -1 if not found</returns>
        int IList.IndexOf(object value)
        {
            VerifyValueType(value);
            return this.IndexOf((T)value);
        }

        /// <summary>
        /// Insert ad given index
        /// </summary>
        /// <param name="index">index to insert at</param>
        /// <param name="value">value to insert</param>
        void IList.Insert(int index, object value)
        {
            VerifyValueType(value);
            this.Insert(index, (T)value);
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        /// <param name="value">value to remove</param>
        void IList.Remove(object value)
        {
            VerifyValueType(value);
            this.Remove((T)value);
        }

        /// <summary>
        /// Test the value type
        /// </summary>
        /// <param name="value">the value to verify</param>
        static void VerifyValueType(object value)
        {
            if (value == null)
            {
                if (typeof(T).GetTypeInfo().IsValueType)
                {
                    throw new ArgumentException("value is null and a value type");
                }
            }
            else if (!(value is T))
            {
                throw
                    new ArgumentException(
                        new FormattedText(
                            "object is of type {0} but collection is of {1}",
                            value.GetType().FullName,
                            typeof(T).FullName
                        ).AsString()
                    );
            }
        }
    }
}
