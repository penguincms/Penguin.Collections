using System;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Collections
{
    public class ListFile<T> : ListFile, IList<T>
    {
        T IList<T>.this[int index]
        {
            get => SerializationSettings.Deserialize(base[index]);
            set => base[index] = SerializationSettings.Serialize(value);
        }

        private readonly SerializationSettings<T> SerializationSettings;

        public ListFile(string path, SerializationSettings<T> serializationSettings) : base(path)
        {
            SerializationSettings = serializationSettings;
        }

        public ListFile(string path, SerializationSettings<T> serializationSettings, bool autoFlush) : base(path, autoFlush)
        {
            SerializationSettings = serializationSettings;
        }

        public void Add(T item)
        {
            Add(SerializationSettings.Serialize(item));
        }

        public bool Contains(T item)
        {
            return Contains(SerializationSettings.Serialize(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy((this as IList<T>).ToArray(), 0, array, arrayIndex, Count);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            IEnumerator<string> baseValues = base.GetEnumerator();

            while (baseValues.MoveNext())
            {
                yield return SerializationSettings.Deserialize(baseValues.Current);
            }
        }

        public int IndexOf(T item)
        {
            return IndexOf(SerializationSettings.Serialize(item));
        }

        public void Insert(int index, T item)
        {
            Insert(index, SerializationSettings.Serialize(item));
        }

        public bool Remove(T item)
        {
            return Remove(SerializationSettings.Serialize(item));
        }
    }
}