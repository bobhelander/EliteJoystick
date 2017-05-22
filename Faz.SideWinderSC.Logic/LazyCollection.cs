using System;
using System.Collections;
using System.Collections.Generic;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides a collection that supports lazy loading.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class LazyCollection<T> : ICollection<T>
    {
        /// <summary>
        /// The underline collection.
        /// </summary>
        private readonly ICollection<T> encapsulated;

        /// <summary>
        /// The initializer of the collection.
        /// </summary>
        private readonly Func<IEnumerable<T>> initialize;

        /// <summary>
        /// A value indicating whether the collection was initialized.
        /// </summary>
        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyCollection{T}"/> class
        /// without initializer.
        /// </summary>
        public LazyCollection()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyCollection{T}"/> class
        /// with a specific initializer.
        /// </summary>
        /// <param name="initialize">
        /// The initializer of the collection.
        /// </param>
        public LazyCollection(Func<IEnumerable<T>> initialize)
            : this(new LinkedList<T>(), initialize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyCollection{T}"/> class
        /// with a specific initializer and its encapsulated collection implementation.
        /// </summary>
        /// <param name="encapsulated">
        /// The encapsulated collection implementation.
        /// </param>
        /// <param name="initialize">
        /// The initializer of the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="encapsulated"/> parameter is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="encapsulated"/> collection is not empty.
        /// </exception>
        protected LazyCollection(ICollection<T> encapsulated, Func<IEnumerable<T>> initialize)
        {
            if (encapsulated == null)
            {
                throw new ArgumentNullException("encapsulated");
            }
            else if (encapsulated.Count != 0)
            {
                throw new ArgumentException("The encapsulated collection should be empty", "encapsulated");
            }

            this.encapsulated = encapsulated;
            this.initialize = initialize;
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                this.Initialize();
                return this.Encapsulated.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                this.Initialize();
                return ((ICollection<T>)this.Encapsulated).IsReadOnly;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is already initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return this.initialized; }
        }

        /// <summary>
        /// Gets the encapsulated collection.
        /// </summary>
        /// <value>
        /// The elements associated with the collection when already initialized;
        /// otherwise an empty collection. This property will never returns <c>null</c>.
        /// </value>
        protected ICollection<T> Encapsulated
        {
            get { return this.encapsulated; }
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The object to add to the collection.</param>
        public void Add(T item)
        {
            this.Initialize();
            this.Encapsulated.Add(item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            // No call to initialize as the values will be removed just after
            this.Encapsulated.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>
        /// <c>true</c> if item is found in the collection; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            this.Initialize();
            return this.Encapsulated.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements copied from collection.
        /// The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Initialize();
            this.Encapsulated.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="item"/> was successfully removed from the collection;
        /// otherwise, <c>false</c>.
        /// This method also returns <c>false</c> if item is not found in the original collection.
        /// </returns>
        public bool Remove(T item)
        {
            this.Initialize();
            return this.Encapsulated.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            this.Initialize();
            return this.Encapsulated.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            this.Initialize();
            return this.Encapsulated.GetEnumerator();
        }

        /// <summary>
        /// Initializes the collection - if required.
        /// </summary>
        protected void Initialize()
        {
            if (this.initialized)
            {
                return;
            }

            if (this.initialize != null)
            {
                foreach (var value in this.initialize())
                {
                    this.Encapsulated.Add(value);
                }
            }

            this.initialized = true;
        }
    }
}
