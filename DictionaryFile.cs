using System;
using System.Collections;
using System.Collections.Generic;

namespace Penguin.Collections
{
    public class DictionaryFile<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable
    {
        public TValue this[TKey key]
        {
            get => backingDictionary[key];
            set
            {
                _ = Remove(key);

                Add(key, value);
            }
        }

        private readonly IDictionary<TKey, TValue> backingDictionary;

        private readonly ListFile backingFile;

        private readonly SerializationSettings<TKey> KeySerialization;

        private readonly SerializationSettings<TValue> ValueSerialization;

        public int Count => backingDictionary.Count;

        public bool IsReadOnly => backingDictionary.IsReadOnly;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => backingDictionary.Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => backingDictionary.Values;

        public DictionaryFile(string path, SerializationSettings<TKey> keySerialization, SerializationSettings<TValue> valueSerialization, bool autoflush = true)
        {
            KeySerialization = keySerialization;
            ValueSerialization = valueSerialization;

            backingFile = new ListFile(path, autoflush);
            backingDictionary = new Dictionary<TKey, TValue>();

            foreach (string line in backingFile)
            {
                backingDictionary.Add(KeySerialization.Deserialize(line.Split('\t')[0]), ValueSerialization.Deserialize(line.Split('\t')[1]));
            }
        }

        public void Flush()
        {
            backingFile.Flush();
        }

        public void Add(TKey key, TValue value)
        {
            backingDictionary.Add(key, value);
            backingFile.Add(GetRow(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            backingDictionary.Add(item);
            backingFile.Add(GetRow(item));
        }

        public void Clear()
        {
            backingDictionary.Clear();
            backingFile.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return backingDictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return backingDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            backingDictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return backingDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)backingDictionary).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            bool v = backingDictionary.Remove(key);

            string sKey = KeySerialization.Serialize(key);

            foreach (string line in backingFile)
            {
                if (line.StartsWith($"{sKey}\t"))
                {
                    _ = backingFile.Remove(line);
                    break;
                }
            }
            return v;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool v = backingDictionary.Remove(item);
            _ = backingFile.Remove($"{item.Key}\t{item.Value}");
            return v;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return backingDictionary.TryGetValue(key, out value);
        }

        private string GetRow(TKey key, TValue value)
        {
            return $"{KeySerialization.Serialize(key)}\t{ValueSerialization.Serialize(value)}";
        }

        private string GetRow(KeyValuePair<TKey, TValue> item)
        {
            return GetRow(item.Key, item.Value);
        }

        public void Dispose()
        {
            ((IDisposable)backingFile).Dispose();
        }
    }
}