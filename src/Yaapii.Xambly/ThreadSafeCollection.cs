// MIT License
//
// Copyright(c) 2022 ICARUS Consulting GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Reflection;
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
        private readonly List<T> items;

        /// <summary>
        /// Lock object
        /// </summary>
        private readonly object sync;

        /// <summary>
        /// ctor
        /// </summary>
        public ThreadsafeCollection()
        {
            this.items = new List<T>();
            this.sync = new Object();
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="syncRoot"></param>
        public ThreadsafeCollection(object syncRoot)
        {
            if (syncRoot == null)
                throw new ArgumentNullException("syncRoot");

            this.items = new List<T>();
            this.sync = syncRoot;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="syncRoot">root object to sync</param>
        /// <param name="list">list to instantiate from</param>
        public ThreadsafeCollection(object syncRoot, IEnumerable<T> list)
        {
            if (syncRoot == null)
                throw new ArgumentNullException("syncRoot");
            if (list == null)
                throw new ArgumentNullException("list");

            this.items = new List<T>(list);
            this.sync = syncRoot;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="syncRoot">root object to sync</param>
        /// <param name="list">list to instantiate from</param>
        public ThreadsafeCollection(object syncRoot, params T[] list)
        {
            if (syncRoot == null)
                throw new ArgumentNullException("syncRoot");
            if (list == null)
                throw new ArgumentNullException("list");

            this.items = new List<T>(list.Length);
            for (int i = 0; i < list.Length; i++)
                this.items.Add(list[i]);

            this.sync = syncRoot;
        }

        /// <summary>
        /// Count items
        /// </summary>
        public int Count
        {
            get { lock (this.sync) { return this.items.Count; } }
        }

        /// <summary>
        /// The items
        /// </summary>
        protected List<T> Items
        {
            get { return this.items; }
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
                    return this.items[index];
                }
            }
            set
            {
                lock (this.sync)
                {
                    if (index < 0 || index >= this.items.Count)
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
                int index = this.items.Count;
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
                this.items.CopyTo(array, index);
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
                return this.items.Contains(item);
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
                return this.items.GetEnumerator();
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
                if (index < 0 || index > this.items.Count)
                    throw new ArgumentOutOfRangeException(
                        "index", index,
                        new Formatted(
                            "value {0} must be in range of {1}", index, this.Items.Count).AsString());

                this.InsertItem(index, item);
            }
        }

        /// <summary>
        /// Internal IndexOf
        /// </summary>
        /// <param name="item">item to search</param>
        /// <returns>the index</returns>
        private int InternalIndexOf(T item)
        {
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                if (object.Equals(items[i], item))
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
                if (index < 0 || index >= this.items.Count)
                    throw new ArgumentOutOfRangeException("index", index, $"value {index} must be in range of {this.Items.Count}");


                this.RemoveItem(index);
            }
        }

        /// <summary>
        /// Clears all items
        /// </summary>
        protected virtual void ClearItems()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Insert an item at given index
        /// </summary>
        /// <param name="index">index to insert at</param>
        /// <param name="item">item to insert</param>
        protected virtual void InsertItem(int index, T item)
        {
            this.items.Insert(index, item);
        }

        /// <summary>
        /// Removes an item at given index.
        /// </summary>
        /// <param name="index">index to remove at</param>
        protected virtual void RemoveItem(int index)
        {
            this.items.RemoveAt(index);
        }

        /// <summary>
        /// Replaces an item at the given index.
        /// </summary>
        /// <param name="index">index to replace at</param>
        /// <param name="item">replacement item</param>
        protected virtual void SetItem(int index, T item)
        {
            this.items[index] = item;
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
            return ((IList)this.items).GetEnumerator();
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
                ((IList)this.items).CopyTo(array, index);
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
        private static void VerifyValueType(object value)
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
                        new Formatted(
                            "object is of type {0} but collection is of {1}",
                            value.GetType().FullName,
                            typeof(T).FullName).AsString());
            }
        }
    }
}
