using System.Collections;
using System.Collections.Generic;

namespace Penguin.Collections
{
    public class DictionaryFile : IDictionary<string, string>
    {
        public DictionaryFile(string path)
        {
            this.backingFile = new ListFile(path);
            this.backingDictionary = new Dictionary<string, string>();
            
            foreach (string line in this.backingFile)
            {
                this.backingDictionary.Add(line.Split('\t')[0], line.Split('\t')[1]);
            }
        }

        private readonly ListFile backingFile;

        private readonly IDictionary<string, string> backingDictionary;

        public ICollection<string> Keys => this.backingDictionary.Keys;

        public ICollection<string> Values => this.backingDictionary.Values;

        public int Count => this.backingDictionary.Count;

        public bool IsReadOnly => this.backingDictionary.IsReadOnly;

        public string this[string key] { get => this.backingDictionary[key]; set => this.backingDictionary[key] = value; }

        public void Add(string key, string value)
        {
            this.backingDictionary.Add(key, value);
            this.backingFile.Add($"{key}\t{value}");
        }

        public bool ContainsKey(string key)
        {
            return this.backingDictionary.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            bool v = this.backingDictionary.Remove(key);
            foreach(string line in this.backingFile)
            {
                if (line.StartsWith($"{key}\t"))
                {
                    this.backingFile.Remove(line);
                    break;
                }
            }
            return v;
        }

        public bool TryGetValue(string key, out string value)
        {
            return this.backingDictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            this.backingDictionary.Add(item);
            this.backingFile.Add($"{item.Key}\t{item.Value}");
        }

        public void Clear()
        {
            this.backingDictionary.Clear();
            this.backingFile.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return this.backingDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            this.backingDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            bool v = this.backingDictionary.Remove(item);
            this.backingFile.Remove($"{item.Key}\t{item.Value}");
            return v;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.backingDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.backingDictionary).GetEnumerator();
        }
    }
}
