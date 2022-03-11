using System.Collections;
using System.Collections.Generic;

namespace Penguin.Collections
{
    public class DictionaryFile<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public TValue this[TKey key]
        {
            get => this.backingDictionary[key];
            set
            {
                this.Remove(key);

                this.Add(key, value);
            }
        }

        private readonly IDictionary<TKey, TValue> backingDictionary;

        private readonly ListFile backingFile;

        private readonly SerializationSettings<TKey> KeySerialization;

        private readonly SerializationSettings<TValue> ValueSerialization;

        public int Count => this.backingDictionary.Count;

        public bool IsReadOnly => this.backingDictionary.IsReadOnly;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => backingDictionary.Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => backingDictionary.Values;

        public DictionaryFile(string path, SerializationSettings<TKey> keySerialization, SerializationSettings<TValue> valueSerialization, bool autoflush = true)
        {
            this.KeySerialization = keySerialization;
            this.ValueSerialization = valueSerialization;

            this.backingFile = new ListFile(path, autoflush);
            this.backingDictionary = new Dictionary<TKey, TValue>();

            foreach (string line in this.backingFile)
            {
                this.backingDictionary.Add(KeySerialization.Deserialize(line.Split('\t')[0]), ValueSerialization.Deserialize(line.Split('\t')[1]));
            }
        }

        public void Flush()
        {
            this.backingFile.Flush();
        }
        public void Add(TKey key, TValue value)
        {
            this.backingDictionary.Add(key, value);
            this.backingFile.Add(GetRow(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.backingDictionary.Add(item);
            this.backingFile.Add(GetRow(item));
        }

        public void Clear()
        {
            this.backingDictionary.Clear();
            this.backingFile.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.backingDictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return this.backingDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.backingDictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.backingDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.backingDictionary).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            bool v = this.backingDictionary.Remove(key);

            string sKey = KeySerialization.Serialize(key);

            foreach (string line in this.backingFile)
            {
                if (line.StartsWith($"{sKey}\t"))
                {
                    this.backingFile.Remove(line);
                    break;
                }
            }
            return v;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool v = this.backingDictionary.Remove(item);
            this.backingFile.Remove($"{item.Key}\t{item.Value}");
            return v;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.backingDictionary.TryGetValue(key, out value);
        }

        private string GetRow(TKey key, TValue value)
        {
            return $"{KeySerialization.Serialize(key)}\t{ValueSerialization.Serialize(value)}";
        }

        private string GetRow(KeyValuePair<TKey, TValue> item)
        {
            return GetRow(item.Key, item.Value);
        }
    }
}